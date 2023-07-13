﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    public class FlashcardDTO
    {
        public int DisplayID { get; set; }
        public int FlashcardID { get; set; }
        public string CardFront { get; set; }
        public string CardBack { get; set; }

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
