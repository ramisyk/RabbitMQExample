// 1. Create Connection to RabbitMQ

using System.Text;
using RabbitMQ.Client;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new ("amqps://xvjabifq:cfggZ3ucf6wtav4AyKy4fm_5KTiUVcsL@toad.rmq.cloudamqp.com/xvjabifq");

// 2. Activate Connection and Open Channel
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();



// 3. Create Queue
// exclusive -> if it equals true (as default), queue is created only this channel and it will be deleted before reach consumer
// durable: true means provide durability for queue (in case of crashing RabbitMQ server)
channel.QueueDeclare(queue: "example-queue", exclusive: false, durable: true);

// Durability operations for message side
IBasicProperties properties = channel.CreateBasicProperties();
properties.Persistent = true;


// 4. Send Message to Queue
// Message is approved as byte by RabbitMQ 
for (int i = 0; i < 1000; i++)
{
    await Task.Delay(3000);
    byte[] message = Encoding.UTF8.GetBytes($"Hello {i}");
    channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message);
    
}
byte[] message2 = Encoding.UTF8.GetBytes("Hello World");

// if exchange is null it means default exchange so direct exchange
// in direct exchange routingKey equals queue name
channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message2);

Console.Read();