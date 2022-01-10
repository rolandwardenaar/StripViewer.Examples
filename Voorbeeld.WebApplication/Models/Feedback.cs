using System;
using System.Text.Json.Serialization;

namespace Voorbeeld.WebApplication.Models
{
    public class Feedback
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string ReplyEmailAddress { get; set; }

        public int BlockId { get; set; }
        public int KtypeId { get; set; }
        public string PlateNumber { get; set; }
        public string Message { get; set; }

        [JsonIgnore]
        public string Response { get; set; }
    }
}