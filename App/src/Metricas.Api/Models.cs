using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Metricas.Api
{
    public class Pedido
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int IdUsuario { get; set; }
        public ICollection<ItemPedido> ItensPedido { get; set; }
    }
    public class ItemPedido
    {
        public string IdProduto { get; set; }
        public int Quantidade { get; set; }
    }
}
