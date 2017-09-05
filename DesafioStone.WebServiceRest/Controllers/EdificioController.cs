using DesafioStone.Domain.Entity;
using DesafioStone.Infra.Data.Interface;
using DesafioStone.Infra.Data.Repositories;
using DesafioStone.WebServiceRest.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DesafioStone.WebServiceRest.Controllers
{
    public class EdificioController : ApiController
    {
        private IEdificioRepository _edificioRepository;
        public EdificioController(IEdificioRepository edificioRepository)
        {
            _edificioRepository = edificioRepository;
        }

        [AcceptVerbs("GET")]
        public async Task<IHttpActionResult> ObterTodosEdificiosAsync()
        {
            try
            {
                var result = await _edificioRepository.GetAllAsync();

                return ResponseMessage(Request.CreateResponse<List<Edificio>>(HttpStatusCode.OK, result));
            }
            catch (Exception erro)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocorreu um erro ao tentar processar a requisição."));
            }
        }

        [AcceptVerbs("GET")]
        public IHttpActionResult ObterEdificioById(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.NotAcceptable, "Necessário informar parâmetro Id."));

            try
            {
                var result = _edificioRepository.GetByIdAsync(id);

                if (result != null)
                {
                    return ResponseMessage(Request.CreateResponse<Edificio>(HttpStatusCode.OK, result));
                }

                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.NotFound, "Edificio não localizado."));
            }
            catch (Exception erro)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocorreu um erro ao tentar processar a requisição."));
            }
        }

        [AcceptVerbs("POST")]
        public IHttpActionResult Criar([FromBody]EdificioModel obj)
        {
            if (String.IsNullOrWhiteSpace(obj.Nome) || String.IsNullOrWhiteSpace(obj.Endereco) || String.IsNullOrWhiteSpace(obj.UF) || String.IsNullOrWhiteSpace(obj.Pais))
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.NotAcceptable, "Necessário informar campos: Nome, Endereco, UF e Pais."));

            try
            {
                Edificio edificio = new Edificio(obj.Nome, obj.Endereco, obj.UF, obj.Pais);

                _edificioRepository.AddAsync(edificio);
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.Created));
            }
            catch (Exception erro)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocorreu um erro ao tentar processar a requisição."));
            }
        }

        [AcceptVerbs("PUT")]
        public async Task<IHttpActionResult> AtualizarAsync(string id, [FromBody]EdificioModel obj)
        {
            if (String.IsNullOrWhiteSpace(id) || String.IsNullOrWhiteSpace(obj.Nome) || String.IsNullOrWhiteSpace(obj.Endereco) || String.IsNullOrWhiteSpace(obj.UF) || String.IsNullOrWhiteSpace(obj.Pais))
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.NotAcceptable, "Necessário informar campos: Id , Nome, Endereco, UF e Pais."));

            try
            {
                Edificio edificio = new Edificio(obj.Id, obj.Nome, obj.Endereco, obj.UF, obj.Pais);

                var result = await _edificioRepository.Update(id, edificio);

                if (result != null)
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK));
                else
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, "Edificio não localizado."));
            }
            catch (Exception erro)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocorreu um erro ao tentar processar a requisição."));
            }

        }

        [AcceptVerbs("DELETE")]
        public async Task<IHttpActionResult> DeleteAsync(string edificioId)
        {
            if (String.IsNullOrWhiteSpace(edificioId))
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.NotAcceptable, "Necessário informar parâmetro Id."));

            try
            {

                var result = await _edificioRepository.RemoveAsync(edificioId);

                if (result != null)
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK));
                else
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, "Edificio não localizado."));
            }
            catch (Exception erro)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocorreu um erro ao tentar processar a requisição."));
            }
        }

        //Implementar exclusão lógica ou manter exclusão normal?
    }
}