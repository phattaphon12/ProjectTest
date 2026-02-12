using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace RecipeMicroservice.Services
{
    public class WebSocketHandler
    {
        private readonly StatusEquipServices _statusService;

        public WebSocketHandler(StatusEquipServices statusService)
        {
            _statusService = statusService;
        }

        public async Task HandleAsync(HttpContext context, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];

            try
            {
                while (webSocket.State == WebSocketState.Open)
                {
                    // ตัวอย่าง: ดึงข้อมูลจาก DB ผ่าน Service ที่คุณมีอยู่แล้ว
                    var data = await _statusService.GetAllStatusEquipAsync();
                    var json = JsonSerializer.Serialize(data);
                    var bytes = Encoding.UTF8.GetBytes(json);

                    // ส่งข้อมูลไปที่ React Frontend
                    await webSocket.SendAsync(
                        new ArraySegment<byte>(bytes),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None);

                    // หน่วงเวลาตามความเหมาะสม (เช่น 1-2 วินาที)
                    await Task.Delay(2000);
                }
            }
            catch (Exception ex)
            {
                // จัดการเมื่อการเชื่อมต่อหลุด
                Console.WriteLine($"WebSocket Error: {ex.Message}");
            }
            finally
            {
                if (webSocket.State != WebSocketState.Closed)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                }
            }
        }
    }
}
