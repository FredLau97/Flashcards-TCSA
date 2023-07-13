using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Flashcards
{
    internal class DatabaseConnection
    {
        private SqlConnection _connection;
        private SqlCommand _cmd;
        private SqlDataAdapter _adapter;

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
                    Console.WriteLine("Connection to database established");
                    return true;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine($"Connection to database failed...\n" +
                    $"Error: {e.Message}");
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
                    Console.WriteLine("Connection to database closed");
                    return true;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine($"Connection to database failed to close...\n" +
                                       $"Error: {e.Message}");
            }

            return false;
        }
    }
}
