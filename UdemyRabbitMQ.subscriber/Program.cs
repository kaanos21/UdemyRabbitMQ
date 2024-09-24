using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace UdemyRabbitMQ.subscriber
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://mhqkinbn:IeeRyd_9tKyqdXlGEuqT-cP3j1RarMe7@woodpecker.rmq.cloudamqp.com/mhqkinbn");
            using var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            // channel.QueueDeclare("hello-queue", true, false, false);

           




            channel.BasicQos(0, 1, false);
           var consumer=new EventingBasicConsumer(channel);

            var queueName = "direct-queue-Critical";
            channel.BasicConsume(queueName, false,consumer);

            Console.WriteLine("logari dinleniyor");


            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                Thread.Sleep(1500);
                Console.WriteLine("Gelen mesaj : " + message);

                channel.BasicAck(e.DeliveryTag,false);
            };                

            Console.ReadLine();
        }
    }
}
