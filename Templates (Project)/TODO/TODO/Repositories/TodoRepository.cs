using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Models;

namespace Template10.Repositories
{
    public class TodoRepository
    {
        readonly string CACHEKEY;
        Services.FileService.FileService _fileService;

        public TodoRepository()
        {
            CACHEKEY = string.Format("CACHE {0}", this.GetType());
            _fileService = new Services.FileService.FileService();
        }

        List<Models.Todo> _cache;
        public async Task<List<Models.Todo>> GetAsync()
        {
            return _cache ?? (_cache = await _fileService.ReadAsync<Models.Todo>(CACHEKEY) ?? new List<Todo>());
        }

        public async Task<Models.Todo> GetAsync(string key)
        {
            return (await this.GetAsync()).FirstOrDefault(x => x.Key.Equals(key));
        }

        public async Task SaveAsync(List<Models.Todo> items)
        {
            if (items != null)
                await _fileService.WriteAsync<Models.Todo>(CACHEKEY, items);
        }

        public Models.Todo Factory(string key = null, States? state = null, string title = null)
        {
            return new Models.Todo
            {
                Key = key ?? Guid.NewGuid().ToString(),
                State = state ?? States.NotStarted,
                Title = title ?? string.Empty,
            };
        }

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
