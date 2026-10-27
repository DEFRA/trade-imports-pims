namespace Defra.Imports.UnitTests.Support
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Messages;
    using Microsoft.Xrm.Sdk.Query;
    using Moq;

    // A strict organisation service over an in-memory store, for tests of logic that interleaves reads and writes.
    // Queries (sent by the early-bound LINQ provider through Execute) match on equality conditions, and writes
    // change the store, so later reads see them. Every call is recorded, in order, in Events.
    internal sealed class InspectionDataverse
    {
        private readonly Dictionary<string, List<Entity>> store = new Dictionary<string, List<Entity>>();

        public InspectionDataverse()
        {
            this.Service.Setup(service => service.Execute(It.IsAny<OrganizationRequest>())).Returns<OrganizationRequest>(this.Execute);
            this.Service
                .Setup(service => service.Retrieve(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<ColumnSet>()))
                .Returns<string, Guid, ColumnSet>(this.Retrieve);
            this.Service.Setup(service => service.Update(It.IsAny<Entity>())).Callback<Entity>(this.Update);
            this.Service.Setup(service => service.Create(It.IsAny<Entity>())).Returns<Entity>(this.Create);
        }

        public Mock<IOrganizationService> Service { get; } = new Mock<IOrganizationService>(MockBehavior.Strict);

        // Every query, retrieve, update and create in order, as "Query:table", "Retrieve:table", "Update:table" and "Create:table".
        public List<string> Events { get; } = new List<string>();

        // Copies of the entities sent to Update, in order.
        public List<Entity> Updates { get; } = new List<Entity>();

        // Copies of the entities sent to Create, in order.
        public List<Entity> Creates { get; } = new List<Entity>();

        public InspectionDataverse Seed(params Entity[] entities)
        {
            foreach (var entity in entities)
            {
                this.Table(entity.LogicalName).Add(Copy(entity));
            }

            return this;
        }

        // Replaces the stored entity that has the same identifier, as a change made outside the logic under test.
        public InspectionDataverse Replace(Entity entity)
        {
            var table = this.Table(entity.LogicalName);
            table.RemoveAll(candidate => candidate.Id == entity.Id);
            table.Add(Copy(entity));
            return this;
        }

        public T Stored<T>(Guid id)
            where T : Entity
        {
            return this.Table(Activator.CreateInstance<T>().LogicalName).Single(entity => entity.Id == id).ToEntity<T>();
        }

        public IReadOnlyList<T> StoredRecords<T>()
            where T : Entity
        {
            return this.Table(Activator.CreateInstance<T>().LogicalName).Select(entity => entity.ToEntity<T>()).ToList();
        }

        public IReadOnlyList<Entity> UpdatesOf(string logicalName)
        {
            return this.Updates.Where(entity => entity.LogicalName == logicalName).ToList();
        }

        private static Entity Copy(Entity entity)
        {
            var copy = (Entity)Activator.CreateInstance(entity.GetType());
            foreach (var attribute in entity.Attributes)
            {
                copy[attribute.Key] = attribute.Value;
            }

            copy.LogicalName = entity.LogicalName;
            if (copy.Id != entity.Id)
            {
                copy.Id = entity.Id;
            }

            return copy;
        }

        private static bool Matches(Entity entity, FilterExpression filter)
        {
            var conditions = filter.Conditions.Select(condition => Matches(entity, condition));
            var children = filter.Filters.Select(child => Matches(entity, child));
            var all = conditions.Concat(children).ToList();
            return filter.FilterOperator == LogicalOperator.Or ? all.Any(match => match) : all.All(match => match);
        }

        private static bool Matches(Entity entity, ConditionExpression condition)
        {
            if (condition.Operator != ConditionOperator.Equal)
            {
                throw new NotSupportedException("The in-memory store supports only Equal conditions, not " + condition.Operator + ".");
            }

            var expected = condition.Values.Single();
            if (condition.AttributeName == entity.LogicalName + "id")
            {
                return entity.Id.Equals(expected);
            }

            if (!entity.Attributes.TryGetValue(condition.AttributeName, out var actual) || actual == null)
            {
                return false;
            }

            if (actual is EntityReference reference)
            {
                return reference.Id.Equals(expected);
            }

            if (actual is OptionSetValue option)
            {
                return option.Value == Convert.ToInt32(expected);
            }

            return actual.Equals(expected);
        }

        private List<Entity> Table(string logicalName)
        {
            if (!this.store.TryGetValue(logicalName, out var table))
            {
                table = new List<Entity>();
                this.store[logicalName] = table;
            }

            return table;
        }

        private OrganizationResponse Execute(OrganizationRequest request)
        {
            var query = (QueryExpression)((RetrieveMultipleRequest)request).Query;
            this.Events.Add("Query:" + query.EntityName);

            var matches = this.Table(query.EntityName).Where(entity => Matches(entity, query.Criteria)).Select(Copy).ToList();
            if (query.TopCount.HasValue)
            {
                matches = matches.Take(query.TopCount.Value).ToList();
            }

            return new RetrieveMultipleResponse
            {
                Results = new ParameterCollection
                {
                    { "EntityCollection", new EntityCollection(matches) { EntityName = query.EntityName } },
                },
            };
        }

        private Entity Retrieve(string logicalName, Guid id, ColumnSet columns)
        {
            this.Events.Add("Retrieve:" + logicalName);

            var found = Copy(this.Table(logicalName).Single(entity => entity.Id == id));
            if (!columns.AllColumns)
            {
                foreach (var name in found.Attributes.Keys.Where(key => !columns.Columns.Contains(key) && key != logicalName + "id").ToList())
                {
                    found.Attributes.Remove(name);
                }
            }

            return found;
        }

        private void Update(Entity entity)
        {
            this.Events.Add("Update:" + entity.LogicalName);
            this.Updates.Add(Copy(entity));

            var stored = this.Table(entity.LogicalName).SingleOrDefault(candidate => candidate.Id == entity.Id);
            if (stored != null)
            {
                foreach (var attribute in entity.Attributes)
                {
                    stored[attribute.Key] = attribute.Value;
                }
            }
        }

        private Guid Create(Entity entity)
        {
            this.Events.Add("Create:" + entity.LogicalName);
            this.Creates.Add(Copy(entity));

            var created = Copy(entity);
            if (created.Id == Guid.Empty)
            {
                created.Id = Guid.NewGuid();
            }

            this.Table(entity.LogicalName).Add(created);
            return created.Id;
        }
    }
}
