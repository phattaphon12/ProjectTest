using Microsoft.AspNetCore.Mvc;
using RecipeMicroservice.Models;
using RecipeMicroservice.Services;

namespace RecipeMicroservice.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class StatusEquipController : ControllerBase
    {
        private readonly StatusEquipServices _statusEquipServices;

        public StatusEquipController(StatusEquipServices statusEquipServices)
        {
            _statusEquipServices = statusEquipServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStatusEquip()
        {
            var statusEquips = await _statusEquipServices.GetAllStatusEquipAsync();
            return Ok(statusEquips);
        }

        [HttpPost("get-status-by-equip")]
        public async Task<IActionResult> GetStatusEquip([FromBody] int EquipID)
        {
            var statusEquip = await _statusEquipServices.GetStatusByEquipAsync(EquipID);
            if (statusEquip == null)
            {
                return NotFound();
            }
            return Ok(statusEquip);
        }

        [HttpPost("input-status")]
        public async Task<IActionResult> UpdateStatusEquip([FromBody] FormUpdateStatusEquip req)
        {
            var result = await _statusEquipServices.UpdateStatusEquipAsync(req);
            return Ok(result);
        }
    }
}
