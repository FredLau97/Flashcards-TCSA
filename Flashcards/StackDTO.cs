using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    public class StackDTO
    {
        public int StackId { get; set; }
        public string StackName { get; set; }

        public StackDTO() { }

        public StackDTO(int stackId, string stackName)
        {
            StackId = stackId;
            StackName = stackName;
        }
    }
}
