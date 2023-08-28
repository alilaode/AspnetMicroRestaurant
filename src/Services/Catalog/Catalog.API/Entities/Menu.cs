using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Catalog.API.Entities
{
    public class Menu
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }
        public string Category { get; set; }
 
        public decimal Price { get; set; }
    }
}
