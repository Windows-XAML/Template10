using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Models;

namespace Template10.Repositories
{
    public class TodoItemRepository
    {
        public Models.TodoItem Factory(string key = null, States? state = null, string title = null, DateTime? dueDate = null)
        {
            return new Models.TodoItem
            {
                Key = key ?? Guid.NewGuid().ToString(),
                State = state ?? States.NotStarted,
                Title = title ?? string.Empty,
            };
        }

        public Models.TodoItem Clone(Models.TodoItem item)
        {
            return Factory
                (
                    Guid.Empty.ToString(),
                    item.State,
                    item.Title,
                    item.DueDate
                );
        }

        public IEnumerable<Models.TodoItem> Sample(int count = 5)
        {
            var random = new Random((int)DateTime.Now.Ticks);
            Func<States> state = () =>
            {
                switch (random.Next(0, 4))
                {
                    case 1: return States.Done;
                    case 2: return States.InProcess;
                    default: return States.NotStarted;
                }
            };
            foreach (var item in Enumerable.Range(1, count))
            {
                yield return Factory
                    (
                        Guid.Empty.ToString(),
                        state.Invoke(),
                        Guid.NewGuid().ToString(),
                        DateTime.Now.AddHours(random.Next(1, 200))
                    );
            }
        }
    }
}
