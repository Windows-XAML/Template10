using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Models;

namespace Template10.Repositories
{
    public class TodoItemRepository
    {
        readonly string CACHEKEY;
        Services.FileService.FileService _fileService;

        public TodoItemRepository()
        {
            CACHEKEY = string.Format("CACHE {0}", this.GetType());
            _fileService = new Services.FileService.FileService();
        }

        List<Models.TodoItem> _cache;
        public async Task<List<Models.TodoItem>> GetAsync()
        {
            return _cache ?? (_cache = await _fileService.ReadAsync<Models.TodoItem>(CACHEKEY) ?? new List<TodoItem>());
        }

        public async Task<Models.TodoItem> GetAsync(string key)
        {
            return (await this.GetAsync()).FirstOrDefault(x => x.Key.Equals(key));
        }

        public async Task SaveAsync(List<Models.TodoItem> items)
        {
            if (items != null)
                await _fileService.WriteAsync<Models.TodoItem>(CACHEKEY, items);
        }

        public Models.TodoItem Factory(string key = null, States? state = null, string title = null)
        {
            return new Models.TodoItem
            {
                Key = key ?? Guid.NewGuid().ToString(),
                State = state ?? States.NotStarted,
                Title = title ?? string.Empty,
            };
        }

        public IEnumerable<Models.TodoItem> Sample(int count = 5)
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
                yield return new Models.TodoItem
                {
                    Key = Guid.Empty.ToString(),
                    Title = Guid.NewGuid().ToString(),
                    State = state.Invoke(),
                    DueDate = DateTime.Now.AddHours(random.Next(1, 200)),
                };
            }
        }
    }
}
