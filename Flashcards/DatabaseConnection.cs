using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Flashcards
{
    internal class DatabaseConnection
    {
        private SqlConnection _connection;
        private SqlCommand _cmd;
        private SqlDataAdapter _adapter;
        private string _sql;

        public DatabaseConnection()
        {
            _connection = new(ConfigurationManager.ConnectionStrings["Flashcards"].ConnectionString);
            _cmd = new();
            _adapter = new();
        }

        // Opens the connection to the database
        // Checks if the connection is already open, and if not, tries to open it
        // Returns true if the connection was openend successfully, otherwise returns false
        // If there was an error, it will be logged to the console
        public bool OpenConnection()
        {
            try
            {
                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                    // Console.WriteLine("Connection to database established");
                    return true;
                }
            }
            catch (SqlException e)
            {
                //Console.WriteLine($"Connection to database failed...\n" + $"Error: {e.Message}");
            }

            return false;
        }

        public bool CloseConnection()
        {
            try
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                    // Console.WriteLine("Connection to database closed");
                    return true;
                }
            }
            catch (SqlException e)
            {
                //Console.WriteLine($"Connection to database failed to close...\n" + $"Error: {e.Message}");
            }

            return false;
        }

        public List<StackDTO> GetStacks()
        {
            if (!OpenConnection()) return null;

            _sql = "SELECT * FROM Stacks";
            _cmd = new(_sql, _connection);
            var stacks = new List<StackDTO>();
            var reader = _cmd.ExecuteReader();

            while (reader.Read())
            {
                var stackId = reader.GetInt32(0);
                var stackName = reader.GetString(1);
                var stack = new StackDTO(stackId, stackName);
                stacks.Add(stack);
            }

            CloseConnection();
            return stacks;
        }

        public void CreateStack(string stackName)
        {
            if (!OpenConnection()) return;

            _sql = $"INSERT INTO Stacks (StackName) VALUES ('{stackName}')";
            _cmd = new(_sql, _connection);
            _cmd.ExecuteNonQuery();

            CloseConnection();
        }

        public StackDTO GetStack(string stackName)
        {
            if (!OpenConnection()) return null;

            _sql = $"SELECT * FROM Stacks WHERE StackName = '{stackName}'";
            _cmd = new(_sql, _connection);
            var reader = _cmd.ExecuteReader();
            var stack = new StackDTO();

            while (reader.Read())
            {
                var stackId = reader.GetInt32(0);
                stack.StackId = stackId;
                stack.StackName = stackName;
            }

            CloseConnection();
            return stack;
        }

        public StackDTO GetStack(int stackId)
        {
            if (!OpenConnection()) return null;

            _sql = $"SELECT * FROM Stacks WHERE StackID = {stackId}";
            _cmd = new(_sql, _connection);
            var reader = _cmd.ExecuteReader();
            var stack = new StackDTO();

            while (reader.Read())
            {
                var stackName = reader.GetString(1);
                stack.StackId = stackId;
                stack.StackName = stackName;
            }

            CloseConnection();
            return stack;
        }

        public void DeleteStack(int stackId)
        {
            if (!OpenConnection()) return;

            DeleteCardsInStack(stackId);
            _sql = $"DELETE FROM Stacks WHERE StackID = {stackId}";
            _cmd = new(_sql, _connection);
            _cmd.ExecuteNonQuery();
            CloseConnection();
        }

        private void DeleteCardsInStack(int stackId)
        {
            _sql = $"DELETE FROM Cards WHERE StackID = {stackId}";
            _cmd = new(_sql, _connection);
            _cmd.ExecuteNonQuery();
        }

        public void CreateFlashcard(FlashcardDTO flashCard, StackDTO stack)
        {
            if (!OpenConnection()) return;

            _sql = $"INSERT INTO Cards (CardFront, CardBack, StackID) VALUES ('{flashCard.CardFront}', '{flashCard.CardBack}', {stack.StackId})";
            _cmd = new(_sql, _connection);
            _cmd.ExecuteNonQuery();
            CloseConnection();
        }

        public List<FlashcardDTO> GetCardsInStack(StackDTO stack)
        {
            if (!OpenConnection()) return null;

            _sql = $"SELECT * FROM Cards WHERE StackID = {stack.StackId}";
            _cmd = new(_sql, _connection);
            var reader = _cmd.ExecuteReader();
            var cards = new List<FlashcardDTO>();
            var iterator = 1;

            while (reader.Read())
            {
                var displayID = iterator++;
                var cardId = reader.GetInt32(0);
                var cardFront = reader.GetString(1);
                var cardBack = reader.GetString(2);
                var card = new FlashcardDTO(displayID, cardId, cardFront, cardBack);
                cards.Add(card);
            }

            CloseConnection();
            return cards;
        }

        public void EditFlashcard(FlashcardDTO flashcard)
        {
            if (!OpenConnection()) return;

            _sql = $"UPDATE Cards SET CardFront = '{flashcard.CardFront}', CardBack = '{flashcard.CardBack}' WHERE CardID = {flashcard.FlashcardID}";
            _cmd = new(_sql, _connection);
            _cmd.ExecuteNonQuery();
            CloseConnection();
        }

        public void DeleteFlashcard(int flashcardID)
        {
            if (!OpenConnection()) return;

            _sql = $"DELETE FROM Cards WHERE CardID = {flashcardID}";
            _cmd = new(_sql, _connection);
            _cmd.ExecuteNonQuery();
            CloseConnection();
        }
    }
}
