using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

[ApiController]
[Route("api/webhook")]
public class WhatsAppWebhookController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private const string Token = "EAAHV0JpFtmIBO5tfWsVfMVYvNZBdKUTAlhsZCHmTOXI0l6Q62zBRIRFytTvfnZA29BKTbDr2b05yCpcELmUYiYkbSwC6OoaJuT0W98naGZA8xy18HvDO5J8tOYfVRUEPaKlPOAvFnpgD1me1JD2XohD2RleAyHLAbYY7EV9xerVaxOeT2cJQeSNcNRNdUzIJue2387oVssmJvOGWN0ZBrLxmgWHcZD";


    public WhatsAppWebhookController()
    {
        _httpClient = new HttpClient();
    }
    [HttpGet]
    public IActionResult VerifyWebhook([FromQuery(Name = "hub.mode")] string hubMode,
                                  [FromQuery(Name = "hub.challenge")] string hubChallenge,
                                  [FromQuery(Name = "hub.verify_token")] string hubVerifyToken)
    {
        //const string VerifyToken = "SEU_TOKEN_AQUI"; // O mesmo que foi configurado no Meta

        if (hubMode == "subscribe" && hubVerifyToken == Token)
        {
            return Ok(hubChallenge); // O Meta exige esse retorno para validar
        }

        return BadRequest("Token inválido ou modo incorreto.");
    }
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] JObject bodyParam)
    {
        if (bodyParam["object"] != null)
        {
            var entry = bodyParam["entry"]?.FirstOrDefault();
            var changes = entry?["changes"]?.FirstOrDefault();
            var value = changes?["value"];
            var messages = value?["messages"]?.FirstOrDefault();

            if (messages != null)
            {
                string phoneNumberId = value?["metadata"]?["phone_number_id"]?.ToString();
                string from = messages?["from"]?.ToString();
                string msgBody = messages?["text"]?["body"]?.ToString();

                Console.WriteLine($"Phone number: {phoneNumberId}");
                Console.WriteLine($"From: {from}");
                Console.WriteLine($"Message: {msgBody}");

                var responseBody = new
                {
                    messaging_product = "whatsapp",
                    to = from,
                    text = new { body = $"Hi.. I'm Prasath, your message is {msgBody}" }
                };

                var jsonContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(responseBody), Encoding.UTF8, "application/json");


                await _httpClient.PostAsync($"https://graph.facebook.com/v13.0/{phoneNumberId}/messages?access_token={Token}", jsonContent);

                return Ok();
            }
        }
        return NotFound();
    }
}