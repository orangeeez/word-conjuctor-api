using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace ConjuctorAPI.Models.Interfaces
{
    public interface IConjuctionRepository
    {
        Task<IEnumerable<Conjuction>> GetAllConjuctions();
        Task<List<string>> GetConjuction(string verb);
        Task AddConjuction(Conjuction item);
        Task<DeleteResult> RemoveConjuction(string id);
        // Task<UpdateResult> UpdateConjuction(string id, string body);
    }
}