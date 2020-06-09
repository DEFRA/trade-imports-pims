using Defra.Imports.Repositories;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Defra.Imports.BusinessLogic.Utils
{
    class WebResourceXmlRetriever : IXmlRetriever
    {
        private IRepositoryFactory _repositoryFactory;
        private EntityReference _xmlWebResourceRef;

        public WebResourceXmlRetriever(IRepositoryFactory repositoryFactory, EntityReference xmlWebResourceRef)
        {
            _repositoryFactory = repositoryFactory;
            _xmlWebResourceRef = xmlWebResourceRef;
        }

        public string GetXml()
        {
            string webResourceContent = "";
            // Retrieve the mapping web resource
            ICrmRepository webResourceRepository = _repositoryFactory.GetRepository(_xmlWebResourceRef.LogicalName);
            Entity mappingWebResource = webResourceRepository.Retrieve(_xmlWebResourceRef.Id, new string[] { "content" });

            if (mappingWebResource.Attributes.Contains("content"))
            {
                byte[] webResourceBytes = Convert.FromBase64String(mappingWebResource.Attributes["content"].ToString());
                webResourceContent = Encoding.UTF8.GetString(webResourceBytes);
                string byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
                if (webResourceContent.StartsWith(byteOrderMarkUtf8, StringComparison.Ordinal))
                {
                    webResourceContent = webResourceContent.Remove(0, byteOrderMarkUtf8.Length);
                }
            }

            return webResourceContent;
        }
    }
}
