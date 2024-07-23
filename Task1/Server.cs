using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Task1
{
    internal static class Server
    {        
        static private CancellationTokenSource cts = new CancellationTokenSource();
        static private CancellationToken ct;

        public static async Task UdpReciever()
        {
            IPEndPoint receiverEndPoint = new IPEndPoint(IPAddress.Any, 0);
            UdpClient udpClient = new UdpClient(12345);
            Console.WriteLine("Receiver is waiting for messages...");
            ct = cts.Token;


            new Task(() =>
            {
                while (true)
                {
                    if (Console.ReadKey().Key == ConsoleKey.Escape)                           
                        break;
                }

                // Отправка сообщения о завершении работы в консоль сервера                         
                Message escapeMessage = new Message("Udp Server", "Getting Esc...!\nServer shutdown!");
                Console.WriteLine("x" + escapeMessage);
                Environment.Exit(0);                
            }).Start();               

            while (ct.IsCancellationRequested != true)
            {
                try
                {
                    byte[] bytes = udpClient.Receive(ref receiverEndPoint);
                    string message = Encoding.UTF8.GetString(bytes);

                    await Task.Run(async () =>
                    {
                        Message? newMessage = Message.GetMessage(message);

                        if (newMessage?.MessageText?.ToLower() == "exit")
                        {
                            cts.Cancel();

                            // Отправка подтверждения получения сообщения завершения работы сервера
                            Message acceptMessage = new Message("Udp Server", "Exiting command got!");
                            string jsonMsg = acceptMessage.GetJson();
                            byte[] respondBytes = Encoding.UTF8.GetBytes(jsonMsg);
                            await udpClient.SendAsync(respondBytes, receiverEndPoint);
                            Console.WriteLine(acceptMessage);
                            Thread.Sleep(500);
                        }
                        else
                        {
                            if (newMessage != null)
                            {
                                Console.WriteLine(newMessage);                                

                                // Отправка подтверждения получения сообщения
                                Message acceptMessage = new Message("Udp Server", "Message accepted!");
                                string jsonMsg = acceptMessage.GetJson();
                                byte[] respondBytes = Encoding.UTF8.GetBytes(jsonMsg);
                                await udpClient.SendAsync(respondBytes, receiverEndPoint);
                            }
                            else
                                Console.WriteLine("Somthing went wrong with message!");
                        }
                    });                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }


            }
        }
    }
}
