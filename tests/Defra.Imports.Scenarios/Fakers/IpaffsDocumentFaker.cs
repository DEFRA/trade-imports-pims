namespace Defra.Imports.Scenarios.Fakers
{
    using Defra.Imports.Model;

    /// <summary>
    /// A <see cref="RecordFaker{TEntity}"/> for <see cref="defraimp_ipaffsdocument"/> records - the child record the EU
    /// Imports Notifications Logic App creates per <c>veterinaryInformation.accompanyingDocuments</c> entry - see
    /// <c>Create_a_new_record_-_Document</c> in <c>EUImportsNotifications.logicapp.json</c>.
    /// Does not set <see cref="defraimp_ipaffsdocument.defraimp_ImporterNotificationId"/> - association with the parent
    /// notification is left to the deep-insert relationship on <see cref="defraimp_ImporterNotification"/> instead.
    /// </summary>
    public class IpaffsDocumentFaker : RecordFaker<defraimp_ipaffsdocument>
    {
        private static readonly defraimp_ipaffsdocument_defraimp_documenttype[] DocumentTypes =
        {
            defraimp_ipaffsdocument_defraimp_documenttype.ITAHC,
            defraimp_ipaffsdocument_defraimp_documenttype.DOCOM,
            defraimp_ipaffsdocument_defraimp_documenttype.CommercialInvoice,
            defraimp_ipaffsdocument_defraimp_documenttype.Other,
        };

        private static readonly string[] AttachmentContentTypes = { "application/pdf", "image/png" };

        /// <summary>
        /// Initializes a new instance of the <see cref="IpaffsDocumentFaker"/> class.
        /// </summary>
        public IpaffsDocumentFaker()
        {
            this.RuleFor(d => d.defraimp_DocumentType, f => f.PickRandom(DocumentTypes));
            this.RuleFor(d => d.defraimp_DocumentReference, f => f.Random.Replace("DOC-#####"));
            this.RuleFor(d => d.defraimp_DocumentIssueDate, f => f.Date.Recent(30));

            this.RuleFor(d => d.defraimp_AttachmentId, f => f.Random.Guid().ToString());
            this.RuleFor(d => d.defraimp_AttachmentContentType, f => f.PickRandom(AttachmentContentTypes));
            this.RuleFor(d => d.defraimp_name, (f, d) => f.Random.Replace("document-#####" + (d.defraimp_AttachmentContentType == "image/png" ? ".png" : ".pdf")));

            // Simplified placeholder - the real URL is derived from the IPAFFS base URL configuration parameter and the
            // parent notification's reference number, neither of which this record-level faker has visibility of.
            this.RuleFor(d => d.defraimp_DocumentUrl, f => f.Internet.Url());
        }
    }
}
