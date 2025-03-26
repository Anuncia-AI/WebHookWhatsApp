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



    [HttpPost]
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