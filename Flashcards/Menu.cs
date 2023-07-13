using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    internal class Menu
    {
        private InputHandler _inputHandler;
        private FlashcardManager _flashcardManager;

        public Menu() 
        {
            _inputHandler = new();
            _flashcardManager = new(_inputHandler, this);

            Console.WriteLine("Welcome to the Flashcards App!");
            Show();
        }

        public void Show()
        {
            Console.WriteLine("---------------");
            Console.WriteLine("1. Manage Stacks");
            Console.WriteLine("2. Study");
            Console.WriteLine("3. Exit");
            Console.WriteLine("---------------");

            var input = _inputHandler.GetNumericInput(new[] {1, 2, 3});

            switch(input)
            {
                case 1:
                    _flashcardManager.ManageStacks();
                    break;
                case 2:
                    _flashcardManager.Study();
                    break;
                case 3:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
