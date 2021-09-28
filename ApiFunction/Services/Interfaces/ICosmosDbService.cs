using ApiFunctions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiFunctions.Services.Interfaces
{
    public interface ICosmosDbService<T> where T : BaseEntity
    {
        Task<T> AddAsync(T newEntity);
        Task<T> GetAsync(string entityId);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(string entityId);
        Task<IReadOnlyList<T>> GetAllAsync();
    }
}
