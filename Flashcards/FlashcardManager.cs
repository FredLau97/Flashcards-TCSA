using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    internal class FlashcardManager
    {
        private InputHandler _inputHandler;
        private Menu _menu;
        private int pointsPerCard = 10;

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

            Console.WriteLine("Which stack would you like to interact with? (Type N to create a new stack)");
            Formatter.FormatStackDTO(stacks);
            var stackNames = stacks.Select(stack => stack.StackName).ToArray();
            stackNames = stackNames.Append("N").ToArray();
            stackNames = stackNames.Append("n").ToArray();
            var stackName = _inputHandler.GetTextInput(stackNames);

            if (stackName == "N" || stackName == "n")
            {
                CreateStack(dataAccess);
                return;
            }

            var stack = dataAccess.GetStack(stackName);
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

            if (!StackNameIsUnique(stackName))
            {
                Console.WriteLine($"A stack with the name '{stackName}' already exists. Please choose a different name.");
                CreateStack(dataAccess);
                return;
            }

            Console.WriteLine($"The new stack will be named '{stackName}'. Do you want to proceed? Y/N");
            var confirmation = _inputHandler.GetTextInput(new[] { "Y", "N" }, true).ToUpper();

            if (confirmation == "N")
            {
                stackName = _inputHandler.GetTextInput("\nEnter a name for the new stack, or enter Q to return to the main menu: ");
            }

            dataAccess.CreateStack(stackName);
            var stack = dataAccess.GetStack(stackName);
            Console.WriteLine($"The stack '{stack.StackName}' has been created");
            InteractWithStack(stack);
        }

        public void Study()
        {
            Console.WriteLine("---------------");
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("R to return to the main menu");
            Console.WriteLine("C to create a new study session");
            Console.WriteLine("V to view all study sessions");
            Console.WriteLine("---------------");

            var input = _inputHandler.GetTextInput(new[] { "R", "C", "V" }, true);
            switch(input)
            {
                case "R":
                    _menu.Show();
                    break;
                case "C":
                    CreateStudySession();
                    break;
                case "V":
                    ViewStudySessions();
                    break;
            }
        }

        private void ViewStudySessions()
        {
            var dataAccess = new DataAccess();
            var studySessions = dataAccess.GetStudySessions();

            if (studySessions.Count == 0)
            {
                Console.WriteLine("There are currently no study sessions to view.");
                Console.WriteLine("Would you like to create a new study session? Y/N");
                var input = _inputHandler.GetTextInput(new[] { "Y", "N" }, true);
                if (input == "Y")
                {
                    CreateStudySession();
                    return;
                }
                _menu.Show();
                return;
            }

            Formatter.FormatStudySessions(studySessions);
            Study();
        }

        private void CreateStudySession()
        {
            var dataAccess = new DataAccess();
            var stacks = dataAccess.GetStacks();
            var stackNames = stacks.Select(stack => stack.StackName).ToArray();
            Formatter.FormatStackDTO(stacks);
            Console.WriteLine("\nWhich stack would you like to study?");
            var stackName = _inputHandler.GetTextInput(stackNames);
            var stack = stacks.Where(stack => stack.StackName == stackName).FirstOrDefault();
            // Store today date as a DateTime so that it can be stored in SQL DateTime format
            var date = DateTime.Today.ToString("yyyy-MM-dd HH:mm:ss");
            var cardsInStack = dataAccess.GetCardsInStack(stack);
            var maxPoints = cardsInStack.Count * pointsPerCard;
            var studySession = new StudySessionDTO()
            {
                StudyStack = stackName,
                Date = date,
                MaxPoints = maxPoints
            };

            StudyStack(cardsInStack, ref studySession);

        }

        private void StudyStack(List<FlashcardDTO> cardsToStudy, ref StudySessionDTO studySession)
        {
            foreach(var card in cardsToStudy)
            {
                Console.WriteLine($"---------------\n{card.CardFront}\n---------------");
                Console.WriteLine("Press enter to reveal the answer");
                Console.ReadLine();
                Console.WriteLine($"---------------\n{card.CardBack}\n---------------");
                Console.WriteLine("Did you get it right? Y/N");
                var input = _inputHandler.GetTextInput(new[] { "Y", "N" }, true);
                if (input == "Y")
                {
                    studySession.PointsGained += pointsPerCard;
                }
            }

            Console.WriteLine($"You have finished studying the stack {studySession.StudyStack}.");
            Console.WriteLine($"You earned {studySession.PointsGained} out of {studySession.MaxPoints} points.");
            Console.WriteLine("Would you like to save this study session? Y/N");
            var save = _inputHandler.GetTextInput(new[] { "Y", "N" }, true);
            if (save == "Y")
            {
                var dataAccess = new DataAccess();
                dataAccess.CreateStudySession(studySession);
                Console.WriteLine("Study session saved.");
            }
            
            Study();
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
            Console.WriteLine($"Are you sure you want to delete this stack ({stack.StackName})? Y/N");
            var confirmDelete = _inputHandler.GetTextInput(new string[] { "Y", "N" }, true).ToUpper() == "Y";

            if (!confirmDelete)
            {
                InteractWithStack(stack);
                return;
            }

            var dataAccessLayer = new DataAccess();
            dataAccessLayer.DeleteStack(stack.StackId);
            Console.WriteLine("Stack deleted successfully.");
            _menu.Show();
        }

        private void DeleteFlashcard(StackDTO stack)
        {
            var flashcardsInStack = GetFlashcards(stack);
            var flashcardIDs = flashcardsInStack.Select(flashcard => flashcard.DisplayID).ToArray();
            Console.WriteLine("Which flashcard (ID) would you like to delete?");
            var flashcardID = _inputHandler.GetNumericInput(flashcardIDs);
            Console.WriteLine($"Are you sure you want to delete this flashcard ({flashcardID})? Y/N");
            var confirmDelete = _inputHandler.GetTextInput(new string[] { "Y", "N" }, true).ToUpper() == "Y";

            if (!confirmDelete)
            {
                InteractWithStack(stack);
                return;
            }

            var dataAccess = new DataAccess();
            dataAccess.DeleteFlashcard(flashcardID);
            Console.WriteLine("Flashcard deleted successfully.");
            InteractWithStack(stack);
        }

        private void EditFlashcard(StackDTO stack)
        {
            var flashcardsInStack = GetFlashcards(stack);
            var flashcardIDs = flashcardsInStack.Select(flashcard => flashcard.DisplayID).ToArray();
            Console.WriteLine("Which flashcard (ID) would you like to edit?");
            var flashcardDisplayID = _inputHandler.GetNumericInput(flashcardIDs);
            var flashcardID = flashcardsInStack.Where(flashcard => flashcard.DisplayID == flashcardDisplayID).Select(flashcard => flashcard.FlashcardID).FirstOrDefault();
            var flashcard = CreateFlashcardData();
            flashcard.FlashcardID = flashcardID;
            var dataAccess = new DataAccess();
            dataAccess.EditFlashcard(flashcard);
            Console.WriteLine("Flashcard has been changed.");
            InteractWithStack(stack);
        }

        private List<FlashcardDTO> GetFlashcards(StackDTO stack)
        {
            var dataAccess = new DataAccess();
            var flashcardsInStack = dataAccess.GetCardsInStack(stack);

            if (flashcardsInStack.Count == 0)
            {
                Console.WriteLine("There are currently no flashcards in this stack.");
                InteractWithStack(stack);
                return null;
            }

            Formatter.FormatFlashcardDTO(flashcardsInStack);
            return flashcardsInStack;
        }

        private void CreateFlashcard(StackDTO stack)
        {
            var flashCard = CreateFlashcardData();

            var dataAccess = new DataAccess();
            dataAccess.CreateFlashcard(flashCard, stack);
            Console.WriteLine("Flashcard created successfully.");

            InteractWithStack(stack);
        }

        private FlashcardDTO CreateFlashcardData()
        {
            var cardFront = _inputHandler.GetTextInput("\nEnter the question for the flashcard: ");
            var cardBack = _inputHandler.GetTextInput("Enter the answer for the flashcard: ");
            var flashCard = new FlashcardDTO
            {
                CardFront = cardFront,
                CardBack = cardBack,
            };
            return flashCard;
        }

        private void ViewStack(StackDTO stack)
        {
            var dataAccess = new DataAccess();
            var flashcards = dataAccess.GetCardsInStack(stack);

            if (flashcards.Count < 1)
            {
                Console.WriteLine("There are currently no flashcards in this stack.");
                InteractWithStack(stack);
                return;
            }

            Formatter.FormatFlashcardDTO(flashcards);
            InteractWithStack(stack);
        }

        private bool StackNameIsUnique(string stackName)
        {
            var dataAccess = new DataAccess();
            var stacks = dataAccess.GetStacks();
            return !stacks.Any(stack => stack.StackName.ToLower() == stackName.ToLower());
        }
    }
}
