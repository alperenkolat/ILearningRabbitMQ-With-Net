// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Hello, World!");

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
var connection = factory.CreateConnection();
var channel = connection.CreateModel();



channel.BasicQos(0, 1, false);
var consumer =new EventingBasicConsumer(channel);

var queueName = "direct-queue-Critical";
    channel.BasicConsume(queueName, false,consumer);

consumer.Received += (object? sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());
    Thread.Sleep(1000);
    Console.WriteLine(message);
    File.AppendAllText(queueName+".txt", message+"\n");  //!does not support running multiple times
    channel.BasicAck(e.DeliveryTag,false);
};

Console.ReadLine();
