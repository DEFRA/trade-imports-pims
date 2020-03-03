using Defra.Imports.Repositories;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Defra.Imports.BusinessLogic.Utils
{
  public class MapFieldsFromOneRecordToAnotherBusinessLogic
  {
    private IRepositoryFactory _repositoryFactory;
    private EntityReference _entityFromRef;
    private EntityReference _entityToRef;
    private EntityReference _mappingWebResourceRef;
    private bool _shouldOverwriteExisting;

    public MapFieldsFromOneRecordToAnotherBusinessLogic(IRepositoryFactory repositoryFactory, EntityReference entityFromRef, EntityReference entityToRef, EntityReference mappingWebResourceRef, bool shouldOverwriteExisting)
    {
      _repositoryFactory = repositoryFactory;
      _entityFromRef = entityFromRef;
      _entityToRef = entityToRef;
      _mappingWebResourceRef = mappingWebResourceRef;
      _shouldOverwriteExisting = shouldOverwriteExisting;
    }

    public void RunLogic()
    {
      // Retrieve the mapping web resource
      ICrmRepository webResourceRepository = _repositoryFactory.GetRepository(_mappingWebResourceRef.LogicalName);
      Entity mappingWebResource = webResourceRepository.Retrieve(_mappingWebResourceRef.Id, new string[] { "content" });

      if (mappingWebResource.Attributes.Contains("content"))
      {
        byte[] webResourceBytes = Convert.FromBase64String(mappingWebResource.Attributes["content"].ToString());
        string webResourceContent = Encoding.UTF8.GetString(webResourceBytes);
        string byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
        if (webResourceContent.StartsWith(byteOrderMarkUtf8, StringComparison.Ordinal))
        {
          webResourceContent = webResourceContent.Remove(0, byteOrderMarkUtf8.Length);
        }

        // Parse the web resource to a dictionary
        IFieldMappingConfigParser fieldMappingConfigReader = new FieldMappingConfigParser();
        Dictionary<string, string> fieldMappingConfig = fieldMappingConfigReader.ParseMappingConfig(webResourceContent);

        // Retrieve the entity to map from
        ICrmRepository entityToMapFromRepo = _repositoryFactory.GetRepository(_entityFromRef.LogicalName);
        Entity entityToMapFrom = entityToMapFromRepo.Retrieve(_entityFromRef.Id, fieldMappingConfig.Keys.ToArray());

        // Retrieve the entity to map to
        ICrmRepository entityToMapToRepo = _repositoryFactory.GetRepository(_entityToRef.LogicalName);
        Entity entityToMapTo = entityToMapToRepo.Retrieve(_entityToRef.Id, fieldMappingConfig.Values.ToArray());

        // Pass each of these to the mapper
        IGenericEntityMapper<Entity, Entity> genericEntityMapper = new GenericEntityMapper<Entity, Entity>(fieldMappingConfig);

        bool shouldOverwriteFields = _shouldOverwriteExisting;
        if (shouldOverwriteFields)
        {
          entityToMapTo = genericEntityMapper.MapAllFields(entityToMapFrom, entityToMapTo);
        }
        else
        {
          entityToMapTo = genericEntityMapper.MapEmptyFields(entityToMapFrom, entityToMapTo);
        }

        // Perform an update on the map to entity
        entityToMapToRepo.Update(entityToMapTo);
      }
    }
  }

}
