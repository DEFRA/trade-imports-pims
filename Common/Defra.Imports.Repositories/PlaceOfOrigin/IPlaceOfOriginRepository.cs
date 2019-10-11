namespace Defra.Imports.Repositories
{

    using System;
    using Defra.Imports.Model;

    interface IPlaceOfOriginRepository
    {
        defraimp_placeoforigin GetPlaceOfOrigin(Guid placeOfOriginId);

        int GetCounterValue(Guid placeOfOriginId);

        void IncrementCounter(Guid placeOfOriginId);

        void DecrementCounter(Guid placeOfOriginId);

        void SetCounterValue(Guid placeOfOriginId);
    }
}
