using System.Collections.Generic;

namespace Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.IdentificationOfAnimalsObjects
{
    public class IdentificationOfAnimals
    {
        public int complementID { get; set; }

        public string speciesID { get; set; }

        public List<KeyDataPair> keyDataPair { get; set; }

        public List<Identifiers> identifiers { get; set; }
    }
}
