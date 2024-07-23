using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Task1
{
    internal static class Client
    {
        public static async Task UdpSender(string name)
        {
            IPEndPoint senderEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
            UdpClient udpClient = new UdpClient();

            while (true)
            {
                Console.WriteLine("Введите сообщение:");
                string? messageText = Console.ReadLine();

                if (messageText?.ToLower() == "exit")
                {
                    Message exitMessage = new Message(name, messageText);
                    string jsonExitMsg = exitMessage.GetJson();
                    byte[] exitBytes = Encoding.UTF8.GetBytes(jsonExitMsg);
                    await udpClient.SendAsync(exitBytes, senderEndPoint);

                    // Получение подтверждения получения сообщения
                    byte[] acceptExitBytes = udpClient.Receive(ref senderEndPoint);
                    string exitMessageStr = Encoding.UTF8.GetString(acceptExitBytes);
                    Message? exitComittedMessage = Message.GetMessage(exitMessageStr);
                    if (exitComittedMessage?.MessageText == "Exiting command got!") 
                    {
                        Console.WriteLine(exitComittedMessage);
                        break; 
                    }

                }

                Message newMessage = new Message(name, messageText);
                string jsonMsg = newMessage.GetJson();
                byte[] bytes = Encoding.UTF8.GetBytes(jsonMsg);
                await udpClient.SendAsync(bytes, senderEndPoint);

                // Получение подтверждения получения сообщения
                byte[] acceptBytes = udpClient.Receive(ref senderEndPoint);
                string message = Encoding.UTF8.GetString(acceptBytes);
                Message? acceptMessage = Message.GetMessage(message);
                Console.WriteLine(acceptMessage);
            }
        }
    }
}
