using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    public class FlashcardDTO
    {
        public int DisplayID { get; set; } // The ID of the flashcard to display. This is not the same as the FlashcardID.
        public int FlashcardID { get; set; } // The ID of the flashcard in the database.
        public string CardFront { get; set; } // The front of the flashcard. This is the question.
        public string CardBack { get; set; } // The back of the flashcard. This is the answer.

        public FlashcardDTO() { }

        public FlashcardDTO(int displayID, int flashcardID, string cardFront, string cardBack)
        {
            DisplayID = displayID;
            FlashcardID = flashcardID;
            CardFront = cardFront;
            CardBack = cardBack;
        }
    }
}
