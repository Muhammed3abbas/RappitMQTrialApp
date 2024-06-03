using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Dapper;
using Microsoft.Data.SqlClient;



//class Consumer
//{
//    // Connection string to the MS SQL Server database
//    //private static string connectionString = "Server=localhost;Database=RabbitMQExampleDB;User Id=your_username;Password=your_password;";
//    private static string connectionString = "Server=.;Database=RabbitMQExampleDB;Trusted_Connection=True;TrustServerCertificate=True";

//    public void StartListening()
//    {
//        // Create a connection to RabbitMQ server & Opens a channel for communication with RabbitMQ.
//        var factory = new ConnectionFactory() { HostName = "localhost" };
//        using (var connection = factory.CreateConnection())
//        using (var channel = connection.CreateModel())
//        {
//            // Declare a queue named "hello" to Ensures the queue named "hello" exists. If it doesn't, it creates the queue.
//            channel.QueueDeclare(queue: "hello",
//                                 durable: false,
//                                 exclusive: false,
//                                 autoDelete: false,
//                                 arguments: null);


//            // Create a consumer to listen for messages
//            var consumer = new EventingBasicConsumer(channel);
//            consumer.Received += (model, ea) =>
//            {
//                // Get the message body
//                var body = ea.Body.ToArray();
//                //Converts the message body from byte array to string.
//                var message = Encoding.UTF8.GetString(body);
//                Console.WriteLine(" Consumer aaReceived {0}", message);

//                // Save the message to the database
//                SaveMessageToDatabase(message);
//            };
//            // Start consuming messages
//            channel.BasicConsume(queue: "hello",
//                                 autoAck: true,
//                                 consumer: consumer);

//            //Console.WriteLine(" Press [enter] to exit.");
//            Console.ReadLine();
//        }
//    }

//    private void SaveMessageToDatabase(string message)
//    {
//        // Open a connection to the database and save the message
//        using (var connection = new SqlConnection(connectionString))
//        {
//            var query = "INSERT INTO Messages (Text) VALUES (@Text)";
//            connection.Execute(query, new { Text = message });
//            Console.WriteLine(" Message saved to database");
//        }
//    }
////}

class Consumer
{
    // Connection string to the MS SQL Server database
    private static string connectionString = "Server=.;Database=RabbitMQExampleDB;Trusted_Connection=True;TrustServerCertificate=True";


    public async Task StartListening()
    {
        // Create a connection to RabbitMQ server & Opens a channel for communication with RabbitMQ.
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            // Declare a queue named "hello" to Ensures the queue named "hello" exists. If it doesn't, it creates the queue.
            channel.QueueDeclare(queue: "hello",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            // Create a consumer to listen for messages
            var consumer = new EventingBasicConsumer(channel);
            var messageReceived = new TaskCompletionSource<bool>();

            consumer.Received += (model, ea) =>
            {
                // Get the message body
                var body = ea.Body.ToArray();
                // Converts the message body from byte array to string.
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine("Consumer Received {0}", message);

                // Save the message to the database
                SaveMessageToDatabase(message);

                // Set the messageReceived flag to true
                messageReceived.TrySetResult(true);
            };

            // Start consuming messages asynchronously
            channel.BasicConsume(queue: "hello",
                                 autoAck: true,
                                 consumer: consumer);

            // Wait asynchronously for a message to arrive
            await Task.WhenAny(messageReceived.Task, Task.Delay(1000)); // Timeout after 1 seconds (adjustable)

            // If no message arrived, display a message indicating no messages available
            if (!messageReceived.Task.IsCompleted)
            {
                Console.WriteLine("No messages available to listen to.");
            }
        }
    }


    private void SaveMessageToDatabase(string message)
    {
        // Open a connection to the database and save the message using dapper
        using (var connection = new SqlConnection(connectionString))
        {
            var query = "INSERT INTO Messages (Text) VALUES (@Text)";
            connection.Execute(query, new { Text = message });
            Console.WriteLine("Message saved to database");
        }
    }


}



