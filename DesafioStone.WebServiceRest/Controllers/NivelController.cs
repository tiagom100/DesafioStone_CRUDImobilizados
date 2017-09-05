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
    public class NivelController : ApiController
    {
        private INivelRepository _nivelRepository;
        public NivelController(INivelRepository nivelRepository)
        {
            _nivelRepository = nivelRepository;
        }

        [AcceptVerbs("GET")]
        public async Task<IHttpActionResult> ObterTodosNiveisAsync()
        {
            try
            {
                var result = await _nivelRepository.GetAllAsync();

                return ResponseMessage(Request.CreateResponse<List<Nivel>>(HttpStatusCode.OK, result));
            }
            catch (Exception erro)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocorreu um erro ao tentar processar a requisição."));
            }
        }

        [AcceptVerbs("GET")]
        public IHttpActionResult ObterNivelById(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.NotAcceptable, "Necessário informar parâmetro Id."));

            try
            {
                var result = _nivelRepository.GetById(id);

                if (result != null)
                {
                    return ResponseMessage(Request.CreateResponse<Nivel>(HttpStatusCode.OK, result));
                }

                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.NotFound, "Edificio não localizado."));
            }
            catch (Exception erro)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocorreu um erro ao tentar processar a requisição."));
            }
        }

        [AcceptVerbs("POST")]
        public IHttpActionResult Criar([FromBody]NivelModel obj)
        {
            if (obj == null || String.IsNullOrWhiteSpace(obj.EdificioId) || String.IsNullOrWhiteSpace(obj.Id) || String.IsNullOrWhiteSpace(obj.Nome) || String.IsNullOrWhiteSpace(obj.Descricao))
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.NotAcceptable, "Necessário informar campos: Id, EdificioId, Nome e Descricao."));

            try
            {
                Nivel nivel = new Nivel(obj.Nome, obj.Descricao);
                nivel.SetEdifioId(obj.EdificioId);

                _nivelRepository.AddAsync(nivel);
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.Created));
            }
            catch (Exception erro)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocorreu um erro ao tentar processar a requisição."));
            }
        }

        [AcceptVerbs("PUT")]
        public IHttpActionResult Atualizar(string id, [FromBody]NivelModel obj)
        {
            if (String.IsNullOrWhiteSpace(id) || obj == null || String.IsNullOrWhiteSpace(obj.Nome) || String.IsNullOrWhiteSpace(obj.Descricao))
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.NotAcceptable, "Necessário informar campos: Id, Nome e Descricao."));

            try
            {
                Nivel nivel = new Nivel(obj.Id, obj.Nome, obj.Descricao);

                var result = _nivelRepository.Update(id, nivel);

                if (result != null)
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK));
                else
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, "Nivel não localizado."));
            }
            catch (Exception erro)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocorreu um erro ao tentar processar a requisição."));
            }
        }

        [AcceptVerbs("DELETE")]
        public IHttpActionResult Delete(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.NotAcceptable, "Necessário informar parâmetro Id."));

            try
            {
                var result = _nivelRepository.RemoveAsync(id);
                //var filter = Builders<Nivel>.Filter.Eq(x => x.Id, id);
                //var resultNivel = await _nivelRepository.Niveis.FindOneAndDeleteAsync(filter);

                //if(resultNivel != null)
                //    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, "Nivel não localizado."));

                //var builder = Builders<Edificio>.Filter;
                //var filterEdificio = builder.Eq(x => x.Id, resultNivel.EdificioId);

                //var filterNivel = builder.Eq("Id", resultNivel.Id);

                //var result = await EdificioDb.Edificios.FindOneAndUpdateAsync(filterEdificio, Builders<Edificio>.Update.PullFilter("Niveis", filterNivel));

                //var filterItem = Builders<Item>.Filter.Eq("Imobilizado.NivelId", resultNivel.Id);

                //var update = Builders<Item>.Update
                // .Unset("Imobilizado");

                //await ItemDb.Itens.FindOneAndUpdateAsync(filterItem, update);
                if (result != null)
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK));
                else
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, "Nivel não localizado."));
            }
            catch (Exception erro)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocorreu um erro ao tentar processar a requisição."));
            }
        }
    }
}