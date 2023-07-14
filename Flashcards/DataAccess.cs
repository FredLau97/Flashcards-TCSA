using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    internal class DataAccess
    {
        private DatabaseConnection _connection;

        public DataAccess()
        {
            _connection = new();
        }

        /// <summary>
        /// Gets all stacks from the database.
        /// </summary>
        /// <returns>A list of all the stacks.</returns>
        public List<StackDTO> GetStacks()
        {
            var stacks = _connection.GetStacks();
            return stacks;
        }

        /// <summary>
        /// Creates a new stack in the database.
        /// </summary>
        /// <param name="stackName">The name of the stack</param>
        public void CreateStack(string stackName)
        {
            _connection.CreateStack(stackName);
        }

        /// <summary>
        /// Gets a stack from the database by the name of the stack.
        /// </summary>
        /// <param name="stackName">The name of the stack</param>
        /// <returns>The StackDTO</returns>
        public StackDTO GetStack(string stackName)
        {
            var stack = _connection.GetStack(stackName);
            return stack;
        }

        /// <summary>
        /// Gets a stack from the database by the id of the stack.
        /// </summary>
        /// <param name="stackId">The id of the stack.</param>
        /// <returns>The StackDTO</returns>
        public StackDTO GetStack(int stackId)
        {
            var stack = _connection.GetStack(stackId);
            return stack;
        }

        /// <summary>
        /// Deletes a stack from the database.
        /// </summary>
        /// <param name="stackId">The id of the stack to delete.</param>
        public void DeleteStack(int stackId)
        {
            _connection.DeleteStack(stackId);
        }

        /// <summary>
        /// Creates a new flashcard in the database.
        /// </summary>
        /// <param name="flashCard">The flashcardDTO</param>
        /// <param name="stack">The StackDTO the flashcard was created in</param>
        public void CreateFlashcard(FlashcardDTO flashCard, StackDTO stack)
        {
            _connection.CreateFlashcard(flashCard, stack);
        }

        /// <summary>
        /// Gets all the flashcards in a stack.
        /// </summary>
        /// <param name="stack">The StackDTO</param>
        /// <returns>List of all the flashcards in the stack</returns>
        public List<FlashcardDTO> GetCardsInStack(StackDTO stack)
        {
            var cards = _connection.GetCardsInStack(stack);
            return cards;
        }

        /// <summary>
        /// Edit a flashcard in the database.
        /// </summary>
        /// <param name="flashcard">The FlashcardDTO to edit.</param>
        public void EditFlashcard(FlashcardDTO flashcard)
        {
            _connection.EditFlashcard(flashcard);
        }

        /// <summary>
        /// Deletes a flashcard from the database.
        /// </summary>
        /// <param name="flashcardID">The id of the flashcard to delete.</param>
        public void DeleteFlashcard(int flashcardID)
        {
            _connection.DeleteFlashcard(flashcardID);
        }

        /// <summary>
        /// Gets all the study sessions from the database.
        /// </summary>
        /// <returns>List of StudySessionDTO</returns>
        public List<StudySessionDTO> GetStudySessions()
        {
            var studySessions = _connection.GetStudySessions();
            return studySessions;
        }

        /// <summary>
        /// Create a new study session in the database.
        /// </summary>
        /// <param name="studySession">The StudySessionDTO</param>
        public void CreateStudySession(StudySessionDTO studySession)
        {
            _connection.CreateStudySession(studySession);
        }
    }
}
