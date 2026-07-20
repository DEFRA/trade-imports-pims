namespace Defra.Imports.IntegrationTests.Dynamics.ImporterNotification.Assertions.Validators
{
    using Defra.Imports.Model;
    using FluentAssertions;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using System;
    using System.Linq;

    public class LinkedWatchFlagValidator : ISpecificationValidator<defraimp_ImporterNotification>
    {
        private ImportsContext context;
        private Guid watchListId;

        public LinkedWatchFlagValidator(ImportsContext context, Guid watchListId)
        {
            this.context = context;
            this.watchListId = watchListId;
        }

        public void Validate(defraimp_ImporterNotification item)
        {
            defraimp_WatchFlag watchFlag = this.context.defraimp_WatchFlagSet.FirstOrDefault(x => x.defraimp_ImporterNotificationId.Id == item.Id && x.defraimp_WatchListId.Id == this.watchListId);
            watchFlag.Should().NotBeNull();
        }
    }
}
