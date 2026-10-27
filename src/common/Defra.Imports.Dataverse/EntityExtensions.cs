namespace Defra.Imports.Dataverse
{
    using System;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// Hand-written helpers for cloning entities and calculating the values that changed. They live outside the
    /// generated <c>Defra.Imports.Dataverse.Model</c> namespace so that regeneration cannot overwrite them.
    /// </summary>
    public static class EntityExtensions
    {
        /// <summary>
        /// Creates a shallow copy of the entity. The attribute collection is new, so adding or replacing values
        /// never touches the source. Attribute values are shared, so mutable values must be replaced, not mutated.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="entity">The entity to copy.</param>
        /// <returns>A new entity with the same identifier and attribute values.</returns>
        public static T Clone<T>(this T entity)
            where T : Entity, new()
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var clone = new T();
            foreach (var attribute in entity.Attributes)
            {
                clone[attribute.Key] = attribute.Value;
            }

            // A parameterless Entity has no logical name, and setting an empty identifier on an early-bound
            // type would add a primary key attribute that the source does not have.
            clone.LogicalName = entity.LogicalName;
            if (clone.Id != entity.Id)
            {
                clone.Id = entity.Id;
            }

            return clone;
        }

        /// <summary>
        /// Gets an entity holding only the values that are new or different, suitable for an Update request.
        /// Values are compared using <see cref="object.Equals(object, object)"/>.
        /// </summary>
        /// <param name="updated">The entity containing the desired values.</param>
        /// <param name="original">The entity to compare against.</param>
        /// <returns>A new entity containing only the new or different values.</returns>
        public static Entity GetChanges(this Entity updated, Entity original)
        {
            if (updated == null)
            {
                throw new ArgumentNullException(nameof(updated));
            }

            if (original == null)
            {
                throw new ArgumentNullException(nameof(original));
            }

            var changes = new Entity(updated.LogicalName, updated.Id);
            foreach (var attribute in updated.Attributes)
            {
                if (!original.Attributes.TryGetValue(attribute.Key, out var value)
                    || !Equals(value, attribute.Value))
                {
                    changes[attribute.Key] = attribute.Value;
                }
            }

            return changes;
        }
    }
}
