using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace UdemyRabbitMQ.publisher
{
    public enum LogNames
    {
        Critical=1,
        Error=2,
        Warning=3,
        Info=4,
    }
     class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://mhqkinbn:IeeRyd_9tKyqdXlGEuqT-cP3j1RarMe7@woodpecker.rmq.cloudamqp.com/mhqkinbn");
            using var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            //channel.QueueDeclare("hello-queue", true, false, false);
            channel.ExchangeDeclare("logs-direct", durable: true, type: ExchangeType.Direct);

            Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
            {
                var routeKey = $"route-{x}";
                var queueName = $"direct-queue-{x}";
                channel.QueueDeclare(queueName,true,false,false);
                channel.QueueBind(queueName,"logs-direct",routeKey,null);
            });


            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                LogNames log=(LogNames)new Random().Next(1,4);
                string message = $"log-type: {log}";

                var messageBody = Encoding.UTF8.GetBytes(message);

                var routeKey = $"route-{log}";

                channel.BasicPublish("logs-direct", routeKey, null, messageBody);

                Console.WriteLine("Mesajınız Gönderildi : "+message+" "+x);
            });



            Console.ReadLine();
        }
    }
}
