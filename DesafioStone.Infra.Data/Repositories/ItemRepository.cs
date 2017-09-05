using DesafioStone.Domain.Entity;
using DesafioStone.Infra.Data.Interface;
using MongoDB.Driver;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace DesafioStone.Infra.Data.Repositories
{
    public class ItemRepository : IItemRepository
    {
        public IMongoDatabase Database;
        public String DataBaseName = "DesafioStone";
        string conexaoMongoDB = "";

        public ItemRepository()
        {
            conexaoMongoDB = ConfigurationManager.AppSettings["DesafioStoneMongoDB"];
            var cliente = new MongoClient(conexaoMongoDB);
            Database = cliente.GetDatabase(DataBaseName);

            var a = Database.ListCollections();
        }

        public IMongoCollection<Item> Itens
        {
            get
            {
                var Itens = Database.GetCollection<Item>("Item");
                return Itens;
            }
        }

        public async void AddAsync(Item item)
        {
            await Itens.InsertOneAsync(item);
        }

        public async Task<List<Item>> GetAllAsync()
        {
            List<Item> listItem = new List<Item>();
            var filter = new BsonDocument();

            using (var cursor = await Itens.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (var document in batch)
                    {
                        listItem.Add(document);
                    }
                }
            }

            return listItem;
        }

        public Item GetById(string id)
        {
            var filter = Builders<Item>.Filter.Eq(x => x.Id, id);

            var result = Itens.Find(filter).First();

            return result;
        }

        //Testar esse método aqui.
        //Retornar Item ou Msg.
        public async Task<Item> ImobilizarItemAsync(string itemId, string nivelId)
        {
            INivelRepository nivelRepository = new NivelRepository();

            var filterItem = Builders<Item>.Filter.Eq(x => x.Id, itemId) & Builders<Item>.Filter.Exists("Imobilizado", false);

            var resultNivel = nivelRepository.GetById(nivelId);

            var update = Builders<Item>.Update
             .Set("Imobilizado.NivelId", nivelId)
             .Set("Imobilizado.Nivel", resultNivel.Nome)
             .Set("Imobilizado.Data", DateTime.Now);

            var result = await Itens.FindOneAndUpdateAsync(filterItem, update);

            return result;
        }

        public async Task<Item> LiberarItemAsync(string id)
        {
            var filter = Builders<Item>.Filter.Eq(x => x.Id, id);

            var update = Builders<Item>.Update
             .Unset("Imobilizado");

            var result = await Itens.FindOneAndUpdateAsync(filter, update);

            return result;
        }

        public async Task<List<Item>> ObterItensImobilizadosAsync()
        {
            List<Item> listItem = new List<Item>();
            var filter = Builders<Item>.Filter.Exists("Imobilizado");

            using (var cursor = await Itens.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (var document in batch)
                    {
                        listItem.Add(document);
                    }
                }
            }

            return listItem;
        }

        public async Task<List<Item>> ObterItensImobilizadosByNivelIdAsync(string nivelId)
        {
            List<Item> listItem = new List<Item>();
            var filter = Builders<Item>.Filter.Exists("Imobilizado") & Builders<Item>.Filter.Eq("Imobilizado.NivelId", nivelId);

            using (var cursor = await Itens.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (var document in batch)
                    {
                        listItem.Add(document);
                    }
                }
            }

            return listItem;
        }

        public async Task<List<Item>> ObterItensLivresAsync()
        {
            List<Item> listItem = new List<Item>();
            var filter = Builders<Item>.Filter.Exists("Imobilizado", false);

            using (var cursor = await Itens.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (var document in batch)
                    {
                        listItem.Add(document);
                    }
                }
            }

            return listItem;
        }

        public async Task<Item> RemoveAsync(string id)
        {
            var filter = Builders<Item>.Filter.Eq(x => x.Id, id);
            var result = await Itens.FindOneAndDeleteAsync(filter);

            return result;
        }

        public async Task<Item> UpdateAsync(string id, Item item)
        {
            var filter = Builders<Item>.Filter.Eq(x => x.Id, id);

            var update = Builders<Item>.Update
                .Set(x => x.Nome, item.Nome)
                .Set(x => x.Descricao, item.Descricao)
                .Set(x => x.UltimaAtualizacao, item.UltimaAtualizacao);

            var result = await Itens.FindOneAndUpdateAsync(filter, update);

            return result;
        }
    }
}