using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace GeoQuiz
{
    public partial class GeoQuiz : Form
    {
        private Question question;
        private int score;
        private int questionsAsked;

        public GeoQuiz()
        {
            InitializeComponent();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                string country = textBox1.Text;
                Debug.WriteLine(Properties.Resources.ApiKey);

                pictureBox1.ImageLocation = "https://maps.googleapis.com/maps/api/staticmap?size=400x400&maptype=hybrid&center=" + country + "&style=feature:administrative.country|element:labels.text|visibility:off|&&size=400x400&markers=color:blue|" + country + "&key="+Properties.Resources.ApiKey;
            }
        }

        private void GeoQuiz_Load(object sender, EventArgs e)
        {
            NextQuestion();
        }

        private void NextQuestion()
        {
            question = new Question();

            label1.Text= "Score: " + score + "/" + questionsAsked;
            questionsAsked++;

            pictureBox1.ImageLocation = "https://maps.googleapis.com/maps/api/staticmap?size=400x400&maptype=hybrid&center=" + question.CorrectAnswer + "&style=feature:administrative.country|element:labels.text|visibility:off|&&size=400x400&markers=color:blue|" + question.CorrectAnswer + " &key=" + Properties.Resources.ApiKey; 

            Button[] buttons = { button1, button2, button3, button6, button7 };
            for (int i = 0; i < question.Choices.Length; i++)
            {
                buttons[i].Text = question.Choices[i];
            }
        }

        private void Choice(string choice)
        {
            if (question.CorrectAnswer == choice)
            {
                Debug.WriteLine("richtig");
                pictureBox2.Image = Properties.Resources.OK;
                score++;
            } else
            {
                Debug.WriteLine("falsch");
                pictureBox2.Image = Properties.Resources.NOT_OK;
            }
            NextQuestion();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Choice(button1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Choice(button2.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Choice(button3.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Choice(button6.Text);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Choice(button7.Text);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            NextQuestion();
        }


    }
}
