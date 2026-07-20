namespace Defra.Imports.BusinessLogic.ImportApplication
{
    using Defra.Imports.BusinessLogic.Logging;
    using Defra.Imports.Model;
    using Defra.Imports.Repositories;

    class InspectionsSinceLastCheckCounterManagerBusinessLogic
    {
        defraimp_importapplication _preImageImportApplication;
        defraimp_importapplication _postOperationImportApplication;
        IPlaceOfOriginRepository _placeOfOriginRepo;
        ILogWriter _logWriter;

        public InspectionsSinceLastCheckCounterManagerBusinessLogic(defraimp_importapplication preImageImportApplication, defraimp_importapplication postOperationImportApplication, IPlaceOfOriginRepository placeOfOriginRepo, ILogWriter logWriter)
        {
            _preImageImportApplication = preImageImportApplication;
            _postOperationImportApplication = postOperationImportApplication;
            _placeOfOriginRepo = placeOfOriginRepo;
            _logWriter = logWriter;
        }

        public void RunLogic()
        {
            defraimp_placeoforigin preImagePlaceOfOrigin = null;
            defraimp_placeoforigin postOperationPlaceOfOrigin = null;

            // Ensure we have a pre-image import application. We won't receive this on create.
            if (_preImageImportApplication != null)
            {
                if (_preImageImportApplication.defraimp_PlaceofOriginid != null)
                {
                    preImagePlaceOfOrigin = _placeOfOriginRepo.Find(_preImageImportApplication.defraimp_PlaceofOriginid.Id);
                }
            }

            // Check we have a post-operation import application. We should always receive this except on delete.
            if (_postOperationImportApplication != null)
            {
                if (_postOperationImportApplication.defraimp_PlaceofOriginid != null)
                {
                    postOperationPlaceOfOrigin = _placeOfOriginRepo.Find(_postOperationImportApplication.defraimp_PlaceofOriginid.Id);
                }
            }

            // Does both the PreImage and Post Operation Import Application have Places of Origin?
            if (preImagePlaceOfOrigin != null && postOperationPlaceOfOrigin != null)
            {
                // Are we moving to completion?
                if (_preImageImportApplication.defraimp_MovetoCompletion == false && _postOperationImportApplication.defraimp_MovetoCompletion == true)
                {
                    // Was the outcome satisfactory or unsatisfactory? In other words did we do an inspection?
                    if (_postOperationImportApplication.defraimp_InspectionOutcome == defraimp_importapplication_defraimp_inspectionoutcome.Satisfactory || _postOperationImportApplication.defraimp_InspectionOutcome == defraimp_importapplication_defraimp_inspectionoutcome.Unsatisfactory)
                    {
                        // Set 0 as a check has occurred
                        _placeOfOriginRepo.SetNumberOfRecordsSinceLastCheckValue(postOperationPlaceOfOrigin.Id, 0);
                    } // Else a check has not actually ocurred so increment
                    else if (_postOperationImportApplication.defraimp_InspectionOutcome == defraimp_importapplication_defraimp_inspectionoutcome.AwaitingResultofChecks || _postOperationImportApplication.defraimp_InspectionOutcome == defraimp_importapplication_defraimp_inspectionoutcome.NotVisited || _postOperationImportApplication.defraimp_InspectionOutcome == null)
                    {
                        // Increment
                        _placeOfOriginRepo.IncrementNumberOfRecordsSinceLastCheck(postOperationPlaceOfOrigin.Id);
                    }
                } // Are we reverting the move to completion?
                else if (_preImageImportApplication.defraimp_MovetoCompletion == true && _postOperationImportApplication.defraimp_MovetoCompletion == false)
                {
                    // Have we gone back a step and need to remove the value?
                    if (_postOperationImportApplication.defraimp_InspectionOutcome == defraimp_importapplication_defraimp_inspectionoutcome.AwaitingResultofChecks || _postOperationImportApplication.defraimp_InspectionOutcome == null)
                    {
                        // Decrement
                        _placeOfOriginRepo.DecrementNumberOfRecordsSinceLastCheck(postOperationPlaceOfOrigin.Id);
                    }
                }
            }
        }
    }
}
