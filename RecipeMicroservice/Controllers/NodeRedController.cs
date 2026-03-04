using Microsoft.AspNetCore.Mvc;
using RecipeMicroservice.Models;
using System.Net.Http; // แนะนำให้ใช้ IHttpClientFactory แทนการสร้าง new HttpClient() ทุกครั้ง

namespace RecipeMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NodeRedController : ControllerBase
    {
        private readonly string _nodeRedBaseUrl;
        private readonly IHttpClientFactory _httpClientFactory;

        // ฉีด IConfiguration และ IHttpClientFactory เข้ามาผ่าน Constructor
        public NodeRedController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            // ดึงค่าจาก appsettings.json
            _nodeRedBaseUrl = configuration.GetValue<string>("NodeRedSettings:BaseUrl");
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("upload-recipe")]
        public async Task<IActionResult> RequestFromNodeRed()
        {
            string url = $"{_nodeRedBaseUrl}/get-recipe-data";
            var client = _httpClientFactory.CreateClient();

            try
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var recipe = await response.Content.ReadFromJsonAsync<RecipeFromPLC>();
                    return Ok(recipe);
                }
                return BadRequest("Cannot connect to Node-RED");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost("download-recipe")]
        public async Task<IActionResult> DownloadRecipe([FromBody] RecipeFromPLC req)
        {
            string url = $"{_nodeRedBaseUrl}/receive-recipe";
            var client = _httpClientFactory.CreateClient();

            try
            {
                var response = await client.PostAsJsonAsync(url, req);

                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { message = "ส่งข้อมูลไปยัง Node-RED สำเร็จ" });
                }
                return StatusCode(500, "ไม่สามารถเชื่อมต่อกับ Node-RED ได้");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}