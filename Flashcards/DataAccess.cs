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

        public List<StackDTO> GetStacks()
        {
            var stacks = _connection.GetStacks();
            return stacks;
        }

        public void CreateStack(string stackName)
        {
            _connection.CreateStack(stackName);
        }

        public StackDTO GetStack(string stackName)
        {
            var stack = _connection.GetStack(stackName);
            return stack;
        }

        public StackDTO GetStack(int stackId)
        {
            var stack = _connection.GetStack(stackId);
            return stack;
        }

        public void DeleteStack(int stackId)
        {
            _connection.DeleteStack(stackId);
        }

        public void CreateFlashcard(FlashcardDTO flashCard, StackDTO stack)
        {
            _connection.CreateFlashcard(flashCard, stack);
        }

        public List<FlashcardDTO> GetCardsInStack(StackDTO stack)
        {
            var cards = _connection.GetCardsInStack(stack);
            return cards;
        }

        public void EditFlashcard(FlashcardDTO flashcard)
        {
            _connection.EditFlashcard(flashcard);
        }

        public void DeleteFlashcard(int flashcardID)
        {
            _connection.DeleteFlashcard(flashcardID);
        }

        public List<StudySessionDTO> GetStudySessions()
        {
            var studySessions = _connection.GetStudySessions();
            return studySessions;
        }

        public void CreateStudySession(StudySessionDTO studySession)
        {
            _connection.CreateStudySession(studySession);
        }
    }
}
