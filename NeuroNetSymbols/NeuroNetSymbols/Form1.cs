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
        //это параметры образца. Если их поменять - будет образец другого размера
        //константами сделаны чтобы можно было их указать как размер массива
        //длина образца
        const int SymbolLength = 3;
        //высота образца
        const int SymbolHeight = 4;
        
        //сама нейросеть
        ARTNeuroNet art = new ARTNeuroNet();

        int[,] symbol = new int[SymbolLength, SymbolHeight];

        public Form1()
        {
            InitializeComponent();
            art.CreateNet(SymbolLength * SymbolHeight);
        }


        //преобразует символ в строку
        int[] SymbolToInput(int[,] symbol)
        {
            int[] result = new int[SymbolLength * SymbolHeight];
            int resultIterator = 0;
            for (int i = 0; i < SymbolLength; i++)
                for (int j = 0; j < SymbolHeight; j++)
                {
                    result[resultIterator] = symbol[i, j];
                    resultIterator++;
                }
                    return result;
        }


        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            int width = 10;
            int height = 20;
            for (int i = 0; i < SymbolLength; i++)
            {
                for (int j = 0; j < SymbolHeight; j++)
                {
                    if (symbol[i, j] == 1)
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
            symbol = DigitGenerator.generateSymbol3();
            GetResult();
            pictureBox1.Invalidate();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            symbol = DigitGenerator.generateSymbol2();
            GetResult();
            pictureBox1.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            symbol = DigitGenerator.generateSymbol1();
            GetResult();
            pictureBox1.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            symbol = DigitGenerator.generateSymbol0();
            GetResult();
            pictureBox1.Invalidate();
        }

        private void button5_Click(object sender, EventArgs e)
        {
          
        }

        void GetResult()
        {
            //дать сети образец, получить результат, вывести результат в label4
            label4.Text = art.AddSample(SymbolToInput(symbol)).ToString();
            double[] inputOfActiveNeuron = new double[SymbolLength * SymbolHeight];
            int[] outputOfActiveNeuron = new int[SymbolLength * SymbolHeight];

            dataGridView1.Rows.Clear();
            for (int i = 0; i < art.recognizeNeuronCount; i++)
            {
                //массив для показа в таблице одной строки
                string[] strings = new string[13];
                strings[0] = i.ToString() + " in";
                for (int j = 0; j < art.compareNeuronsCount; j++)
                {
                    strings[j + 1] = art.compareToRecognize[j][i].ToString();
                }
                dataGridView1.Rows.Add(strings);

                strings = new string[13];
                strings[0] =  "out";
                for (int j = 0; j < art.compareNeuronsCount; j++)
                {
                    strings[j + 1] = art.recognizeToCompare[i][j].ToString();
                }
                dataGridView1.Rows.Add(strings);
            }
            

        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            symbol = DigitGenerator.generateSymbol4();
            GetResult();
            pictureBox1.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Уменьшаю ширину колонок
            //кодом проще, чем в ручную
            for(int i = 0;i<dataGridView1.Columns.Count;i++)
                dataGridView1.Columns[i].Width = 40;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
