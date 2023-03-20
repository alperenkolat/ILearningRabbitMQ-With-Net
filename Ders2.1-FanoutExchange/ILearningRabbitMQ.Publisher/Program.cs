﻿// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;

Console.WriteLine("Hello, World!");

var factory =new ConnectionFactory();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
var connection = factory.CreateConnection();
var channel = connection.CreateModel();
//channel.QueueDeclare("hello-queue", true, false, false);


channel.ExchangeDeclare("logs-fanout",durable:true,type:ExchangeType.Fanout);


Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    string message = $"Message{x}";
    var messageBody = Encoding.UTF8.GetBytes(message);
    channel.BasicPublish("logs-fanout", "", null, messageBody);
    Console.WriteLine(message);
}
);
Console.ReadLine();



    