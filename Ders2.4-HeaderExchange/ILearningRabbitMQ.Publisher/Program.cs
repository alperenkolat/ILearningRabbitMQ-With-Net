// See https://aka.ms/new-console-template for more information
using ILearningRabbitMQ.Publisher;
using RabbitMQ.Client;
using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Channels;




Console.WriteLine("Hello, World!");

var factory =new ConnectionFactory();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
var connection = factory.CreateConnection();
var channel = connection.CreateModel();


channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);

Dictionary<string, object> headers = new Dictionary<string, object>();
headers.Add("format", "pdf");
headers.Add("shape", "a4");
var properties = channel.CreateBasicProperties();
properties.Headers=headers;

channel.BasicPublish("header-exchange",string.Empty,properties,Encoding.UTF8.GetBytes("header message"));
Console.WriteLine("message posted:");
Console.ReadLine();



