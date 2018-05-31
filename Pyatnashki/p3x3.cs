using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Pyatnashki
{
    public partial class p3x3 : Form
    {
        int inNullSliceIndex, inmoves = 0;
        List<Bitmap> PictureList = new List<Bitmap>();
        System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();

        public p3x3()
        {
            InitializeComponent();
            PictureList.AddRange(new Bitmap[] { Properties.Resources.q1, Properties.Resources.q2, Properties.Resources.q3,
                Properties.Resources.q4, Properties.Resources.q5, Properties.Resources.q6, Properties.Resources.q7,
                Properties.Resources.q8, Properties.Resources.q9, Properties.Resources._null });
            Moves.Text += inmoves;
            TimeElapsed.Text = "00:00:00";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Shuffle();
        }

        void Shuffle()
        {
            do
            {
                int j;
                List<int> Indexes = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 9 });//8 немає - оскільки це останній шматочок.
                Random r = new Random();
                for (int i = 0; i < 9; i++)
                {
                    Indexes.Remove((j = Indexes[r.Next(0, Indexes.Count)]));
                    ((PictureBox)GroupPuzzleBox.Controls[i]).Image = PictureList[j];
                    if (j == 9) inNullSliceIndex = i;//зберігати порожній індекс папки картинки
                }
            } while (CheckWin());
        }

        private void ButtonShuffle_Click(object sender, EventArgs e)
        {
            DialogResult YesOrNo = new DialogResult();
            if (TimeElapsed.Text != "00:00:00")
            {
                YesOrNo = MessageBox.Show("Ви впевнені, що хочете перезапустити гру?", "П'ятнашки", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            if (YesOrNo == DialogResult.Yes || TimeElapsed.Text == "00:00:00")
            {
                Shuffle();
                timer.Reset();
                TimeElapsed.Text = "00:00:00";
                inmoves = 0;
                Moves.Text = "Ходів: 0";
            }
        }

        private void ButtonQuit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SwitchPictureBox(object sender, EventArgs e)
        {
            if (TimeElapsed.Text == "00:00:00")
                timer.Start();

            int inPictureBoxIndex = GroupPuzzleBox.Controls.IndexOf(sender as Control);
            if (inNullSliceIndex != inPictureBoxIndex)
            {
                List<int> Neighbours = new List<int>(new int[] { ((inPictureBoxIndex % 3 == 0) ? -1 : inPictureBoxIndex - 1), inPictureBoxIndex - 3, (inPictureBoxIndex % 3 == 2) ? -1 : inPictureBoxIndex + 1, inPictureBoxIndex + 3 });
                if (Neighbours.Contains(inNullSliceIndex))
                {
                    ((PictureBox)GroupPuzzleBox.Controls[inNullSliceIndex]).Image = ((PictureBox)GroupPuzzleBox.Controls[inPictureBoxIndex]).Image;
                    ((PictureBox)GroupPuzzleBox.Controls[inPictureBoxIndex]).Image = PictureList[9];
                    inNullSliceIndex = inPictureBoxIndex;
                    Moves.Text = "Ходів : " + (++inmoves);
                    if (CheckWin())
                    {
                        timer.Stop();
                        (GroupPuzzleBox.Controls[8] as PictureBox).Image = PictureList[8];
                        MessageBox.Show("Вітаю!!!...\nВи виграли!!!\nЧас минув : " + timer.Elapsed.ToString().Remove(8) + "\nВсього ходів : " + inmoves, "П'ятнашки");
                        inmoves = 0;
                        Moves.Text = "Ходів : 0";
                        TimeElapsed.Text = "00:00:00";
                        timer.Reset();
                        Shuffle();
                    }
                }
            }
        }

        bool CheckWin() //Перевірка на перемогу
        {
            int i;
            for (i = 0; i < 8; i++)
            {
                if ((GroupPuzzleBox.Controls[i] as PictureBox).Image != PictureList[i]) break;
            }
            if (i == 8) return true;
            else return false;
        }

        private void UpdateTimeElapsed(object sender, EventArgs e)
        {
            if (timer.Elapsed.ToString() != "00:00:00")
                TimeElapsed.Text = timer.Elapsed.ToString().Remove(8);
            if (timer.Elapsed.ToString() == "00:00:00")
                ButtonPause.Enabled = false;
            else
                ButtonPause.Enabled = true;
            if (timer.Elapsed.Minutes.ToString() == "5") //час гри
            {
                timer.Reset();
                Moves.Text = "Кроків : 0";
                TimeElapsed.Text = "00:00:00";
                inmoves = 0;
                ButtonPause.Enabled = false;
                MessageBox.Show("Час вийшов\nСпробуйте ще", "П'ятнашки");
                Shuffle();
            }
        }

        private void PauseOrResume(object sender, EventArgs e)
        {
            if (ButtonPause.Text == "Пауза")
            {
                timer.Stop();
                GroupPuzzleBox.Visible = false;
                ButtonPause.Text = "Продовжити";
            }
            else
            {
                timer.Start();
                GroupPuzzleBox.Visible = true;
                ButtonPause.Text = "Пауза";
            }
        }
    }


}
