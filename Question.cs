using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace GeoQuiz
{
    class Question
    {
        private string countries = Properties.Resources.countries;

        private int numberOfChoices = 5;
        private string correctAnswer;
        private string[] choices = new string[5];
        private Random random = new Random();

        public Question()
        {
            for (var i = 0; i < numberOfChoices;i++)
            {
                choices[i] = RandomCountry();
 
            }
            correctAnswer = choices[new Random().Next(numberOfChoices)];
        }

        public string CorrectAnswer { get => correctAnswer;  }
        public string[] Choices { get => choices; }

        public string RandomCountry()
        {
            
            var lineCount = countries.Count(c => c == '\n') ;

            int randomLineNumber = random.Next(lineCount);
           
            var lines = countries.Split('\n');

            Debug.WriteLine("Number of lines: " + lineCount);
            Debug.WriteLine("Random line: " + randomLineNumber+ ": " + lines[randomLineNumber]);

            return lines[randomLineNumber];
        }
    }
}
