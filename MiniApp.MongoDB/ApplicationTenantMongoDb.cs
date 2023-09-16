using MiniApp.Core;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MiniApp.MongoDB
{
    [BsonIgnoreExtraElements]
    public class ApplicationTenantMongoDb : ApplicationTenant
    {
        [BsonId]
        public new ObjectId? ItemId { get; set; }
    }
}
