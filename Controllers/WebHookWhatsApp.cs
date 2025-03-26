using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

[Route("api/webhook")]
[ApiController]
public class WebhookController : ControllerBase
{
    private const string VerifyToken = "EAAHV0JpFtmIBO5tfWsVfMVYvNZBdKUTAlhsZCHmTOXI0l6Q62zBRIRFytTvfnZA29BKTbDr2b05yCpcELmUYiYkbSwC6OoaJuT0W98naGZA8xy18HvDO5J8tOYfVRUEPaKlPOAvFnpgD1me1JD2XohD2RleAyHLAbYY7EV9xerVaxOeT2cJQeSNcNRNdUzIJue2387oVssmJvOGWN0ZBrLxmgWHcZD";
    //[HttpGet]
    //public IActionResult VerifyWebhook([FromQuery(Name = "hub.mode")] string hubMode,
    //                                   [FromQuery(Name = "hub.challenge")] string hubChallenge,
    //                                   [FromQuery(Name = "hub.verify_token")] string hubVerifyToken)
    //{
    //    hubMode = "subscribe";
    //
    //    if (hubMode == "subscribe" && hubVerifyToken == VerifyToken)
    //    {
    //        return Ok(hubChallenge);
    //    }
    //
    //    return BadRequest("Token inválido ou modo incorreto.");
    //}

    [HttpPost]
    public IActionResult ReceiveMessage([FromBody] JsonElement body)
    {
        Console.WriteLine(JsonSerializer.Serialize(body, new JsonSerializerOptions { WriteIndented = true }));

        if (body.TryGetProperty("entry", out var entryArray) && entryArray.ValueKind == JsonValueKind.Array)
        {
            foreach (var entry in entryArray.EnumerateArray())
            {
                if (entry.TryGetProperty("changes", out var changesArray) && changesArray.ValueKind == JsonValueKind.Array)
                {
                    foreach (var change in changesArray.EnumerateArray())
                    {
                        if (change.TryGetProperty("value", out var value))
                        {
                            string phoneNumberId = value.GetProperty("metadata").GetProperty("phone_number_id").GetString();
                            if (value.TryGetProperty("messages", out var messagesArray) && messagesArray.ValueKind == JsonValueKind.Array)
                            {
                                foreach (var message in messagesArray.EnumerateArray())
                                {
                                    string from = message.GetProperty("from").GetString();
                                    string messageBody = message.GetProperty("text").GetProperty("body").GetString();

                                    Console.WriteLine($"📩 Nova mensagem de {from}: {messageBody}");
                                }
                            }
                        }
                    }
                }
            }
        }

        return Ok();
    }
}
