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
        // Log para depuração
        Console.WriteLine(JsonSerializer.Serialize(body, new JsonSerializerOptions { WriteIndented = true }));

        var mensagem = body.GetProperty("value")
                            .GetProperty("messages")[0]
                            .GetProperty("text")
                            .GetProperty("body")
                            .GetString();

        var mensagemObjeto = new Mensagem
        {
            mensagem = mensagem
        };

        return Ok(mensagemObjeto);
    }
}

public class Mensagem 
{
    public string mensagem { get; set; }
}