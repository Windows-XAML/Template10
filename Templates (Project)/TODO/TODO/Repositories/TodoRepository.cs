using System;
using System.Collections.Generic;
using System.Linq;
using Template10.Models;

namespace Template10.Repositories
{
    public class TodoRepository : RepositoryBase<Models.Todo>
    {
        public TodoRepository()
            : base("Todo-Cache")
        { }

        public IEnumerable<Models.Todo> Sample(int count = 5)
        {
            var random = new Random((int)DateTime.Now.Ticks);
            Func<States> state = () =>
            {
                switch (random.Next(1, 3))
                {
                    case 1: return States.Done;
                    case 2: return States.InProcess;
                    default: return States.NotStarted;
                }
            };
            foreach (var item in Enumerable.Range(1, count))
            {
                yield return new Models.Todo
                {
                    Key = Guid.Empty.ToString(),
                    Title = Guid.NewGuid().ToString(),
                    State = state.Invoke(),
                };
            }
        }
    }
}
