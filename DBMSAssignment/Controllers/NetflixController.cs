
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBMSAssignment.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace DBMSAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NetflixController : ControllerBase
    {
        string connStr = "mongodb://127.0.0.1:27017";
        string connectionString= "mongodb+srv://sxy59550:mFytjr91igtzLc9K@cluster0.kyfhftb.mongodb.net/?retryWrites=true&w=majority";
        string databasename = "database";
        string collectionname = "netflix";

        [HttpGet]
        public async Task<IEnumerable<Netflix>> Get()
        {
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase(databasename);
            var collection = db.GetCollection<Netflix>(collectionname);
            var result = await collection.FindAsync(x => x.ImdbScore > 8);
            return result.ToList();
        }

        [HttpPost]
        public async Task<IActionResult> AddMovie(Netflix netflix)
        {
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase(databasename);
            var collection = db.GetCollection<Netflix>(collectionname);
            await collection.InsertOneAsync(netflix);
            return Ok();
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateMovie(Netflix netflix)
        {
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase(databasename);
            var collection = db.GetCollection<Netflix>(collectionname);
            var filter = Builders<Netflix>.Filter.Eq("Id", netflix.Id);
            await collection.ReplaceOneAsync(filter, netflix);
            return Ok();
        }

        [HttpDelete]
        [Route("/{fname}")]
        public async Task<IActionResult> DeleteMovie(string fname)
        {
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase(databasename);
            var collection = db.GetCollection<Netflix>(collectionname);
            await collection.DeleteOneAsync(x => x.Title.ToUpper().Equals(fname.ToUpper()));
            return Ok();
        }

        [HttpGet]
        [Route("/{fname}")]
        public async Task<IEnumerable<Netflix>> GetMovie(string fname)
        {
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase(databasename);
            var collection = db.GetCollection<Netflix>(collectionname);
            var result = await collection.FindAsync(x => x.Title.ToUpper().Contains(fname.ToUpper()));
            return result.ToList();
        }
    }
}
