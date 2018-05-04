using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace GeoQuiz
{
    public partial class GeoQuiz : Form
    {
        private Question _question;
        private User _user;
        private Game _game;

        public GeoQuiz()
        {
            InitializeComponent();
        }

        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char) Keys.Return) return;

            string country = textBox1.Text;
            Debug.WriteLine(Properties.Resources.ApiKey);

            pictureBox1.ImageLocation =
                "https://maps.googleapis.com/maps/api/staticmap?size=400x400&maptype=hybrid&center=" + country +
                "&style=feature:administrative.country|element:labels.text|visibility:off|&&size=400x400&markers=color:blue|" +
                country + "&key=" + Properties.Resources.ApiKey;
        }

        private void GeoQuiz_Load(object sender, EventArgs e)
        {
            panel1.Controls.Add(label2);
            panel1.Controls.Add(labelCorrectAnswer);
            this.Controls.Add(panel2);
            for (var i = 0; i < Controls.Count; i++)
            {
                Debug.WriteLine(Controls[i].Name + ": " + Controls[i].ToString());
                if (Controls[i].GetType() == typeof(Panel))
                    continue;
                panel1.Controls.Add(Controls[i]);
                i--;
            }

            labelCorrectAnswer.Text = "";
            panel1.Visible = false;
            panel2.Visible = true;
        }

        private void NextQuestion()
        {
            _question = new Question();

            labelScore.Text = _user.Name + "'s Score: " + _game.Correct + "/" + _game.Questions;
            int total = _game.TotalQuestions + _game.Questions;
            int correct = _game.TotalCorrect + _game.Correct;
            label2.Text = "Total: " + correct + "/" + total;
            if (!_game.Online)
                label2.Text = "OFFLINE MODE";
            _game.Questions++;

            pictureBox1.ImageLocation =
                "https://maps.googleapis.com/maps/api/staticmap?size=400x400&maptype=hybrid&center=" +
                _question.CorrectAnswer +
                "&style=feature:administrative.country|element:labels.text|visibility:off|&&size=400x400&markers=color:blue|" +
                _question.CorrectAnswer + " &key=" + Properties.Resources.ApiKey;

            Button[] buttons = {button1, button2, button3, button6, button7, button8};
            for (var i = 0; i < _question.Choices.Length; i++)
            {
                buttons[i].Text = _question.Choices[i];
            }
        }

        private void Choice(string choice)
        {
            labelCorrectAnswer.Text = "";
            if (_question.CorrectAnswer == choice)
            {
                pictureBox2.Image = Properties.Resources.OK;
                _game.Correct++;
            }
            else
            {
                pictureBox2.Image = Properties.Resources.NOT_OK;
                labelCorrectAnswer.Text = _question.CorrectAnswer;
            }
           
            NextQuestion();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Choice(button1.Text);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Choice(button2.Text);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Choice(button3.Text);
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            Choice(button6.Text);
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            Choice(button7.Text);
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            Choice(button8.Text);
        }

        private void TextBoxPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
                Login();
        }

        private void GeoQuiz_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_game == null)
                return;
            if (!_game.Online)
                return;

            _game.Questions--;
            Database.CreateGame(_user.Name, _game.Questions, _game.Correct);
            Debug.WriteLine("");
            Debug.WriteLine(Database.HallOfFame());
            Debug.WriteLine("ende");
        }

        private void ButtonLogin_Click(object sender, EventArgs e)
        {
            Login();
        }

        private void Login()
        {
            var online = true;

            if (TextBoxUsername.Text == "")
            {
                labelLogin.Text = "Username cannot be empty";
                return;
            }

            if(TextBoxPassword.Text == "")
            {
                labelLogin.Text = "Password cannot be empty";
                return;
            }

            try
            {
                _user = Database.SearchByUsername(TextBoxUsername.Text) ?? null;
            }
            catch (SqlException)
            {
                online = false;
                _user = new User
                {
                    Name = TextBoxUsername.Text
                };
            }

            if (_user == null)
            {
                labelLogin.Text = "user '" + TextBoxUsername.Text + "' not found";
                TextBoxUsername.Clear();
                TextBoxPassword.Clear();
                TextBoxUsername.Focus();
                return;
            }

            if (online && !_user.CheckPassword(TextBoxPassword.Text))
            {
                labelLogin.Text = "wrong password :(";
                TextBoxPassword.Text = "";
                TextBoxPassword.Focus();
                return;
            }

            StartGame(online);
        }

        private void ButtonCreateUser_Click(object sender, EventArgs e)
        {
            bool online = true;

            try
            {
                _user = Database.CreateUser(TextBoxUsername.Text, TextBoxPassword.Text) ?? null;
            }
            catch (SqlException ex)
            {
                online = false;
                Debug.WriteLine(ex.Message);
                
                _user = new User
                {
                    Name = TextBoxUsername.Text
                };
            }

            if (_user == null)
            {
                labelLogin.Text = "this user already exists";
                return;
            }

            StartGame(online);
        }

        private void StartGame(bool online)
        {
            _game = new Game(_user.Name)
            {
                Online = online
            };

            if (Regex.Match(_user.Name, @"harry", RegexOptions.IgnoreCase).Success)
            {
                _game.DebugMode = true;
                textBox1.Visible = true;
            }

            NextQuestion();

            if (!_game.Online)
            {
                toolStripMenuItem1.Visible = false;
            }

            panel1.Visible = true;
            panel2.Visible = false;
        }

  
        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Database.HallOfFame(), "Hall of Fame");
        }

        private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Guess the country you see on the map!", "Help");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Geo Quiz\nVersion 1.0, built on " + DateTime.Today.ToShortDateString() + "\nHarry Maierhofer\nharry.maierhofer@gmail.com", "About");
        }
    }
}
