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
        private int pointsPerCard = 10; // The number of points a user can earn for correctly answering a flashcard

        public FlashcardManager(InputHandler inputHandler, Menu menu)
        {
            _inputHandler = inputHandler;
            _menu = menu;
        }

        /// <summary>
        /// Enables the user to interact with a stack of flashcards.
        /// If there are no flashcards in the stack, the user will be prompted to create a new flashcard.
        /// </summary>
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

        /// <summary>
        /// Prompts the user for the name of a new stack, and creates it.
        /// </summary>
        /// <param name="dataAccess"></param>
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

        /// <summary>
        /// Enables the user to interact with the study interface.
        /// </summary>
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

        /// <summary>
        /// Displays all study sessions in the database.
        /// If there are no study sessions, the user will be prompted to create a new study session.
        /// </summary>
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

        /// <summary>
        /// Prompts the user to create a new study session.
        /// User is prompted to select a stack to study, and then the study session begins.
        /// </summary>
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

        /// <summary>
        /// Takes a list of flashcards and a study session, and enables the user to study the flashcards.
        /// and asks the user if they got the card right, and awards points accordingly.
        /// Allows the user to view the answer before moving on to the next card.
        /// Asks the user if they want to save the session when they are done.
        /// </summary>
        /// <param name="cardsToStudy"></param>
        /// <param name="studySession"></param>
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

        /// <summary>
        /// Enables the user to interact with a stack.
        /// </summary>
        /// <param name="stack"></param>
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

        /// <summary>
        /// Deletes a stack from the database. Asks the user to confirm the deletion.
        /// </summary>
        /// <param name="stack"></param>
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

        /// <summary>
        /// Deletes a flashcard from the database. Asks the user to confirm the deletion.
        /// </summary>
        /// <param name="stack"></param>
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

        /// <summary>
        /// Prompts the user to enter the data for to edit the flashcard, and creates it in the database.
        /// </summary>
        /// <param name="stack"></param>
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

        /// <summary>
        /// Displays all flashcards in a stack, and returns them as a list.
        /// </summary>
        /// <param name="stack"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Prompts the user to enter the data for a new flashcard, and creates it in the database.
        /// </summary>
        /// <param name="stack"></param>
        private void CreateFlashcard(StackDTO stack)
        {
            var flashCard = CreateFlashcardData();

            var dataAccess = new DataAccess();
            dataAccess.CreateFlashcard(flashCard, stack);
            Console.WriteLine("Flashcard created successfully.");

            InteractWithStack(stack);
        }

        /// <summary>
        /// Creates a new flashcard object from user input.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Displays all stacks in the database.
        /// </summary>
        /// <param name="stack"></param>
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

        /// <summary>
        /// Check if the stack name is unique.
        /// </summary>
        /// <param name="stackName"></param>
        /// <returns></returns>
        private bool StackNameIsUnique(string stackName)
        {
            var dataAccess = new DataAccess();
            var stacks = dataAccess.GetStacks();
            return !stacks.Any(stack => stack.StackName.ToLower() == stackName.ToLower());
        }
    }
}
