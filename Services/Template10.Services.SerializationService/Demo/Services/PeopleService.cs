using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Services.SerializationService;

namespace SerializationService.Demo.Services
{
    public sealed class PeopleService
    {
        public static PeopleService Instance { get; } =
            new PeopleService();

        private PeopleService()
        {

        }

        private static ISerializationService SerializationService { get; } =
            Template10.Services.SerializationService.SerializationService.Json;

        public string SerializePerson(Models.Person person)
        {
            return SerializationService.Serialize(person);
        }

        public Models.Person DeserializePerson(string json)
        {
            return SerializationService.Deserialize<Models.Person>(json);
        }
    }
}
