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

        public Menu() 
        {
            _inputHandler = new();

            Console.WriteLine("Welcome to the Flashcards App!");
            Show();
        }

        public void Show()
        {
            Console.WriteLine("---------------");
            Console.WriteLine("1. Add a new flashcard");
            Console.WriteLine("2. Show all flashcards");
            Console.WriteLine("3. Practice");
            Console.WriteLine("4. Exit");
            Console.WriteLine("---------------");

            var input = _inputHandler.GetNumericInput(new[] {1, 2, 3, 4});
        }
    }
}
