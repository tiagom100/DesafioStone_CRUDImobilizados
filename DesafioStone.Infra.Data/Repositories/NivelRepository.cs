using DesafioStone.Domain.Entity;
using DesafioStone.Infra.Data.Interface;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace DesafioStone.Infra.Data.Repositories
{
    public class NivelRepository : INivelRepository
    {
        public IMongoDatabase Database;
        public String DataBaseName = "DesafioStone";
        string conexaoMongoDB = "";

        public NivelRepository()
        {
            conexaoMongoDB = ConfigurationManager.AppSettings["DesafioStoneMongoDB"];
            var cliente = new MongoClient(conexaoMongoDB);
            Database = cliente.GetDatabase(DataBaseName);

            var a = Database.ListCollections();
        }

        public IMongoCollection<Nivel> Niveis
        {
            get
            {
                var Niveis = Database.GetCollection<Nivel>("Nivel");
                return Niveis;
            }
        }

        public async void AddAsync(Nivel nivel)
        {
            IEdificioRepository edificioRepository = new EdificioRepository();

            var filterEdificio = Builders<Edificio>.Filter.Eq(x => x.Id, nivel.EdificioId);
            var resultEdificio = edificioRepository.GetByIdAsync(nivel.EdificioId);

            if (resultEdificio != null)
            {
                await Niveis.InsertOneAsync(nivel);

                if (resultEdificio.ListNivel == null)
                    resultEdificio.InstanciarListNivel();

                var filterNivel = Builders<Nivel>.Filter.Eq(x => x.EdificioId, resultEdificio.Id);

                var resultNivel = await Niveis.FindAsync(filterNivel);

                while (await resultNivel.MoveNextAsync())
                {
                    var batch = resultNivel.Current;
                    foreach (var document in batch)
                    {
                        resultEdificio.AdicionarNivel(document);
                    }
                }

                var updateEdificio = Builders<Edificio>.Update
                .Set(x => x.ListNivel, resultEdificio.ListNivel);

                await edificioRepository.UpdateAsync(filterEdificio, updateEdificio);
            }
        }

        public async Task<List<Nivel>> GetAllAsync()
        {
            List<Nivel> listNivel = new List<Nivel>();
            var filter = new BsonDocument();

            using (var cursor = await Niveis.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    var batch = cursor.Current;
                    foreach (var document in batch)
                    {
                        listNivel.Add(document);
                    }
                }
            }

            return listNivel;
        }

        public Nivel GetById(string id)
        {
            var filter = Builders<Nivel>.Filter.Eq(x => x.Id, id);

            var result = Niveis.Find(filter).First();

            return result;
        }

        public async Task<Nivel> RemoveAsync(string id)
        {
            IItemRepository itemRepository = new ItemRepository();
            IEdificioRepository edificioRepository = new EdificioRepository();

            var filter = Builders<Nivel>.Filter.Eq(x => x.Id, id);
            var resultNivel = await Niveis.FindOneAndDeleteAsync(filter);

            if (resultNivel != null)
                return null;

            var builder = Builders<Edificio>.Filter;
            var filterEdificio = builder.Eq(x => x.Id, resultNivel.EdificioId);

            var filterNivel = builder.Eq("Id", resultNivel.Id);

            var result = await edificioRepository.Edificios.FindOneAndUpdateAsync(filterEdificio, Builders<Edificio>.Update.PullFilter("Niveis", filterNivel));

            var filterItem = Builders<Item>.Filter.Eq("Imobilizado.NivelId", resultNivel.Id);

            var update = Builders<Item>.Update
             .Unset("Imobilizado");

            await itemRepository.Itens.FindOneAndUpdateAsync(filterItem, update);

            return resultNivel;
        }

        public async Task<Nivel> Update(string id, Nivel nivel)
        {
            IEdificioRepository EdificioRepository = new EdificioRepository();

            var filterNivel = Builders<Nivel>.Filter.Eq(x => x.Id, id);

            var updateNivel = Builders<Nivel>.Update
                .Set(x => x.Nome, nivel.Nome)
                .Set(x => x.Descricao, nivel.Descricao)
                .Set(x => x.UltimaAtualizacao, nivel.UltimaAtualizacao);

            var result = await Niveis.FindOneAndUpdateAsync(filterNivel, updateNivel);

            var filterEdificio = Builders<Edificio>.Filter.Eq(x => x.Id, result.EdificioId) & Builders<Edificio>.Filter.Eq("Niveis._id", new ObjectId(result.Id));

            var updateEdificio = Builders<Edificio>.Update
                .Set("Niveis.$.Nome", nivel.Nome)
                .Set("Niveis.$.Descricao", nivel.Descricao)
                .Set("Niveis.$.UltimaAtualizacao", nivel.UltimaAtualizacao);

            await EdificioRepository.UpdateAsync(filterEdificio, updateEdificio);

            return result;
        }

    }
}