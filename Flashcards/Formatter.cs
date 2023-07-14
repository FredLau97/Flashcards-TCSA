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

        /// <summary>
        /// Formats the stacks into a table for the user to see.
        /// </summary>
        /// <param name="stacks">List of StackDTO to display.</param>
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
                    .WithColumn("Name")
                    .ExportAndWriteLine();

                return;
            }

            return;
        }

        /// <summary>
        /// Formats the flashcards into a table for the user to see.
        /// </summary>
        /// <param name="flashcards">List of FlashcardDTO to display.</param>
        public static void FormatFlashcardDTO(List<FlashcardDTO> flashcards)
        {
            var tableData = new List<List<Object>>();

            foreach (var card in flashcards)
            {
                tableData.Add(new List<object> { card.DisplayID, card.CardFront, card.CardBack });
            }

            ConsoleTableBuilder
                .From(tableData)
                .WithTitle("Flashcards")
                .WithColumn("ID", "Front", "Back")
                .ExportAndWriteLine();
        }

        /// <summary>
        /// Formats the study sessions into a table for the user to see.
        /// </summary>
        /// <param name="studySessions">List of StudySessionDTO to display</param>
        public static void FormatStudySessions(List<StudySessionDTO> studySessions)
        {
            var tableData = new List<List<Object>>();

            foreach (var session in studySessions)
            {
                tableData.Add(new List<object> { session.Date, session.StudyStack, session.MaxPoints, session.PointsGained });
            }

            ConsoleTableBuilder
                .From(tableData)
                .WithTitle("Study Sessions")
                .WithColumn("Date", "Studied Stack", "Max Points", "Points Earned")
                .ExportAndWriteLine();
        }
    }
}
