// See https://aka.ms/new-console-template for more information
using ILearningRabbitMQ.Publisher;
using RabbitMQ.Client;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Channels;




        Console.WriteLine("Hello, World!");

        var factory =new ConnectionFactory();
        factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();
        //channel.QueueDeclare("hello-queue", true, false, false);


        channel.ExchangeDeclare("logs-direct",durable:true,type:ExchangeType.Direct);

        Enum.GetNames(typeof(LogNames)).ToList().ForEach(x => {
            var routeKey = $"route-{x}";
            var queueName = $"direct-queue-{x}";
            channel.QueueDeclare(queueName,true,false,false);
            channel.QueueBind(queueName, "logs-direct", routeKey,null);

            });


        Enumerable.Range(1, 50).ToList().ForEach(x =>
        {
            LogNames log = (LogNames)new Random().Next(1, 5);
            string message = $"Message-{log}";
            var messageBody = Encoding.UTF8.GetBytes(message);
            var routeKey = $"route-{log}";

            channel.BasicPublish("logs-direct", routeKey, null, messageBody);
            Console.WriteLine(message);
        }
        );
        Console.ReadLine();



