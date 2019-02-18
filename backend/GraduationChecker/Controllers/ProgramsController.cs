using GraduationChecker.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GraduationChecker.Controllers
{
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ProgramsController : Controller
    {
        private readonly DataService _dataService;

        public ProgramsController(DataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPrograms([FromQuery]string universityName, [FromQuery]string degree, [FromQuery]string year)
        {
            return Ok(await _dataService.GetPrograms(universityName, degree, year));
        }

        [HttpGet("specs")]
        public async Task<IActionResult> GetProgramSpecs()
        {
            return Ok(await _dataService.GetProgramSpecs());
        }

        [HttpPost]
        public async Task<IActionResult> AddProgram([FromBody] Models.Program program)
        {
            await _dataService.CreateProgram(program);
            return Ok();
        }
    }
}
