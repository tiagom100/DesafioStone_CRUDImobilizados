using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DesafioStone.Domain.Entity
{
    public class Nivel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; private set; }

        [BsonRepresentation(BsonType.String)]
        public string Nome { get; private set; }

        [BsonRepresentation(BsonType.String)]
        public string Descricao { get; private set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string EdificioId { get; private set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime DataCadastro { get; private set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime UltimaAtualizacao { get; private set; }

        public Nivel(string nome, string descricao)
        {
            this.Nome = nome;
            this.Descricao = descricao;
            this.DataCadastro = DateTime.Now;
            this.UltimaAtualizacao = DateTime.Now;
        }

        public Nivel(string id, string nome, string descricao)
        {
            this.Nome = nome;
            this.Descricao = descricao;
            this.UltimaAtualizacao = DateTime.Now;
        }

        public void SetEdifioId(string edificioId)
        {           
            this.EdificioId = edificioId;
        }
    }
}