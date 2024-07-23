namespace Task1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                await Server.UdpReciever();
            }
            else
            {                
                await Client.UdpSender($"{args[0]}");             
            }
        }
    }
}
