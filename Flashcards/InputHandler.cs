using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    internal class InputHandler
    {
        /// <summary>
        /// Gets a numeric input from the user, and returns it
        /// </summary>
        /// <param name="validOptions">An int[] of valid inputs</param>
        /// <returns>The input of the user</returns>
        public int GetNumericInput(int[] validOptions)
        {
            Console.Write("Enter your choice: ");
            var input = Console.ReadLine();
            
            // Check if the input is a valid number, and if not, ask again
            while (!int.TryParse(input, out int result) || !validOptions.Contains(result))
            {
                Console.WriteLine($"Input '{input}' is not valid in this context. Please select either of these inputs: {FormatInputCollection(validOptions)}");
                input = GetNumericInput(validOptions).ToString();
            }

            return int.Parse(input);
        }

        /// <summary>
        /// Formats a collection into a string containing each object.
        /// E.g. [1, 2, 3] => "1, 2, 3"]
        /// </summary>
        /// <param name="collection">The collection to format</param>
        /// <returns>A formatted string of the collection</returns>
        private string FormatInputCollection(Array collection)
        {
            var sb = new StringBuilder();

            foreach (var option in collection)
            {
                sb.Append($"{option}, ");
            }

            sb.Remove(sb.Length - 2, 2);

            return sb.ToString();
        }
    }
}
