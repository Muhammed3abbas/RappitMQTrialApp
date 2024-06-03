using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Send a message");
            Console.WriteLine("2. Start listening for messages");
            Console.WriteLine("3. Exit");

            var choice = Console.ReadLine();

            if (choice == "1")
            {
                var producer = new Producer();
                Console.WriteLine("Enter the message to send:");
                var message = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(message))
                {
                    producer.SendMessage(message);
                }
                else
                {
                    Console.WriteLine("No message provided.");
                }
            }
            else if (choice == "2")
            {
                var consumer = new Consumer();
                await consumer.StartListening();
            }
            else if (choice == "3")
            {
                break; // Exit the loop
            }
            else
            {
                Console.WriteLine("Invalid choice");
            }
        }
    }
}
