using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroNetSymbols
{
    class NeuroNet
    {
        const int inputCount = 48;
        const int hiddenCount = 30;

        //Это массив входов
        double[] input = new double[inputCount];


        //Это матрица весов скрытого слоя
        //первый элемент - номер входа, второй элемент - номер выхода
        double[,] hiddenWeight = new double[inputCount, hiddenCount];

        double[] hiddenResult = new double[hiddenCount];
        double[] hiddenErr = new double[hiddenCount];


        //Это матрица весов выходного слоя
         double[,] outWeight = new double[hiddenCount, 4];

        //массив выходов
        public double[] output = new double[4];
        //здесь лежат ошибки выхода
        double[] outErr = new double[4];

        //здесь лежат правильные ответы
        //нужно для обучения
        double[] RightAnswer = new double[4];
      

        //коэффициент обучения
        double LernCoeff;

        //Функция активации
        //сигмоида
        double activate(double x, double alpha = 1)
        {
            return 1 / (1 + Math.Exp(-x*alpha));
        }

        //заставлю нейросеть работать
        //предполагаю, что вcе входы уже заполнены
        public void work()
        {
            //пройтись по всем скрытым нейронам
            for (int i = 0; i < hiddenCount; i++)
            {
                //обнулить 
                hiddenResult[i] = 0;
                for (int j = 0; j < inputCount; j++)
                {
                    hiddenResult[i] = hiddenResult[i] + input[j] * hiddenWeight[j, i];
                }
                //применяю функцию активации
                hiddenResult[i] = activate(hiddenResult[i]);
            }
            //пройтись по всем выходам
            for (int i = 0; i < 4; i++)
            {
                //обнулить 
                output[i] = 0;
                for (int j = 0; j < 4; j++)
                {
                    output[i] = output[i] + hiddenResult[j] * outWeight[j, i];
                }
                //применяю функцию активации
                output[i] = activate(output[i],4);//4 - коэффициент наклона сигмоиды
            }
        }
        

        //установить  входные параметры
        public void setInput(int[,] Symbol)
        {
            int k = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    input[k] = Symbol[i, j];
                    k++;
                }
            }
        }

        //

        //установить желаемый результат
        void setRightAnswer(int answer)
        {
            for (int i = 0; i < 4; i++)
                RightAnswer[i] = 0;
            RightAnswer[answer] = 1;
        }

        //вычисляю ошибку работы сети
        //возвращает среднюю ошибку
        double calcErr()
        {
            //общая ошибка
            double Err = 0;
            //ошибка выходного слоя
            for (int i = 0; i < 4; i++)
            {
                outErr[i] = output[i] - RightAnswer[i];
                Err += Math.Abs(outErr[i]);//Прибавляю модуль ошибки
            }
            //ошибка скрытого слоя
            for (int i = 0; i < hiddenCount; i++)
            {
                hiddenErr[i] = 0;
                for (int j = 0; j < 4; j++)
                {
                    hiddenErr[i] += outErr[j] * outWeight[i, j] * hiddenResult[i] * (1 - hiddenResult[i]);
                }
            }
            return Err / 4;
        }

        //обнеовляю веса сети
        void WeightUpdate()
        {
            //обновление весов последнего слоя
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < hiddenCount; j++)
                {
                    //на сколько нужно изменить вес
                    double deltaWeight = LernCoeff * outErr[i] * hiddenResult[j];
                    outWeight[j, i] = outWeight[j, i] - deltaWeight;
                }
            }
            //обновление весов ведущих в скрытый слой
            for (int i = 0; i < hiddenCount; i++)
            {
                for (int j = 0; j < inputCount; j++)
                {
                    //на сколько нужно изменить вес
                    double deltaWeight = LernCoeff * hiddenErr[i] * input[j];
                    hiddenWeight[j, i] = hiddenWeight[j, i] - deltaWeight;
                }
            }
        }

        //заполнить веса случайными данными от -0.5 до 0.5
        public void ResetWeight()
        {
            //генератор случайных чисел
            Random rnd = new Random();
            //заполнить веса случайными данными от -0.5 до 0.5
            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j < hiddenCount; j++)
                {
                    outWeight[j, i] = 0.5 - rnd.NextDouble();
                }
            }
            //и то же самое в скрытом слое
            for (int i = 0; i < hiddenCount; i++)
            {
                for (int j = 1; j < inputCount; j++)
                {
                    hiddenWeight[j, i] = 0.5 - rnd.NextDouble();
                }
            }
        }

        //test
        public double Test()
        {
            double Err = 0;
            //общий цикл c шумом в 1 - инвертирую один случайный пиксель символа
            for (int i = 0; i < 4; i++)
            {
                //Сгенерировать пример и занести его в нейросеть
                setInput(DigitGenerator.generateSymbol(i, 1));
                setRightAnswer(i);

                //распознать
                work();

                //подсчитать ошибку
                Err += calcErr();
            }
            return Err / 4;
        }

        //обучение
        //возвращает среднюю ошибку
        public double Lern(double coeff)
        {
            double Err = 0;
            LernCoeff = coeff;
            Random rnd = new Random();

            //общий цикл обучения
            for (int i = 0; i < 4; i++)
            {
                //Сгенерировать пример и занести его в нейросеть
                setInput(DigitGenerator.generateSymbol(i,0));
                setRightAnswer(i);

                //распознать
                work();

                //подсчитать ошибку
                Err += calcErr();

                //изменить веса сети
                WeightUpdate();
            }
            return Err / 4;
        }
        //взять результат
        //просто взять номер выхода с самым большим значением
        public int getResult()
        {
            int num = 0;
            double max = output[0];
            for(int i = 0;i<4;i++)
            {
                if(output[i]>max)
                {
                    max = output[i];
                    num = i;
                }

            }
            return num;
            
        }
    }
}