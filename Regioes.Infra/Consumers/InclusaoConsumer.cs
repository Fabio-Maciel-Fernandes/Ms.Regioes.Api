using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Regioes.Core.Models;
using Regioes.Infra.Services.Interfaces;
using System.Text;
using System.Text.Json;


namespace Regioes.Infra.Consumers
{
    public class InclusaoConsumer : BackgroundService
    {

        private readonly ILogger<InclusaoConsumer> logger;
        private readonly IServices<Regiao> services;
        private readonly int intervaloMensagemWorkerAtivo = 60000;
        private readonly string url = "amqps://nzosfmoq:uZNH48guR3ZZzmTWib0KoeZUYFICSQI7@fly.rmq.cloudamqp.com/nzosfmoq";
        private readonly string queue = "REGIAO_INCLUSAO";
        public InclusaoConsumer(ILogger<InclusaoConsumer> logger, IServices<Regiao> services)
        {
            this.logger = logger;
            this.services = services;
            logger.LogInformation($"Queue = {queue}");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Aguardando mensagens...");

            var factory = new ConnectionFactory()
            {
                Uri = new Uri(url),
                Ssl = new SslOption { Enabled = true, ServerName = "fly.rmq.cloudamqp.com" }
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: queue,
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;
            channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation($"Worker ativo em: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                await Task.Delay(intervaloMensagemWorkerAtivo, stoppingToken);
            }
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            logger.LogInformation($"[Nova mensagem | {DateTime.Now:yyyy-MM-dd HH:mm:ss}] " + Encoding.UTF8.GetString(e.Body.ToArray()));

            var mensagem = Encoding.UTF8.GetString(e.Body.ToArray());
            if (!string.IsNullOrEmpty(mensagem))
            {
                Regiao regiao = JsonSerializer.Deserialize<Regiao>(mensagem);
                services.Create(regiao);
            }
            
        }
    }
}
