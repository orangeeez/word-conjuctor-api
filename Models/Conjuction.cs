using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ConjuctorAPI.Models
{
    public class Conjuction
    {
        [BsonId]
        public ObjectId Id {get; set; }
        public long Langid { get; set; }
        public bool Exists { get; set; }
        public Dictionary<string, Tense> Tenses { get; set; }
        public string Verb { get; set; }
    }
}