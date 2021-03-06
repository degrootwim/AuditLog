using System;
using System.Text;
using AuditLog.Abstractions;
using AuditLog.Domain;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing;

namespace AuditLog.Test
{
    [TestClass]
    public class AuditLogEventListenerTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            AuditLogLoggerFactory.LoggerFactory = NullLoggerFactory.Instance;
        }

        [TestMethod]
        public void HandleShouldCallCreateOnRepository()
        {
            // Arrange
             var sender = new object();
             var basicDeliverEventArgs = new BasicDeliverEventArgs
             {
                 BasicProperties = new BasicProperties
                 {
                     Type = "SomeEvent",
                     Timestamp = new AmqpTimestamp(new DateTime(2019, 6, 4).Ticks),
                 },
                 Body = Encoding.Unicode.GetBytes("{'json': 'someJson'}"),
                 RoutingKey = "SomeKey"
             };
            
            var repositoryMock = new Mock<IAuditLogRepository<LogEntry, long>>();
            var repository = repositoryMock.Object;
            var eventListener = new AuditLogEventListener(repository);
            
            // Act
            eventListener.Handle(sender, basicDeliverEventArgs);
            
            // Assert
            repositoryMock.Verify(mock => mock.Create(It.IsAny<LogEntry>()));
        }

        [TestMethod]
        public void HandleShouldCreateLogEntryWithEventType()
        {
            // Arrange
            var testLogEntry = new LogEntry();
            var sender = new object();
            var basicDeliverEventArgs = new BasicDeliverEventArgs
            {
                BasicProperties = new BasicProperties
                {
                    Type = "SomeEvent",
                    Timestamp = new AmqpTimestamp(new DateTime(2019, 6, 4).Ticks),
                },
                Body = Encoding.Unicode.GetBytes("{'json': 'someJson'}"),
                RoutingKey = "SomeKey"
            };
            
            var repositoryMock = new Mock<IAuditLogRepository<LogEntry, long>>();
            var repository = repositoryMock.Object;
            var eventListener = new AuditLogEventListener(repository);
            repositoryMock.Setup(mock => mock.Create(It.IsAny<LogEntry>()))
                .Callback((LogEntry logEntry) => testLogEntry = logEntry);
            
            // Act
            eventListener.Handle(sender, basicDeliverEventArgs);
            
            // Assert
            Assert.AreEqual("SomeEvent", testLogEntry.EventType);
        }
        
        [TestMethod]
        public void HandleShouldCreateLogEntryWithTimeStamp()
        {
            // Arrange
            var testLogEntry = new LogEntry();
            var sender = new object();
            var basicDeliverEventArgs = new BasicDeliverEventArgs
            {
                BasicProperties = new BasicProperties
                {
                    Type = "SomeEvent",
                    Timestamp = new AmqpTimestamp(new DateTime(2019, 6, 4).Ticks),
                },
                Body = Encoding.Unicode.GetBytes("{'json': 'someJson'}"),
                RoutingKey = "SomeKey"
            };
            
            var repositoryMock = new Mock<IAuditLogRepository<LogEntry, long>>();
            var repository = repositoryMock.Object;
            var eventListener = new AuditLogEventListener(repository);
            repositoryMock.Setup(mock => mock.Create(It.IsAny<LogEntry>()))
                .Callback((LogEntry logEntry) => testLogEntry = logEntry);
            
            // Act
            eventListener.Handle(sender, basicDeliverEventArgs);
            
            // Assert
            Assert.AreEqual(new DateTime(2019, 6, 4).Ticks, testLogEntry.Timestamp);
        }
        
        [TestMethod]
        public void HandleShouldCreateLogEntryWithRoutingKey()
        {
            // Arrange
            var testLogEntry = new LogEntry();
            var sender = new object();
            var basicDeliverEventArgs = new BasicDeliverEventArgs
            {
                BasicProperties = new BasicProperties
                {
                    Type = "SomeEvent",
                    Timestamp = new AmqpTimestamp(new DateTime(2019, 6, 4).Ticks),
                },
                Body = Encoding.Unicode.GetBytes("{'json': 'someJson'}"),
                RoutingKey = "SomeKey"
            };
            
            var repositoryMock = new Mock<IAuditLogRepository<LogEntry, long>>();
            var repository = repositoryMock.Object;
            var eventListener = new AuditLogEventListener(repository);
            repositoryMock.Setup(mock => mock.Create(It.IsAny<LogEntry>()))
                .Callback((LogEntry logEntry) => testLogEntry = logEntry);
            
            // Act
            eventListener.Handle(sender, basicDeliverEventArgs);
            
            // Assert
            Assert.AreEqual("SomeKey", testLogEntry.RoutingKey);
        }
        
        [TestMethod]
        public void HandleShouldCreateLogEntryEventJson()
        {
            // Arrange
            var testLogEntry = new LogEntry();
            var sender = new object();
            var basicDeliverEventArgs = new BasicDeliverEventArgs
            {
                BasicProperties = new BasicProperties
                {
                    Type = "SomeEvent",
                    Timestamp = new AmqpTimestamp(new DateTime(2019, 6, 4).Ticks),
                },
                Body = Encoding.Unicode.GetBytes("{'json': 'someJson'}"),
                RoutingKey = "SomeKey"
            };
            
            var repositoryMock = new Mock<IAuditLogRepository<LogEntry, long>>();
            var repository = repositoryMock.Object;
            var eventListener = new AuditLogEventListener(repository);
            repositoryMock.Setup(mock => mock.Create(It.IsAny<LogEntry>()))
                .Callback((LogEntry logEntry) => testLogEntry = logEntry);
            
            // Act
            eventListener.Handle(sender, basicDeliverEventArgs);
            
            // Assert
            Assert.AreEqual("{'json': 'someJson'}", testLogEntry.EventJson);
        }
    }
}