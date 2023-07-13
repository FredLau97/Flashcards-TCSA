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
    }
}
