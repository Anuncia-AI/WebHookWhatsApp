using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebHookWhatsApp;

[Route("api/webhook")]
[ApiController]
public class WebhookController : ControllerBase
{
    private readonly CriarNovoUsuarioProducer _criarUsuarioProducer;

    public WebhookController(CriarNovoUsuarioProducer criarNovoUsuarioProducer)
    {
        _criarUsuarioProducer = criarNovoUsuarioProducer;
    }

    [HttpGet]
    public IActionResult VerifyWebhook([FromQuery] string hub_mode, [FromQuery] string hub_challenge, [FromQuery] string hub_verify_token)
    {
        var verifyToken = "EAAHV0JpFtmIBO7dSkZA8vZAia41U98VMA5G4vw5LVUUsmleVpdcV4o1b6ZC60dFnl2nFAZBvKGpk4yXYMXYZC24Knes4SQxoDcX1NkelYLTP8hSCP1qA39ahoCOVIXm5QE7yQIwXZBZCWtZAOOj1IAMpOGK82nLuShVYFSssUEYQMYLzj8YvWT0bfRyFunxgzhOypZBMUThZADJAdk9YRgeEcBIVsgem8ZD"; // O mesmo token que você configurou no portal do Meta

        // Verifica se os parâmetros estão corretos e se o token bate
        if (hub_mode == "subscribe" && hub_verify_token == verifyToken)
        {
            return Ok(hub_challenge); // Retorna o desafio para confirmar a inscrição
        }

        return BadRequest("Token de verificação inválido");
    }
    [HttpPost (Name = "criarusuario")]
    public async Task<string> ReceiveMessage([FromBody] JsonElement body)
    {
        // Log para depuração
        Console.WriteLine(JsonSerializer.Serialize(body, new JsonSerializerOptions { WriteIndented = true }));

        var mensagem = body.GetProperty("value")
                            .GetProperty("messages")[0]
                            .GetProperty("text")
                            .GetProperty("body")
                            .GetString();


        if (mensagem != null)
        {
           await _criarUsuarioProducer.SendMessageRabbitMq(mensagem);
        }

        var mensagemObjeto = new Mensagem
        {
            mensagem = mensagem
        };



        return mensagem;
    }
}

public class Mensagem
{
    public string mensagem { get; set; }
}