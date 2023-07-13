using ConsoleTableExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    public static class Formatter
    {
        /// <summary>
        /// Formats a collection into a string containing each object.
        /// E.g. [1, 2, 3] => "1, 2, 3"]
        /// </summary>
        /// <param name="collection">The collection to format</param>
        /// <returns>A formatted string of the collection</returns>
        public static string FormatInputCollection(Array collection)
        {
            var sb = new StringBuilder();

            foreach (var option in collection)
            {
                sb.Append($"{option}, ");
            }

            // Remove the last ", " from the string
            sb.Remove(sb.Length - 2, 2);

            return sb.ToString();
        }

        public static void FormatStackDTO(List<StackDTO> stacks)
        {
            var tableData = new List<List<Object>>();

            foreach (var stack in stacks)
            {
                tableData.Add(new List<object> { stack.StackName });
            }

            if (tableData.Count > 0)
            {
                ConsoleTableBuilder
                    .From(tableData)
                    .WithTitle("Stacks")
                    .ExportAndWriteLine();

                return;
            }

            return;
        }
    }
}
