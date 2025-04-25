using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace personality_quiz_1.Models
{
    internal class Question
    {
        public int Id { get; set; }
        public required string Text { get; set; }
        public required List<Answer> Answers { get; set; }
        public string ImageSource { get; set; } = string.Empty;
    }
}
