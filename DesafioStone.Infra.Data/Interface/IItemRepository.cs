using DesafioStone.Domain.Entity;
using MongoDB.Driver;
using System.Collections.Generic;

namespace DesafioStone.Infra.Data.Interface
{
    public interface IItemRepository
    {
        IMongoCollection<Item> Itens { get; }

        System.Threading.Tasks.Task<List<Item>> GetAllAsync();

        Item GetById(string id);

        System.Threading.Tasks.Task<List<Item>> ObterItensImobilizadosAsync();

        System.Threading.Tasks.Task<List<Item>> ObterItensLivresAsync();

        System.Threading.Tasks.Task<List<Item>> ObterItensImobilizadosByNivelIdAsync(string nivelId);

        void AddAsync(Item edificio);

        System.Threading.Tasks.Task<Item> ImobilizarItemAsync(string itemId, string nivelId);

        System.Threading.Tasks.Task<Item> LiberarItemAsync(string id);  

        System.Threading.Tasks.Task<Item> UpdateAsync(string id, Item item);

        System.Threading.Tasks.Task<Item> RemoveAsync(string id);
    }
}