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
        //[Fact]
        //public void Publish_Should_Call_BasicPublish()
        //{
        //    // Arrange
        //    var message = "My message";
        //    var routingKey = "routing-key";
        //    var modelMock = new Mock<IConnectionProvider>();
        //    modelMock.Setup(x => x.GetConnection().CreateModel().BasicPublish(
        //        It.IsAny<string>(), routingKey, It.IsAny<IBasicProperties>(),
        //        It.IsAny<byte[]>()));
        //    var pub = new Publisher(modelMock.Object, It.IsAny<string>(), ExchangeType.Direct,null);
        
        //    // Act
        //    pub.Publish(message, routingKey, null, null);

        //    // Assert
        //    modelMock.Verify(
        //        x => x.GetConnection().CreateModel().BasicPublish(
        //            It.IsAny<string>(),
        //            routingKey,
        //            It.IsAny<IBasicProperties>(),
        //        It.IsAny<byte[]>()
        //        ),
        //    Times.Once
        //    );
       // }
    }
}
