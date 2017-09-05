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
    public class ItemController : ApiController
    {
        private IItemRepository _itemRepository;
        public ItemController(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        [AcceptVerbs("GET")]
        public async Task<IHttpActionResult> ObterTodosItensAsync()
        {
            try
            {
                var result = await _itemRepository.GetAllAsync();

                if (result != null)
                {
                    return ResponseMessage(Request.CreateResponse<List<Item>>(HttpStatusCode.OK, result));
                }

                return ResponseMessage(Request.CreateResponse<List<Item>>(HttpStatusCode.OK, result));
            }
            catch (Exception erro)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocorreu um erro ao tentar processar a requisição."));
            }
        }

        [AcceptVerbs("GET")]
        public IHttpActionResult ObterItemById(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.NotAcceptable, "Necessário informar parâmetro Id."));

            try
            {
                var result = _itemRepository.GetById(id);

                if (result != null)
                {
                    return ResponseMessage(Request.CreateResponse<Item>(HttpStatusCode.OK, result));
                }

                ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.NotFound, "Item não localizado."));

                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            catch (Exception erro)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocorreu um erro ao tentar processar a requisição."));
            }
        }

        [AcceptVerbs("GET")]
        public async Task<IHttpActionResult> ObterItensImobilizadosAsync()
        {
            try
            {
                var result = await _itemRepository.ObterItensImobilizadosAsync();

                return ResponseMessage(Request.CreateResponse<List<Item>>(HttpStatusCode.OK, result));
            }
            catch (Exception erro)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocorreu um erro ao tentar processar a requisição."));
            }
        }

        [AcceptVerbs("GET")]
        public async Task<IHttpActionResult> ObterItensLivresAsync()
        {
            try
            {
                var result = await _itemRepository.ObterItensLivresAsync();

                return ResponseMessage(Request.CreateResponse<List<Item>>(HttpStatusCode.OK, result));
            }
            catch (Exception erro)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocorreu um erro ao tentar processar a requisição."));
            }
        }

        [AcceptVerbs("GET")]
        public async Task<IHttpActionResult> ObterItensImobilizadosByNivelIdAsync(string nivelId)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(nivelId))
                    return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.NotAcceptable, "Necessário informar parâmetro nivelId."));

                var result = await _itemRepository.ObterItensImobilizadosByNivelIdAsync(nivelId);

                return ResponseMessage(Request.CreateResponse<List<Item>>(HttpStatusCode.OK, result));
            }
            catch (Exception erro)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocorreu um erro ao tentar processar a requisição."));
            }
        }

        [AcceptVerbs("POST")]
        public IHttpActionResult Criar([FromBody]ItemModel obj)
        {
            try
            {
                if (obj == null || String.IsNullOrWhiteSpace(obj.Nome) || String.IsNullOrWhiteSpace(obj.Descricao))
                    return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.NotAcceptable, "Necessário informar os campos: Nome e Descricao."));

                try
                {
                    Item item = new Item(obj.Nome,obj.Descricao);

                    _itemRepository.AddAsync(item);

                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.Created));
                }
                catch (Exception erro)
                {

                }

                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotModified));
            }
            catch (Exception erro)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocorreu um erro ao tentar processar a requisição."));
            }
        }

        [AcceptVerbs("POST")]
        public async Task<IHttpActionResult> ImobilizarItemAsync(string itemId, string nivelId)
        {
            if (String.IsNullOrWhiteSpace(nivelId) || String.IsNullOrWhiteSpace(itemId))
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.NotAcceptable, "Necessário informar parâmetro itemId e nivelId."));
            try
            {
                var result = await _itemRepository.ImobilizarItemAsync(itemId, nivelId);

                if (result != null)
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, "Item Imoblizado com sucesso"));
                else
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, "Não foi possível concluir a requisição, Item está imobilizado em outro nível."));
            }
            catch (Exception erro)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocorreu um erro ao tentar processar a requisição."));
            }
        }

        [AcceptVerbs("POST")]
        public IHttpActionResult LiberarItem(string itemId)
        {
            if (String.IsNullOrWhiteSpace(itemId))
                return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.NotAcceptable, "Necessário informar parâmetro itemId."));
            try
            {
                var result = _itemRepository.LiberarItemAsync(itemId);

                if(result != null)
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, "Item liberado com sucesso"));
                else
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, "Item não foi localizado."));
            }
            catch (Exception erro)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocorreu um erro ao tentar processar a requisição."));
            }
        }

        [AcceptVerbs("PUT")]
        public IHttpActionResult Atualizar(string id, [FromBody]ItemModel obj)
        {
            try
            {
                Item item = new Item(obj.Id, obj.Nome, obj.Descricao);

                var result = _itemRepository.UpdateAsync(id, item);

                if (result != null)
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK));
                else
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, "Item não localizado."));
            }
            catch (Exception erro)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocorreu um erro ao tentar processar a requisição."));
            }
        }

        [AcceptVerbs("DELETE")]
        public IHttpActionResult Delete(string itemId)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(itemId))
                    return ResponseMessage(Request.CreateResponse<string>(HttpStatusCode.NotAcceptable, "Necessário informar parâmetro itemId."));

                var result = _itemRepository.RemoveAsync(itemId);

                if (result != null)
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK));
                else
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, "Item não localizado."));
            }
            catch (Exception erro)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, "Ocorreu um erro ao tentar processar a requisição."));
            }
        }
    }
}