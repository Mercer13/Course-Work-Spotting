using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pyatnashki
{
    public partial class MainGame : Form
    {
        public MainGame()
        {
            InitializeComponent();
        }

        private void n4x4_Click(object sender, EventArgs e)
        {
            p4x4 p4x4 = new p4x4();
            p4x4.ShowDialog();
        }

        private void n3x3_Click(object sender, EventArgs e)
        {
            p3x3 p3x3 = new p3x3();
            p3x3.ShowDialog();
        }

        private void AskPermissionBeforeQuite(object sender, FormClosingEventArgs e)
        {
            DialogResult YesOrNO = MessageBox.Show("Ви впевнені, що хочете вийти?", "П'ятнашки", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (sender as Button != button1 && YesOrNO == DialogResult.No) e.Cancel = true;
            if (sender as Button == button1 && YesOrNO == DialogResult.Yes) Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
          AskPermissionBeforeQuite(sender, e as FormClosingEventArgs);
        }
    }
}
