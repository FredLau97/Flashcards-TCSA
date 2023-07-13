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
                Console.WriteLine($"Input '{input}' is not valid in this context. Please select either of these inputs: {Formatter.FormatInputCollection(validOptions)}");
                input = GetNumericInput(validOptions).ToString();
            }

            return int.Parse(input);
        }

        /// <summary>
        /// Gets a string input from the user, and returns it
        /// </summary>
        /// <param name="validOptions">A string[] of valid inputs</param>
        /// <returns>The input of the user</returns>
        public string GetTextInput(string[] validOptions, bool isCapital = false)
        {
            Console.Write("Enter your choice: ");
            var input = Console.ReadLine();

            if (isCapital) input = input.ToUpper();

            // Check if the input is a valid number, and if not, ask again
            while (!validOptions.Contains(input))
            {
                Console.WriteLine($"Input '{input}' is not valid in this context. Please select either of these inputs: {Formatter.FormatInputCollection(validOptions)}");
                input = GetTextInput(validOptions).ToString();
            }

            return input;
        }

        public string GetTextInput(string message)
        {
            Console.Write(message);
            var input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Input cannot be empty. Please try again.\n");
                input = GetTextInput(message);
            }

            if (input.ToUpper() == "Q") return "Q";

            return input;
        }
    }
}
