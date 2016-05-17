using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sample.Models;

namespace Sample.Services.DataService
{
    public interface IDataService<T>
    {
        Task<T> AddAsync(DataItem item);
        Task<T> DeleteAsync(DataItem item);
        Task<IReadOnlyList<T>> LoadAsync();
        Task<bool> SaveAsync();
    }
}