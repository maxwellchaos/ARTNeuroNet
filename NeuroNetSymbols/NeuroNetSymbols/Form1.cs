using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuroNetSymbols
{
    public partial class Form1 : Form
    {
        NeuroNet net = new NeuroNet();
        int[,] Symbol = new int[8, 6];

        double[] testErr = new double[500];

        double[] lernErr = new double[500];
        public Form1()
        {
            InitializeComponent();
        }



        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            int width = 10;
            int height = 20;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (Symbol[i, j] == 1)
                    {
                        e.Graphics.FillRectangle(Brushes.Black, j * width, i * height, width, height);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(Brushes.White, j * width, i * height, width, height);
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Symbol = DigitGenerator.generateSymbol3((int)numericUpDown1.Value);
            GetResult();
            pictureBox1.Invalidate();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Symbol = DigitGenerator.generateSymbol2((int)numericUpDown1.Value);
            GetResult();
            pictureBox1.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Symbol = DigitGenerator.generateSymbol1((int)numericUpDown1.Value);
            GetResult();
            pictureBox1.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Symbol = DigitGenerator.generateSymbol0((int)numericUpDown1.Value);
            GetResult();
            pictureBox1.Invalidate();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 500; i++)
            {
                lernErr[i] = 0;
                testErr[i] = 0;
            }
            int LernCount = 500;
            net.ResetWeight();
            progressBar1.Maximum = LernCount;
            for(int i = 0;i<LernCount;i++)
            {
                lernErr[i] = net.Lern(0.1);
                testErr[i] = net.Test();
                progressBar1.Value = i;
                //обновляю графики ошибок
                pictureBox2.Invalidate();
                pictureBox3.Invalidate();
                Application.DoEvents();
            }
        }

        void GetResult()
        {
            net.setInput(Symbol);
            net.work();
            int result = net.getResult();
            if (result == 0)
                label2.Text = "2";
            if (result == 1)
                label2.Text = "3";
            if (result == 2)
                label2.Text = "Ч";
            if (result == 3)
                label2.Text = "Щ";
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            for(int i = 0;i<500;i++)
            {
                e.Graphics.FillEllipse(Brushes.Black, i, 50-(int)(lernErr[i] * 100), 2, 2);
            }
        }

        private void pictureBox3_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < 500; i++)
            {
                e.Graphics.FillEllipse(Brushes.Black, i, 50 - (int)(testErr[i] * 100), 2, 2);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ARTNeuroNet anet = new ARTNeuroNet();
        }
    }
}
