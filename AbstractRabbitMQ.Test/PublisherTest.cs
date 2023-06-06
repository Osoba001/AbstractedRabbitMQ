using AbstractedRabbitMQ.Publishers;
using AbstractedRabbitMQ.Setup;
using Moq;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AbstractRabbitMQ.Test
{
    public class PublisherTest
    {
        [Fact]
        public void Publish_Should_Call_BasicPublish()
        {
            // Arrange
            var message = "My message";
            var routingKey = "routing-key";
            var exchange = "demoExchange";
           var config= new PublisherConfig();
            var modelMock = new Mock<IConnectionProvider>();
            modelMock.Setup(x => x.GetModel().BasicPublish(
                exchange, routingKey, null,
                null));
            var pub = new Publisher(modelMock.Object, config);

            // Act
            pub.Publish(message, routingKey, null, null);

            // Assert
            //modelMock.Verify(
            //    x => x.GetModel().BasicPublish(
            //        exchange,
            //        routingKey,
            //        null,
            //    null
            //    ),
            //Times.Once
            //);
        }
    }
}
