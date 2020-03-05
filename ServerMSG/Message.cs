using System;

namespace ServerMSG
{
    public class Message
    {
        public string Owner { get; set; }
        public DateTime Time { get; set; }
        public string Text { get; set; }
        public override string ToString()
        {
            return $"Time: {Time}\t Owner: {Owner}\n\t{Text}";
        }
    }
}
