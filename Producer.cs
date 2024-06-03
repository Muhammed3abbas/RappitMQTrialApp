using System;
using RabbitMQ.Client;
using System.Text;
using Microsoft.SqlServer.Server;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Buffers.Text;
using System.Reflection.PortableExecutable;

//class Producer
//{
//    public void SendMessage(string message)
//    {
//        // 1. Create a connection to RabbitMQ server

//        var factory = new ConnectionFactory() { HostName = "localhost" };
//        using (var connection = factory.CreateConnection())
//        using (var channel = connection.CreateModel())
//        {
//            #region clarification comments
//            /* 
//            Queue: A queue is a storage buffer that holds messages
//            (durable: true), it means that the queue will be saved to disk, so if the RabbitMQ broker(server) restarts, the queue will still exist with its current state. false for simplicity, but in production make it true
//            Exclusive: If true, the queue is used by only one connection and will be deleted when that connection closes. It’s false here to allow multiple consumers.
//            AutoDelete: If true, the queue is deleted when the last consumer unsubscribes. It’s false here to keep the queue alive even if no consumers are currently subscribed.
//            Arguments: These are additional parameters for the queue, set to null here because we don’t need any special arguments.
//            */
//            #endregion

//            // 2. Declare a queue named "hello"
//            channel.QueueDeclare(queue: "hello",
//                                 durable: false,
//                                 exclusive: false,
//            autoDelete: false,
//            arguments: null);

//            #region Clarification
//            //RabbitMQ messages are sent as byte arrays.This is a flexible format that allows any type of data to be sent, not just strings. 
//            #endregion

//            // 3. Convert the message to a byte array

//            var body = Encoding.UTF8.GetBytes(message);

//            // 4. Publish the message to the queue

//            #region Clarification
//            /*
//            exchange : In RabbitMQ, an exchange is responsible for routing messages to queues based on certain criteria, like a routing key.There are different types of exchanges(direct, topic, fanout, headers).
//            Routing key: Specifies which queue the message should go to. Here, it’s "hello", so the message goes to the "hello" queue.
//            BasicProperties: Additional metadata for the message, set to null here for simplicity.
//            Body: The actual message content in byte array form.
//            */
//            #endregion

//            channel.BasicPublish(exchange: "",
//            routingKey: "hello",
//                                 basicProperties: null,
//                                 body: body);
//            Console.WriteLine(" Producer Sent {0}", message);
//        }
//    }
//}

class Producer
{
    public void SendMessage(string message)
    {
        // 1. Create a connection to RabbitMQ server
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            // Declare a queue named "hello"
            channel.QueueDeclare(queue: "hello",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            // Convert the message to a byte array
            var body = Encoding.UTF8.GetBytes(message);

            // Publish the message to the queue
            channel.BasicPublish(exchange: "",
                                 routingKey: "hello",
                                 basicProperties: null,
                                 body: body);
            Console.WriteLine("Producer Sent {0}", message);
        }
    }
}
