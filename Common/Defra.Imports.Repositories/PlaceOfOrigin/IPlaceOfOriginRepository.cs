namespace Defra.Imports.Repositories
{

    using System;
    using Defra.Imports.Model;

    public interface IPlaceOfOriginRepository
    {
        int GetApplicationCounterValue(Guid placeOfOriginId);

        int GetQuotaCounterValue(Guid placeOfOriginId);

        int GetPrimaryITAHCCounterValue(Guid placeOfOriginId);

        defraimp_placeoforigin Find(Guid placeOfOriginId);

        void IncrementApplicationCounter(Guid placeOfOriginId);

        void SetApplicationCounter(Guid placeOfOriginId, int value);

        void DecrementApplicationCounter(Guid placeOfOriginId);

        void IncrementQuotaCounter(Guid placeOfOriginId);

        void DecrementQuotaCounter(Guid placeOfOriginId);

        void SetQuotaCounter(Guid placeOfOriginId, int value);

        void IncrementNumberOfPrimaryITAHCCounter(Guid placeOfOriginId);

        void DecrementNumberOfPrimaryITAHCCounter(Guid placeOfOriginId);

        void SetNumberOfRecordsSinceLastCheckValue(Guid placeOfOriginId, int value);

        void IncrementNumberOfRecordsSinceLastCheck(Guid placeOfOriginId);

        void DecrementNumberOfRecordsSinceLastCheck(Guid placeOfOriginId);
    }
}
