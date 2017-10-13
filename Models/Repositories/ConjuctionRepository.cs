using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ConjuctorAPI.Models.Interfaces;
using ConjuctorAPI.Utils;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace ConjuctorAPI.Models.Repositories
{
    public class ConjuctionRepository : IConjuctionRepository
    {
        private static readonly HttpClient _client = new HttpClient();
        private static string _verbURL = "http://api.verbix.com/finder/json/";
        private static string _conjuctionURL = "http://api.verbix.com/conjugator/json/";
        private static string _verbixAPI = "3138c62f-a9f7-11e7-ab6a-00089be4dcbc/v2/";
        private readonly ConjuctionContext _context = null;

        public ConjuctionRepository(IOptions<Settings> settings)
        {
            _context = new ConjuctionContext(settings);
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<IEnumerable<Conjuction>> GetAllConjuctions()
        {
            return await _context.Conjuctions.Find(_ => true).ToListAsync();
        }

        public async Task<List<string>> GetConjuction(string verb, string method)
        {
            var filter = Builders<Conjuction>.Filter.Eq("Verb", verb);            
            var responseVerb = _client.GetAsync(_verbURL + _verbixAPI + "eng/" + verb).Result;
            using (HttpContent contentVerb = responseVerb.Content)
            {
                Task<string> resultVerb =  contentVerb.ReadAsStringAsync();
                var findedVerb = JsonConvert.DeserializeObject<List<FindedVerb>>(resultVerb.Result);
                
                // WORD IS NOT VERB
                if (findedVerb.Count == 0)
                    return null;
                // WORD IS VERB
                else 
                {
                    // SEARCH IN MONGO DB
                    var conjuction = await _context.Conjuctions
                                            .Find(filter)
                                            .FirstOrDefaultAsync();
                    
                    // WORD DOESNT EXIST IN MONGO DB 
                    if (conjuction == null) 
                    {
                        var responseConjuction = _client.GetAsync(_conjuctionURL + _verbixAPI + "eng/" + verb).Result;
                        using (HttpContent contentConjuction = responseConjuction.Content)
                        {
                            Task<string> resultConjuction =  contentConjuction.ReadAsStringAsync();
                            conjuction = JsonConvert.DeserializeObject<Conjuction>(resultConjuction.Result);
                            await this.AddConjuction(conjuction);
                            return ConjuctionUtils.GetFormsFromConjuction(conjuction, method);
                        }
                    }
                    // WORD EXISTS IN MONGO DB
                    else 
                        return ConjuctionUtils.GetFormsFromConjuction(conjuction, method);
                }
            }
        }

        public async Task AddConjuction(Conjuction item)
        {
            await _context.Conjuctions.InsertOneAsync(item);
        }

        public async Task<DeleteResult> RemoveConjuction(string id)
        {
            return await _context.Conjuctions.DeleteOneAsync(
                        Builders<Conjuction>.Filter.Eq("Id", id));
        }

        // public async Task<UpdateResult> UpdateConjuction(string id, string body)
        // {
        //     var filter = Builders<Conjuction>.Filter.Eq(s => s.Id, id);
        //     var update = Builders<Conjuction>.Update
        //                         .Set(s => s.Body, body)
        //                         .CurrentDate(s => s.UpdatedOn);
        //     return await _context.Conjuctions.UpdateOneAsync(filter, update);
        // }

        // public async Task<ReplaceOneResult> UpdateConjuction(string id, Note item)
        // {
        //     return await _context.Notes
        //                         .ReplaceOneAsync(n => n.Id.Equals(id)
        //                                             , item
        //                                             , new UpdateOptions { IsUpsert = true });
        // }

        public async Task<DeleteResult> RemoveAllConjuction()
        {
            return await _context.Conjuctions.DeleteManyAsync(new BsonDocument());
        }   
    }
}