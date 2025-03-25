using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

[ApiController]
[Route("api/webhook")]
public class WhatsAppWebhookController : ControllerBase
{
    [HttpGet]
    public IActionResult Verify([FromQuery] string? hub_mode, [FromQuery] string? hub_challenge, [FromQuery] string? hub_verify_token)
    {
        // Substitua "SEU_TOKEN" pelo token configurado no Meta Developer
        if (hub_mode == "subscribe" 
            && hub_verify_token == "EAAHV0JpFtmIBOxRKwmzA75hzNw9JuPAcsZAZBMHxiAI3b3KT9fc2GeuGiBYowqEx9k8yIMsd0IFk1LmNlbdZBCO93ZAcHosLKQedaIRAZCK5JSmmG47ty2vCiaqJikLRqwwXexrkTzMZBp1SGtOyhOagAvzfIfTtZCB8vykBCozYFfvaz5kvKlZCm0UOTyMuhEOMBaW3UGyihZAijecskngQwQyT0ZAuMZD" 
            && !string.IsNullOrEmpty(hub_challenge))
        {
            return Content(hub_challenge, "text/plain"); // Retorna apenas o desafio em texto puro
        }
        return BadRequest("Par�metros inv�lidos ou ausentes.");
    }

    [HttpPost]
    public async Task<IActionResult> ReceiveMessage()
    {
        using var reader = new StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync();
        var data = JsonConvert.DeserializeObject<dynamic>(body);

        Console.WriteLine("Recebido: " + body);

        return Ok();
    }
}
