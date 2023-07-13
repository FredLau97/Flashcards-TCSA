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
    }
}
