using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    internal class FlashcardDTO
    {
        public int FlashcardID { get; set; }
        public string CardFront { get; set; }
        public string CardBack { get; set; }

        public FlashcardDTO() { }

        public FlashcardDTO(int flashcardID, string cardFront, string cardBack)
        {
            FlashcardID = flashcardID;
            CardFront = cardFront;
            CardBack = cardBack;
        }
    }
}
