using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DesafioStone.Domain.Entity
{
    public class Item
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; private set; }

        [BsonRepresentation(BsonType.String)]
        public string Nome { get; private set; }

        [BsonRepresentation(BsonType.String)]
        public string Descricao { get; private set; }

        //Caso não exista, está livre, se não, está localizado no nível.
       
        public Nivel Nivel { get; private set; }

        [BsonIgnoreIfNull]
        public Object Imobilizado { get; private set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime DataCadastro { get; private set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime UltimaAtualizacao { get; private set; }

        public Item(string nome, string descricao)
        {
            this.Nome = nome;
            this.Descricao = descricao;
            this.DataCadastro = DateTime.Now;
            this.UltimaAtualizacao = DateTime.Now;
        }

        public Item(string id, string nome, string descricao)
        {
            this.Nome = nome;
            this.Descricao = descricao;
            this.UltimaAtualizacao = DateTime.Now;
        }
    }
}