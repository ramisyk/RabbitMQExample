// 1. Create Connection to RabbitMQ

using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new("amqps://xvjabifq:cfggZ3ucf6wtav4AyKy4fm_5KTiUVcsL@toad.rmq.cloudamqp.com/xvjabifq");

IConnection conn = factory.CreateConnection();

// 2. Activate Connection and Open Channel
using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();



// 3. Create Queue
// exclusive -> if it equals true (as default), queue is created only this channel and it will be deleted before reach consumer
// queue should be same as publisher
channel.QueueDeclare(queue: "example-queue", exclusive: false);

// 4. Read Message From Queue
// Message is approved as byte by RabbitMQ 

EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
channel.BasicConsume(queue: "example-queue", autoAck: false, consumer);
consumer.Received += (sender, e) =>
{
    // Process the messages
    // e.Body : get data of message
    // e.Body.Span || e.Body.ToArray() return data as byte
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
    
    // configure Message Acknowledgement properties
    // with this property the messages in queue will be deleted after processed successfully 
    channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
};

Console.Read();