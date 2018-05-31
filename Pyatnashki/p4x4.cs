using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Pyatnashki
{
    public partial class p4x4 : Form
    {
        int inNullSliceIndex, inmoves = 0;
        List<Bitmap> PictureList = new List<Bitmap>();
        System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();

        public p4x4()
        {
            InitializeComponent();
            PictureList.AddRange(new Bitmap[] { Properties.Resources.p1, Properties.Resources.p2, Properties.Resources.p3,
                Properties.Resources.p4, Properties.Resources.p5, Properties.Resources.p6, Properties.Resources.p7, Properties.Resources.p8,
                Properties.Resources.p9, Properties.Resources.p10, Properties.Resources.p11, Properties.Resources.p12, Properties.Resources.p13,
                Properties.Resources.p14, Properties.Resources.p15, Properties.Resources.p16, Properties.Resources._null });
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
                List<int> Indexes = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 16 });//15 немає - оскільки це останній шматочок.
                Random r = new Random();
                for (int i = 0; i < 16; i++)
                {
                    Indexes.Remove((j = Indexes[r.Next(0, Indexes.Count)]));
                    ((PictureBox)GroupPuzzleBox.Controls[i]).Image = PictureList[j];
                    if (j == 16) inNullSliceIndex = i;//зберігати порожній індекс папки картинки
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

            int PictureBoxIndex = GroupPuzzleBox.Controls.IndexOf(sender as Control);
            if (inNullSliceIndex != PictureBoxIndex)
            {
                List<int> FourBrothers = new List<int>(new int[] { ((PictureBoxIndex % 4 == 0) ? -1 : PictureBoxIndex - 1), PictureBoxIndex - 4, (PictureBoxIndex % 3 == 3) ? -1 : PictureBoxIndex + 1, PictureBoxIndex + 4 });
                if (FourBrothers.Contains(inNullSliceIndex))
                {
                    ((PictureBox)GroupPuzzleBox.Controls[inNullSliceIndex]).Image = ((PictureBox)GroupPuzzleBox.Controls[PictureBoxIndex]).Image;
                    ((PictureBox)GroupPuzzleBox.Controls[PictureBoxIndex]).Image = PictureList[16];
                    inNullSliceIndex = PictureBoxIndex;
                    Moves.Text = "Ходів : " + (++inmoves);
                    if (CheckWin())
                    {
                        timer.Stop();
                        (GroupPuzzleBox.Controls[15] as PictureBox).Image = PictureList[15];
                        MessageBox.Show("Вітаю!!!...\nВи виграли!!!\nЧас минув : " + timer.Elapsed.ToString().Remove(15) + "\nВсього ходів : " + inmoves, "П'ятнашки");
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
            for (i = 0; i < 15; i++)
            {
                if ((GroupPuzzleBox.Controls[i] as PictureBox).Image != PictureList[i]) break;
            }
            if (i == 15) return true;
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
            if (timer.Elapsed.Minutes.ToString() == "15") //час гри
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
