using System;
using AuditLog.Abstractions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AuditLog
{
    public class EventBus : IEventBus
    {
        private bool _disposed;

        public EventBus(IConnection connection, string exchangeName)
        {
            Connection = connection;
            ExchangeName = exchangeName;
            Model = connection.CreateModel();
        }
        public IConnection Connection { get; }
        public string ExchangeName { get; }
        public IModel Model { get; }
        public IEventBus AddEventListener(IEventListener eventListener, string topic)
        {
            var queueName = Model.QueueDeclare().QueueName;
            Model.QueueBind(exchange: ExchangeName,
                queue: queueName,
                routingKey: topic);
            
            var consumer = new EventingBasicConsumer(Model);
            consumer.Received += eventListener.Handle;
            Model.BasicConsume(consumer, queueName);

            return this;
        }

        public void Dispose()
        { 
            Dispose(true);
            GC.SuppressFinalize(this);           
        }
   
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return; 
      
            if (disposing) {
                Model.Dispose();
                Connection.Dispose();
            }
      
            _disposed = true;
        }
    }
}