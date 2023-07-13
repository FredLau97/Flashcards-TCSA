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
            _flashcardManager = new();

            Console.WriteLine("Welcome to the Flashcards App!");
            Show();
        }

        public void Show()
        {
            Console.WriteLine("---------------");
            Console.WriteLine("1. Manage Stacks");
            Console.WriteLine("2. Manage Flashcards");
            Console.WriteLine("3. Study");
            Console.WriteLine("4. Exit");
            Console.WriteLine("---------------");

            var input = _inputHandler.GetNumericInput(new[] {1, 2, 3, 4});

            switch(input)
            {
                case 1:
                    _flashcardManager.ManageStacks();
                    break;
                case 2:
                    _flashcardManager.ManageFlashcards();
                    break;
                case 3:
                    _flashcardManager.Study();
                    break;
                case 4:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
