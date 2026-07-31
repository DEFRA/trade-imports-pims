namespace Defra.Imports.IntegrationTests.Dynamics.ImporterNotification.Assertions.Validators
{
    using System;
    using System.Linq;
    using Defra.Imports.Model;
    using FluentAssertions;
    using MarkTek.Fluent.Testing.RecordGeneration;

    public class LinkedWatchFlagValidator : ISpecificationValidator<defraimp_ImporterNotification>
    {
        private ImportsContext context;
        private Guid watchListId;

        public LinkedWatchFlagValidator(ImportsContext context, Guid watchListId)
        {
            this.context = context;
            this.watchListId = watchListId;
        }

        /// <inheritdoc/>
        public void Validate(defraimp_ImporterNotification item)
        {
            defraimp_WatchFlag watchFlag = this.context.defraimp_WatchFlagSet.FirstOrDefault(x => x.defraimp_ImporterNotificationId.Id == item.Id && x.defraimp_WatchListId.Id == this.watchListId);
            watchFlag.Should().NotBeNull();
        }
    }
}
