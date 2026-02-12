using Microsoft.AspNetCore.Mvc;
using RecipeMicroservice.Models;
using RecipeMicroservice.Services;

namespace RecipeMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipController : ControllerBase
    {
        private readonly EquipService _equipService;

        public EquipController(EquipService equipService)
        {
            _equipService = equipService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEquip()
        {
            var equips = await _equipService.GetAllEquipAsync();
            return Ok(equips);
        }

        [HttpGet("log")]
        public async Task<IActionResult> GetAllLog()
        {
            var logEquips = await _equipService.GetAllLogAsync();
            return Ok(logEquips);
        }

        [HttpPost("get-log-by-equip")]
        public async Task<IActionResult> GetLogByEquip([FromBody] int equipID)
        {
            var logEquips = await _equipService.GetLogByEquipAsync(equipID);
            return Ok(logEquips);
        }

        [HttpPost("add-log")]
        public async Task<IActionResult> InsertLog([FromBody] FormLogEquip req)
        {
            var result = await _equipService.InsertLogAsync(req);
            return Ok(result);
        }
    }
}
