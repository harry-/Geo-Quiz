using System;
using System.Linq;
using System.Diagnostics;

namespace GeoQuiz
{
    internal class Question
    {
        private readonly string _countries = Properties.Resources.countries;
        private const int NumberOfChoices = 6;
        private readonly Random _random = new Random();

        public Question()
        {
            for (var i = 0; i < NumberOfChoices;i++)
            {
                Choices[i] = RandomCountry();
            }
            CorrectAnswer = Choices[new Random().Next(NumberOfChoices)];
        }

        public string CorrectAnswer { get; }
        public string[] Choices { get; } = new string[6];

        public string RandomCountry()
        {
            int lineCount = _countries.Count(c => c == '\n') ;

            int randomLineNumber = _random.Next(lineCount);
           
            var lines = _countries.Split('\n');

            Debug.WriteLine("Number of lines: " + lineCount);
            Debug.WriteLine("Random line: " + randomLineNumber+ ": " + lines[randomLineNumber]);

            return lines[randomLineNumber];
        }
    }
}
