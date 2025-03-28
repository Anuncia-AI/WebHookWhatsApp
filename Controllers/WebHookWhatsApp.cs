﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;
using WebHookWhatsApp;

[Route("api/webhook")]
[ApiController]
public class WebhookController : ControllerBase
{
    private readonly CriarNovoUsuarioProducer _criarUsuarioProducer;
    private readonly IConfiguration _configuration;
    private readonly ILogger<WebhookController> _logger;

    public WebhookController(
        CriarNovoUsuarioProducer criarNovoUsuarioProducer,
        IConfiguration configuration,
        ILogger<WebhookController> logger)
    {
        _criarUsuarioProducer = criarNovoUsuarioProducer;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult VerifyWebhook([FromQuery] string hub_mode, [FromQuery] string hub_challenge, [FromQuery] string hub_verify_token)
    {
        var verifyToken = _configuration["Logging:MetaApi:Token"];


        if (hub_mode == "subscribe" && hub_verify_token == verifyToken)
        {
            return Ok(hub_challenge);
        }

        return BadRequest("Token de verificação inválido");
    }
    [HttpPost (Name = "criarusuario")]
    public async Task<string> ReceiveMessage([FromBody] JsonElement body)
    {
        _logger.LogInformation("Recebendo mensagem do webhook.");
        var jsonBody = JsonSerializer.Serialize(body, new JsonSerializerOptions { WriteIndented = true });
        _logger.LogDebug("Payload recebido: {JsonBody}", jsonBody);

        var mensagem = body.GetProperty("value")
                            .GetProperty("messages")[0]
                            .GetProperty("text")
                            .GetProperty("body")
                            .GetString();


        if (mensagem == null)
        {
            _logger.LogInformation("Mensagem recebida: {Mensagem}", mensagem);
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