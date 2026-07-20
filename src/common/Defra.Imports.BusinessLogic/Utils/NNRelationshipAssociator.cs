using Defra.Imports.Model;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace Defra.Imports.BusinessLogic.Utils
{
    class NNRelationshipAssociator : InnRelationshipAssociator
    {
        EntityReference _sourceEntity;
        EntityReferenceCollection _recordsToRelate;
        IOrganizationService _orgSvc;

        public NNRelationshipAssociator(EntityReference sourceEntity, List<Entity> recordsToRelate, IOrganizationService orgSvc)
        {
            _recordsToRelate = new EntityReferenceCollection();

            foreach (Entity entity in recordsToRelate)
            {
                _recordsToRelate.Add(new EntityReference(entity.LogicalName, entity.Id));
            }

            _sourceEntity = sourceEntity;
            _orgSvc = orgSvc;
        }

        public void RunLogic()
        {
            if (_recordsToRelate.Count > 0)
            {
                Relationship rel = new Relationship(nameof(defraimp_PotentiallyRelatedImportRecords));
                _orgSvc.Associate(_sourceEntity.LogicalName, _sourceEntity.Id, rel, _recordsToRelate);
            }
        }

    }
}
