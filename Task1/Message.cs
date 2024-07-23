using System.Text.Json;

namespace Task1
{
    public class Message
    {
        public string SenderName { get; set; }
        public string? MessageText { get; set; }
        public DateTime MessageTime { get; set; }

        public Message(string senderName, string messageText)
        {
            SenderName = senderName;
            MessageText = messageText;
            MessageTime = DateTime.Now;
        }
        
        public string GetJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public static Message? GetMessage(string jsonString)
        {
            return JsonSerializer.Deserialize<Message>(jsonString);
        }

        public override string ToString()
        {
            return $"От: {SenderName} ({MessageTime.ToString("HH:mm:ss")}): {MessageText}";
        }
    }
}
