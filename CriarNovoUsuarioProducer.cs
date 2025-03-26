using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

public class CriarNovoUsuarioProducer
{
    private readonly string _hostName = "jaragua.lmq.cloudamqp.com";
    private readonly string _queueName = "anc-create-users-producer";

    public async Task<string> SendMessageRabbitMq(string mensagem)
    {
        try
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqps://bxubynos:zyM7sLHJYSPdPgvjo9mQ5OGGiYxAuxJZ@jaragua.lmq.cloudamqp.com/bxubynos")
            };

            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "anc-create-users-producer",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            string message = mensagem;

            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: _queueName, // Nome da fila (routing key)
                body: body
            );
            //return await Task.Run(async () =>
            //{
            //    {
            //        await channel.QueueDeclareAsync(queue: _queueName,
            //                                        durable: false,
            //                                        exclusive: false,
            //                                        autoDelete: false,
            //                                        arguments: null);
            //
            //        var body = Encoding.UTF8.GetBytes(mensagem);
            //
            //        await channel.BasicPublishAsync(exchange: string.Empty,
            //                                        routingKey: _queueName,
            //                                        body: body);
            //    }
            //
            //    
            //});
            return "Usuário Criado";
        }
        catch (Exception ex)
        {
            return $"Erro ao enviar mensagem: {ex.Message}";
        }
    }
}
