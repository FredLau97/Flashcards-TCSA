namespace Flashcards
{
    public class StudySessionDTO
    {
        public string Date { get; set; }
        public string StudyStack { get; set; }
        public int MaxPoints { get; set; }
        public int PointsGained { get; set; }

        public StudySessionDTO(string date, string studyStack, int maxPoints, int pointsGained)
        {
            Date = date;
            StudyStack = studyStack;
            MaxPoints = maxPoints;
            PointsGained = pointsGained;
        }

        public StudySessionDTO()
        {
        }
    }
}