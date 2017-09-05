using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace DesafioStone.Domain.Entity
{
    public class Edificio
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; private set; }

        [BsonRepresentation(BsonType.String)]
        public string Nome { get; private set; }

        [BsonRepresentation(BsonType.String)]
        public string Endereco { get; private set; }

        [BsonRepresentation(BsonType.String)]
        public string UF { get; private set; }

        [BsonRepresentation(BsonType.String)]
        public string Pais { get; private set; }

        [BsonElement("Niveis")]
        public List<Nivel> ListNivel { get; private set; }

        [BsonRepresentation(BsonType.Boolean)]
        public bool Ativo { get; private set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime DataCadastro { get; private set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime UltimaAtualizacao { get; private set; }

        public Edificio(string nome, string endereco, string uf, string pais)
        {
            this.Nome = nome;
            this.Endereco = endereco;
            this.UF = uf;
            this.Pais = pais;
            this.DataCadastro = DateTime.Now;
            this.UltimaAtualizacao = DateTime.Now;
            this.Ativo = true;
        }

        //RemoverNivel De Edificio 
        public Edificio(string id, string endereco, string nome, string uf, string pais)
        {
            this.Nome = nome;
            this.Endereco = endereco;
            this.UF = uf;
            this.Pais = pais;
            this.UltimaAtualizacao = DateTime.Now;
            this.ListNivel = new List<Nivel>();
        }

        public void AdicionarNivel(Nivel nivel)
        {
            this.ListNivel.Add(nivel);
        }

        public void RemoverNivelById(string nivelId)
        {

            var index = ListNivel.FindIndex(x => x.Id == nivelId);
            if (index >= 0) ListNivel.RemoveAt(index);
        }

        public void InstanciarListNivel()
        {
            this.ListNivel = new List<Nivel>();
        }

        public void Ativar()
        {
            this.Ativo = true;
        }

        public void Desativar()
        {
            this.Ativo = false;
        }
    }
}