using DesafioStone.Domain.Entity;
using MongoDB.Driver;
using System.Collections.Generic;

namespace DesafioStone.Infra.Data.Interface
{
    public interface INivelRepository
    {
        IMongoCollection<Nivel> Niveis { get; }

        System.Threading.Tasks.Task<List<Nivel>> GetAllAsync();

        Nivel GetById(string id);

        void AddAsync(Nivel edificio);

        System.Threading.Tasks.Task<Nivel> Update(string id, Nivel edificio);

        System.Threading.Tasks.Task<Nivel> RemoveAsync(string id);
    }
}