using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp10
{
    [Serializable]
    class Message
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
