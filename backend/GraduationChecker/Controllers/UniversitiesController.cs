using GraduationChecker.Models;
using GraduationChecker.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GraduationChecker.Controllers
{
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UniversitiesController : Controller
    {
        private readonly DataService _dataService;

        public UniversitiesController(DataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUniversities()
        {
            return Ok(await _dataService.GetUniversities());
        }

        [HttpPost]
        public async Task<IActionResult> AddUniversity([FromBody] University university)
        {
            await _dataService.CreateUniversity(university);
            return Ok();
        }
    }
}