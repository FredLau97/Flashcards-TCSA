using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    internal class FlashcardManager
    {
        private InputHandler _inputHandler;
        private Menu _menu;

        public FlashcardManager(InputHandler inputHandler, Menu menu)
        {
            _inputHandler = inputHandler;
            _menu = menu;
        }

        public void ManageStacks()
        {
            var dataAccess = new DataAccess();
            var stacks = dataAccess.GetStacks();

            if (stacks.Count == 0)
            {
                Console.WriteLine("There are currently no stacks of flashcards to manage.");
                Console.WriteLine("Would you like to create a new stack? Y/N");
                var input = _inputHandler.GetTextInput(new[] { "Y", "N" }, true);

                if (input == "Y")
                {
                    CreateStack(dataAccess);
                    return;
                }

                _menu.Show();
                return;
            }

            Console.WriteLine("Which stack (ID) would you like to interact with?");
            Formatter.FormatStackDTO(stacks);
            var stackIDs = stacks.Select(stack => stack.StackId).ToArray();
            var stackID = _inputHandler.GetNumericInput(stackIDs);
            var stack = dataAccess.GetStack(stackID);
            InteractWithStack(stack);
        }

        private void CreateStack(DataAccess dataAccess)
        {
            var stackName = _inputHandler.GetTextInput("\nEnter a name for the new stack, or enter Q to return to the main menu: ");
            if (stackName == "Q")
            {
                _menu.Show();
                return;
            }
            
            dataAccess.CreateStack(stackName);
            var stack = dataAccess.GetStack(stackName);
            Console.WriteLine($"The stack '{stack.StackName}' has been created");
            InteractWithStack(stack);
        }

        public void ManageFlashcards()
        {
            Console.WriteLine("Manage Flashcards");
        }

        public void Study()
        {
            Console.WriteLine("Study");
        }

        private void InteractWithStack(StackDTO stack)
        {
            Console.WriteLine("---------------");
            Console.WriteLine($"What would you like to do with this stack ({stack.StackName})");
            Console.WriteLine("R to return to the main menu");
            Console.WriteLine("X to change stack");
            Console.WriteLine("V to view all flashcards in the stack");
            Console.WriteLine("C to create a new flashcard in the stack");
            Console.WriteLine("E to edit a flashcard in the stack");
            Console.WriteLine("D to delete a flashcard in the stack");
            Console.WriteLine("A to delete the stack");
            Console.WriteLine("---------------");

            var input = _inputHandler.GetTextInput(new[] { "R", "X", "V", "C", "E", "D", "A" }, true);
            switch(input)
            {
                case "R":
                    _menu.Show();
                    break;
                case "X":
                    ManageStacks();
                    break;
                case "V":
                    ViewStack(stack);
                    break;
                case "C":
                    CreateFlashcard(stack);
                    break;
                case "E":
                    EditFlashcard(stack);
                    break;
                case "D":
                    DeleteFlashcard(stack);
                    break;
                case "A":
                    DeleteStack(stack);
                    break;

            }
        }

        private void DeleteStack(StackDTO stack)
        {
            throw new NotImplementedException();
        }

        private void DeleteFlashcard(StackDTO stack)
        {
            throw new NotImplementedException();
        }

        private void EditFlashcard(StackDTO stack)
        {
            throw new NotImplementedException();
        }

        private void CreateFlashcard(StackDTO stack)
        {
            throw new NotImplementedException();
        }

        private void ViewStack(StackDTO stack)
        {
            throw new NotImplementedException();
        }
    }
}
