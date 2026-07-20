
namespace Defra.Imports.IntegrationTests.Dynamics.Autonumber.Scenarios
{
    using Defra.Imports.Model;
    using Marktek.Fluent.Testing.Engine.Interfaces;
    using System;

    class SetAutonumberValue : IWaitableAction
    {
        private readonly ImportsContext context;
        private readonly Guid autonumberId;
        private readonly int value;

        public SetAutonumberValue(ImportsContext context, Guid autonumberId, int value)
        {
            this.context = context;
            this.autonumberId = autonumberId;
            this.value = value;
        }

        public void Execute()
        {
            defraimp_autonumber importApplicationToUpdate = new defraimp_autonumber
            {
                Id = this.autonumberId,
                defraimp_CurrentNumber = this.value,
            };

            if (!this.context.IsAttached(importApplicationToUpdate))
            {
                this.context.Attach(importApplicationToUpdate);
            }

            this.context.UpdateObject(importApplicationToUpdate);
            this.context.SaveChanges();
        }
    }
}
