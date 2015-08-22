using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Models;

namespace Template10.Repositories
{
    public class TodoItemRepository
    {
        public Models.TodoItem Factory(string key = null, bool? complete = null, string title = null, DateTime? dueDate = null)
        {
            return new Models.TodoItem
            {
                Key = key ?? Guid.NewGuid().ToString(),
                IsComplete = complete ?? false,
                Title = title ?? string.Empty,
            };
        }

        public Models.TodoItem Clone(Models.TodoItem item)
        {
            return Factory
                (
                    Guid.Empty.ToString(),
                    false,
                    item.Title,
                    item.DueDate
                );
        }

        public IEnumerable<Models.TodoItem> Sample(int count = 5)
        {
            var random = new Random((int)DateTime.Now.Ticks);
            foreach (var item in Enumerable.Range(1, count))
            {
                yield return Factory
                    (
                        Guid.NewGuid().ToString(),
                        false,
                        "Task-" + Guid.NewGuid().ToString(),
                        DateTime.Now.AddHours(random.Next(1, 200))
                    );
            }
        }
    }
}
