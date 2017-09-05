using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DesafioStone.WebServiceRest.Models
{
    public class NivelModel
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string EdificioId { get; set; }        
        public DateTime DataCadastro { get; set; }
        public DateTime UltimaAtualizacao { get; set; }
    }
}