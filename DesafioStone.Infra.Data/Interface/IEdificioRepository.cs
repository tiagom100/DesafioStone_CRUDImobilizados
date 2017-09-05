using DesafioStone.Domain.Entity;
using MongoDB.Driver;
using System.Collections.Generic;

namespace DesafioStone.Infra.Data.Interface
{
    public interface IEdificioRepository
    {
        IMongoCollection<Edificio> Edificios { get; }

        System.Threading.Tasks.Task<List<Edificio>> GetAllAsync();

        Edificio GetByIdAsync(string id);

        void AddAsync(Edificio edificio);

        System.Threading.Tasks.Task<Edificio> Update(string id, Edificio edificio);

        System.Threading.Tasks.Task<Edificio> UpdateAsync(FilterDefinition<Edificio> filter, UpdateDefinition<Edificio> update);

        System.Threading.Tasks.Task<Edificio> RemoveAsync(string id);
    }
}