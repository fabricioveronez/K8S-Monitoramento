using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Metricas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {

        private IMongoCollection<Pedido> _pedidos;

        public PedidoController(IMongoCollection<Pedido> pedidos)
        {
            this._pedidos = pedidos;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Pedido>> Get()
        {
            try
            {
                return this._pedidos.Find(FilterDefinition<Pedido>.Empty).ToList();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }

        [HttpGet("error")]
        public ActionResult<IEnumerable<Pedido>> GetError()
        {
            throw new Exception();
        }

        [HttpGet("{id}", Name = "GetId")]
        public ActionResult<Pedido> Get([FromRoute]string id)
        {
            try
            {
                return this._pedidos.Find(e => e.Id.Equals(id)).SingleOrDefault();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]Pedido pedido)
        {
            try
            {
                this._pedidos.InsertOne(pedido);
                return CreatedAtRoute("GetId", new { id = pedido.Id }, pedido);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }
    }
}