using System;
using System.Text;
using System.Threading.Tasks;
using EventBus.Core;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.DependencyInjection;

namespace EventBus.RabbitMQ
{
    public class RabbitMqEventBus : IEventBus
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _serviceProvider;
        private readonly RabbitMqOptions _options;

        public RabbitMqEventBus(IServiceProvider serviceProvider, RabbitMqOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _serviceProvider = serviceProvider;

            var factory = new ConnectionFactory() { HostName = _options.HostName };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(
                exchange: _options.ExchangeName,
                type: _options.ExchangeType,
                durable: _options.DurableExchange);
        }

        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();
            _connection?.Close();
            _connection?.Dispose();
        }

        public Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));

            var props = _channel.CreateBasicProperties();
            props.Persistent = true; 

            if (_options.BasicProperties != null)
            {
                foreach (var header in _options.BasicProperties.Headers ?? new Dictionary<string, object>())
                {
                    props.Headers ??= new Dictionary<string, object>();
                    props.Headers[header.Key] = header.Value;
                }
            }

            _channel.BasicPublish(
                exchange: _options.ExchangeName,
                routingKey: _options.RoutingKey,
                basicProperties: props,
                body: body
            );

            return Task.CompletedTask;
        }

        public void Subscribe<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>, new()
        {
            var eventName = typeof(TEvent).Name;
            var queueName = string.IsNullOrEmpty(_options.QueueNamePrefix)
                ? eventName
                : $"{_options.QueueNamePrefix}.{eventName}";

            _channel.QueueDeclare(
                queue: queueName,
                durable: _options.DurableQueue,
                exclusive: false,
                autoDelete: _options.AutoDeleteQueue,
                arguments: null);

            _channel.QueueBind(
                queue: queueName,
                exchange: _options.ExchangeName,
                routingKey: _options.RoutingKey);

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var @event = JsonConvert.DeserializeObject<TEvent>(message);
                if (@event == null) return;

                using var scope = _serviceProvider.CreateScope();
                var handler = scope.ServiceProvider.GetService<THandler>();
                if (handler != null)
                {
                    await handler.HandleAsync(@event);
                }

                if (!_options.AutoAck)
                {
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
            };

            _channel.BasicConsume(queue: queueName, autoAck: _options.AutoAck, consumer: consumer);
        }




    }
}
