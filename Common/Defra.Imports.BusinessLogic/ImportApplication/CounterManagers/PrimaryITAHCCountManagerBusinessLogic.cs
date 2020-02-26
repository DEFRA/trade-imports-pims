namespace Defra.Imports.BusinessLogic.ImportApplication
{
    using Defra.Imports.BusinessLogic.ImportApplication.Contexts;
    using Defra.Imports.BusinessLogic.ImportApplication.Factories;
    using Defra.Imports.BusinessLogic.Logging;
    using Defra.Imports.BusinessLogic.RepoInterfaces;
    using Defra.Imports.Model;
    using Defra.Imports.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Text;

    class PrimaryITAHCCountManagerBusinessLogic
    {
        defraimp_importapplication _preImageImportApplication;
        defraimp_importapplication _postOperationImportApplication;
        IPlaceOfOriginRepository _placeOfOriginRepo;
        ILogWriter _logWriter;

        public PrimaryITAHCCountManagerBusinessLogic(defraimp_importapplication preImageImportApplication, defraimp_importapplication postOperationImportApplication, IPlaceOfOriginRepository placeOfOriginRepo, ILogWriter logWriter)
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
                // Did the record have an ITAHC but no longer has one?
                if (_preImageImportApplication.defraimp_PrimaryITAHCId != null && _postOperationImportApplication.defraimp_PrimaryITAHCId == null)
                {
                    // Decrement the number of primary ITAHCs
                    _placeOfOriginRepo.DecrementNumberOfPrimaryITAHCCounter(preImagePlaceOfOrigin.Id);
                } // Have we added an ITAHC?
                else if (_preImageImportApplication.defraimp_PrimaryITAHCId == null && _postOperationImportApplication.defraimp_PrimaryITAHCId != null)
                {
                    // increment the counter
                    _placeOfOriginRepo.IncrementNumberOfPrimaryITAHCCounter(postOperationPlaceOfOrigin.Id);
                } // Else if both have an ITAHC
                else if (_preImageImportApplication.defraimp_PrimaryITAHCId != null && _postOperationImportApplication.defraimp_PrimaryITAHCId != null)
                {
                    // Has the Place of Origin changed?
                    if (preImagePlaceOfOrigin != postOperationPlaceOfOrigin)
                    {
                        _placeOfOriginRepo.DecrementNumberOfPrimaryITAHCCounter(preImagePlaceOfOrigin.Id);
                        _placeOfOriginRepo.IncrementNumberOfPrimaryITAHCCounter(postOperationPlaceOfOrigin.Id);
                    }
                }
            } // Have we added a new place of origin on this create/update?
            else if (preImagePlaceOfOrigin == null && postOperationPlaceOfOrigin != null)
            {
                // Do we currently have an ITAHC?
                if (_postOperationImportApplication.defraimp_PrimaryITAHCId != null)
                {
                    // We've added a new place of origin to a record with a valid ITAHC, increment the counter
                    _placeOfOriginRepo.IncrementNumberOfPrimaryITAHCCounter(postOperationPlaceOfOrigin.Id);
                }
            } // Else have removed a place of origin?
            else if (preImagePlaceOfOrigin != null && postOperationPlaceOfOrigin == null)
            {
                // Did we have an ITAHC?
                if (_preImageImportApplication.defraimp_PrimaryITAHCId != null)
                {
                    // We've removed the place of origin and we had an ITAHC previously, so decrement the counter
                    _placeOfOriginRepo.DecrementNumberOfPrimaryITAHCCounter(preImagePlaceOfOrigin.Id);
                }
            }
        }
    }
}
