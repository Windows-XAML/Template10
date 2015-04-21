using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Template10.Repositories
{
    public class GroupRepository : RepositoryBase<Models.Group>
    {
        Repositories.TodoRepository _todoRepository;

        public GroupRepository() 
            : base("Group-Cache")
        {
            _todoRepository = new TodoRepository();
        }

        public IEnumerable<Models.Group> Sample(int count = 5)
        {
            foreach (var item in Enumerable.Range(1, count))
            {
                yield return new Models.Group
                {
                    Title = Guid.NewGuid().ToString(),
                    Items = new ObservableCollection<Models.Todo>(_todoRepository.Sample(5)),
                };
            }
        }
    }
}
