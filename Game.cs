namespace GeoQuiz
{
    internal class Game
    {
        public Game(string user)
        {
            User = user;
            var totalScore = Database.GetTotalScore(User);
            TotalQuestions = totalScore[0];
            TotalCorrect = totalScore[1];
        }

        public string User { get; set; }
        public int Questions { get; set; }
        public int Correct { get; set; }
        public int TotalQuestions { get; set; }
        public int TotalCorrect { get; set; }
        public bool Online { get; set; } = true;
        public bool DebugMode { get; set; } = false;
    }
}
