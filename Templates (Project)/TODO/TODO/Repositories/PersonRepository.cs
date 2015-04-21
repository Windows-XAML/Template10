using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Template10.Repositories
{
    public class PersonRepository : RepositoryBase<Models.Person>
    {
        Repositories.TodoRepository _todoRepository;

        public PersonRepository() 
            : base("Person-Cache")
        {
            _todoRepository = new TodoRepository();
        }

        public IEnumerable<Models.Person> Sample(int count = 5)
        {
            foreach (var item in Enumerable.Range(1, count))
            {
                yield return new Models.Person
                {
                    Key = Guid.Empty.ToString(),
                    Name = Guid.NewGuid().ToString(),
                };
            }
        }
    }
}
