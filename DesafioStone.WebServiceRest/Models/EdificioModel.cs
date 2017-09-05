using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DesafioStone.WebServiceRest.Models
{
    public class EdificioModel
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public string UF { get; set; }
        public string Pais { get; set; }
        public List<NivelModel> ListNivel { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime UltimaAtualizacao { get; set; }
    }
}