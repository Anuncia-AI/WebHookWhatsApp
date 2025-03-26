using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/webhook")]
public class WhatsAppWebhookController : ControllerBase
{
    [HttpGet]
    public string dadosMocado()
    {
        Console.WriteLine("Nada");

        return "a";
    }
}