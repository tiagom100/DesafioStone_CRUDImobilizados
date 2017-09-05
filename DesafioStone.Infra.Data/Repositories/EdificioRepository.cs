using DesafioStone.Domain.Entity;
using DesafioStone.Infra.Data.Interface;
using MongoDB.Driver;
using System;
using System.Configuration;
using System.Collections.Generic;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace DesafioStone.Infra.Data.Repositories
{
    public class EdificioRepository : IEdificioRepository
    {
        public IMongoDatabase Database;
        public String DataBaseName = "DesafioStone";
        string conexaoMongoDB = "";

        public EdificioRepository()
        {
            conexaoMongoDB = ConfigurationManager.AppSettings["DesafioStoneMongoDB"];
            var cliente = new MongoClient(conexaoMongoDB);
            Database = cliente.GetDatabase(DataBaseName);

            var a = Database.ListCollections();
        }

        public IMongoCollection<Edificio> Edificios
        {
            get
            {
                var Edificios = Database.GetCollection<Edificio>("Edificio");
                return Edificios;
            }
        }

        public async void AddAsync(Edificio edificio)
        {
            await this.Edificios.InsertOneAsync(edificio);
        }

        public async Task<List<Edificio>> GetAllAsync()
        {
            List<Edificio> listEdificio = new List<Edificio>();
            var filter = new BsonDocument();

            using (var cursor = await Edificios.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (var document in batch)
                    {
                        listEdificio.Add(document);
                    }
                }
            }

            return listEdificio;
        }

        public Edificio GetByIdAsync(string id)
        {
            var filter = Builders<Edificio>.Filter.Eq(x => x.Id, id);

            var result = Edificios.Find(filter).First();

            return result;
        }

        public async Task<Edificio> RemoveAsync(string id)
        {
            IItemRepository itemRepository = new ItemRepository();
            INivelRepository iNivelRepository = new NivelRepository();

            var filterEdificio = Builders<Edificio>.Filter.Eq(x => x.Id, id);
            var result = await Edificios.FindOneAndDeleteAsync(filterEdificio);

            var filterNivel = Builders<Nivel>.Filter.Eq(x => x.EdificioId, id);

            if (result.ListNivel != null)
            {
                foreach (var nivel in result.ListNivel)
                {
                    var filter = Builders<Item>.Filter.Eq("Imobilizado.NivelId", nivel.Id);

                    var update = Builders<Item>.Update
                     .Unset("Imobilizado");

                    await itemRepository.Itens.FindOneAndUpdateAsync(filter, update);
                }
            }

            await iNivelRepository.Niveis.DeleteManyAsync(filterNivel);

            return result;
        }

        public async Task<Edificio> Update(string id, Edificio edificio)
        {
            var update = Builders<Edificio>.Update
                .Set(x => x.Nome, edificio.Nome)
                .Set(x => x.UF, edificio.UF)
                .Set(x => x.UF, edificio.Pais)
                .Set(x => x.UltimaAtualizacao, edificio.UltimaAtualizacao);

            var filter = Builders<Edificio>.Filter.Eq(x => x.Id, id);

            return await Edificios.FindOneAndUpdateAsync(filter, update);
        }

        public async Task<Edificio> UpdateAsync(FilterDefinition<Edificio> filter, UpdateDefinition<Edificio> update)
        {
            var result = await Edificios.FindOneAndUpdateAsync(filter, update);
            return result;
        }
    }
}