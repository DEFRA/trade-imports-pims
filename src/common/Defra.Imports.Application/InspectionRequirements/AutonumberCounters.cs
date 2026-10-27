namespace Defra.Imports.Application.InspectionRequirements
{
    using System;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Queries;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// Reads and changes the autonumber records that hold the inspection counters. Each change reads the record
    /// and then sends an explicit update.
    /// </summary>
    public sealed class AutonumberCounters
    {
        private readonly AutonumberQueries queries;
        private readonly IOrganizationService service;

        /// <summary>Initializes a new instance of the <see cref="AutonumberCounters"/> class.</summary>
        /// <param name="queries">The queries used to find autonumbers.</param>
        /// <param name="service">The current invocation's organisation service, used for updates.</param>
        public AutonumberCounters(AutonumberQueries queries, IOrganizationService service)
        {
            this.queries = queries ?? throw new ArgumentNullException(nameof(queries));
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>Gets the autonumber with a key. The first match is used if there are several.</summary>
        /// <param name="key">The key.</param>
        /// <returns>The autonumber, or null if there is none.</returns>
        public defraimp_autonumber GetRecord(string key)
        {
            return this.queries.FindByKey(key);
        }

        /// <summary>Gets the current number of an autonumber.</summary>
        /// <param name="key">The key.</param>
        /// <returns>The current number, or zero if it has none.</returns>
        /// <exception cref="InvalidPluginExecutionException">There is no autonumber with the key.</exception>
        public int GetValue(string key)
        {
            var record = this.GetRecord(key);
            if (record != null)
            {
                return record.defraimp_CurrentNumber ?? 0;
            }

            throw new InvalidPluginExecutionException("Autonumber record key not found.");
        }

        /// <summary>Increases the number of an autonumber by one and updates it.</summary>
        /// <param name="key">The key.</param>
        public void Increment(string key)
        {
            var record = this.GetRecord(key);
            record.defraimp_CurrentNumber++;
            this.service.Update(record);
        }

        /// <summary>Decreases the number of an autonumber by one and updates it.</summary>
        /// <param name="key">The key.</param>
        public void Decrement(string key)
        {
            var record = this.GetRecord(key);
            record.defraimp_CurrentNumber--;
            this.service.Update(record);
        }

        /// <summary>Sets the number of an autonumber and updates it.</summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The new number.</param>
        public void Set(string key, int value)
        {
            var record = this.GetRecord(key);
            record.defraimp_CurrentNumber = value;
            this.service.Update(record);
        }
    }
}
