using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConjuctorAPI.Models;
using ConjuctorAPI.Models.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ConjuctorAPI.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("ConjuctionPolicy")]
    public class ConjuctionController : Controller
    {
        private readonly IConjuctionRepository _conjuctionRepository;
        public ConjuctionController(IConjuctionRepository conjuctionRepository) {
            _conjuctionRepository = conjuctionRepository;
        }

        [HttpGet]
        public async Task<string> GetAll()
        {
            var conjuctions = await _conjuctionRepository.GetAllConjuctions();
            return JsonConvert.SerializeObject(conjuctions);
        }
        
        [HttpGet("[action]/{id}")]
        public async Task<string> GetWord(string id)
        {
            var conjuction = await _conjuctionRepository.GetConjuction(id, "word");
            return JsonConvert.SerializeObject(conjuction);
        }

        [HttpGet("[action]/{id}")]
        public async Task<string> GetAll(string id)
        {
            var conjuction = await _conjuctionRepository.GetConjuction(id, "all");
            return JsonConvert.SerializeObject(conjuction);
        }

        [HttpPost]
        public async Task<string> Add([FromBody] Conjuction item)
        {
            await _conjuctionRepository.AddConjuction(item);
            return "";
            // var conjuctions = await _conjuctionRepository.GetAllConjuctions();
            // return JsonConvert.SerializeObject(conjuctions);
        }
    }
}
