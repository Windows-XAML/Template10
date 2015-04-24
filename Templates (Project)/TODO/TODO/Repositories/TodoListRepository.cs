using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Models;

namespace Template10.Repositories
{
    public class TodoListRepository
    {
        List<Models.TodoList> _cache;
        const string cachekey = "cache-todolist";
        Services.FileService.FileService _fileService;
        Repositories.TodoItemRepository _todoItemRepository;

        public TodoListRepository()
        {
            _fileService = new Services.FileService.FileService();
            _todoItemRepository = new TodoItemRepository();
        }

        public async Task<List<Models.TodoList>> GetAsync()
        {
            return _cache ?? (_cache = await _fileService.ReadAsync<Models.TodoList>(cachekey) ?? new List<TodoList>());
        }

        public async Task<Models.TodoList> GetAsync(string key)
        {
            return (await this.GetAsync()).FirstOrDefault(x => x.Key.Equals(key));
        }

        public async Task SaveAsync(List<Models.TodoList> list)
        {
            await _fileService.WriteAsync<Models.TodoList>(cachekey, list);
        }

        public Models.TodoList Factory(string key = null, string title = null, IEnumerable<Models.TodoItem> items = null)
        {
            return new Models.TodoList
            {
                Key = key ?? Guid.NewGuid().ToString(),
                Title = title ?? Guid.NewGuid().ToString(),
                Items = new System.Collections.ObjectModel.ObservableCollection<TodoItem>(items ?? new Models.TodoItem[] { }),
            };
        }

        public Models.TodoList Clone(Models.TodoList list)
        {
            return Factory
                (
                    null,
                    list.Title,
                    list.Items.Select(x => _todoItemRepository.Clone(x))
                );
        }

        public IEnumerable<Models.TodoList> Sample(int count = 5)
        {
            var random = new Random((int)DateTime.Now.Ticks);
            foreach (var List in Enumerable.Range(1, count))
            {
                yield return Factory
                    (
                        Guid.NewGuid().ToString(),
                        "List-" + Guid.NewGuid().ToString(),
                        _todoItemRepository.Sample(random.Next(5, 30))
                    );
            }
        }
    }
}
