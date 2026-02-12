using Microsoft.AspNetCore.Mvc;
using RecipeMicroservice.Models;

namespace RecipeMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NodeRedController : ControllerBase
    {
        [HttpGet("upload-recipe")]
        public async Task<IActionResult> RequestFromNodeRed()
        {
            string url = "http://127.0.0.1:1880/get-recipe-data";
            using var client = new HttpClient();

            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var recipe = await response.Content.ReadFromJsonAsync<RecipeFromPLC>();
                return Ok(recipe); // จะได้ข้อมูล JSON ตามที่คุณต้องการ
            }
            return BadRequest("Cannot connect to Node-RED");
        }

        [HttpPost("download-recipe")]
        public async Task<IActionResult> DownloadRecipe([FromBody] RecipeFromPLC req)
        {
            // 2. ส่งข้อมูลไปยัง Node-RED
            using (var client = new HttpClient())
            {
                // URL ของ Node-RED (เช่น http://raspberrypi.local:1880/receive-recipe)
                string nodeRedUrl = "http://127.0.0.1:1880/receive-recipe"; 

                var response = await client.PostAsJsonAsync(nodeRedUrl, req);

                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { message = "ส่งข้อมูลไปยัง Node-RED สำเร็จ" });
                }
                else
                {
                    return StatusCode(500, "ไม่สามารถเชื่อมต่อกับ Node-RED ได้");
                }
            }
        }
    }
}
