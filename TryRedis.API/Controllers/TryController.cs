using CacheSystem;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TryRedis.API.Services;

namespace TryRedis.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TryController : ControllerBase
    {
        private readonly ICacheService _cacheService;

        public TryController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }
        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            var models = await _cacheService.GetAsync<List<UserModel>>("models") ?? new List<UserModel>();
            var model=new UserModel(Faker.Name.First(), Faker.Name.Last()); 
            
            models.Add(model);
            await _cacheService.SetAsync("models", models);
            return Ok(model);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var models = await _cacheService.GetAsync<List<UserModel>>("models");
            if (models != null)
            {
                try
                {
                    var guidId = new Guid(id);
                    var model = models.FirstOrDefault(p => p.Id == guidId);
                    if (model != null)
                    {
                        return Ok(model);
                    }
                }
                catch (System.Exception)
                {
                }
            }
            return BadRequest("Üye bulunamadı");
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {

            var cache = await _cacheService.GetAsync<List<UserModel>>("models");

            return Ok(cache);
        }
    }
}
