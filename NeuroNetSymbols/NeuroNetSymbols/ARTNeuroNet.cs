using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroNetSymbols
{
    class ARTNeuroNet
    {
        //параметр сходства
        double p = 0.8;

        double L = 2;

        //пр1 и пр2
        int G1;
        int G2;
        //слои нейронов
        List<CompareNeuron> compareLayer = new List<CompareNeuron>();
        List<RecognizeNeuron> recognizeLayer = new List<RecognizeNeuron>();
        //веса связей
        //это двумерный массив(матрица), сделанный как список списков
        //так проще добавлять новые нейроны
        //от распознающего к сравнивающему
        //[номер распознающего][номер сравнивающего]
        List<List<int>> recognizeToCompare = new List<List<int>>();

        //[номер сравнивающего][номер распознающего]
        //от сравнивающего к распознающему
        List<List<double>> compareToRecognize = new List<List<double>>();

        //начальное количество нейронов
        //в сравнивающем слое - по количеству входов
        int compareNeuronsCount;
        //в выходном слое по количеству распознанных образцов
        //начнем с одного нейрона
        int recognizeNeuronCount;

      
        //добавить новый обученный нейрон
        void AddNeuron(int[] symbol)
        {
            recognizeNeuronCount++;

            //добавить нейрон в список
            recognizeLayer.Add(new RecognizeNeuron());

            //добавить веса нейрона
            recognizeToCompare.Add(new List<int>());
            //инициализация весов распознающего
            for (int i = 0; i < compareNeuronsCount; i++)
            {
                recognizeToCompare[recognizeNeuronCount-1].Add(symbol[i]);
            }

            int inputSum = symbol.Sum();//считаю сумму всех элементов символа(сколько 1 в символе)

            //инициализация весов сравнивающего
            for (int i = 0; i < compareNeuronsCount; i++)
            {
                compareToRecognize.Add(new List<double>());
                for (int j = 0; j < recognizeNeuronCount; j++)
                {
                    compareToRecognize[i].Add((recognizeToCompare[0][i] * L) / (L - 1 + inputSum));
                }
            }
        }

        public int AddSample(int[] symbol)
        {
            //это обработка ошибок
            if (symbol.Length != compareNeuronsCount)
                throw new Exception("Вход не той длинны");

            //если нейрон один
            if(recognizeNeuronCount == 1)
            {
                // обучить его 
                
                // добавить новый нейрон
                AddNeuron(symbol);
                return 0;//Возвращаю номер нулевого нейрона
            }
            else
            {
                //подать данные на сравнение
                for (int i = 0; i < compareNeuronsCount; i++)
                {
                    compareLayer[i].x = symbol[i];
                    compareLayer[i].g = 1;//на входе что-то есть (G1=1)
                    compareLayer[i].p = 0;//сигнал со слоя распознавания при старте = 0
                }
                //передать данные на распознавание
                //суммирование произведения выходов слоя сравнения на веса(свертка)
                for (int i = 0; i < recognizeNeuronCount; i++)
                {
                    recognizeLayer[i].input = 0;
                    for (int j = 0; j < compareNeuronsCount; j++)
                    {
                        //GetResult() - вычисляет значение нейрона по правилу 2 из 3
                        recognizeLayer[i].input += compareLayer[i].GetResult() * compareToRecognize[j][i];
                    }
                }
                //ищем максимум из всех выходов распознающего слоя
                int maxNum = 0;
                for (int i = 0; i < recognizeNeuronCount; i++)
                {
                    if (recognizeLayer[i].input > recognizeLayer[maxNum].input)
                        maxNum = i;
                }

                if(maxNum != 0)// если maxNum==0 значит победил необученный нейрон
                {
                    //проверить на p
                    //передаю данные из выхода слоя распознавания на слой сравнения
                    for (int i = 0; i < compareNeuronsCount; i++)
                    {
                        //заполнить вход для обратной связи сравнивающего
                        compareLayer[i].p = recognizeLayer[maxNum].output * recognizeToCompare[maxNum][i];
                        
                        compareLayer[i].g = 0; //G1=0
                    }

                    //сравнить вход и выход сравнивающего слоя
                    int l = 0;//Кол-во совпавших компонент входа и выхода слоя сравнения
                    for (int i = 0; i < compareNeuronsCount; i++)
                    {
                        //считаю выход нейрона сравнения и сравниваю с обратной связью от слоя распознавания
                        if (compareLayer[i].p == compareLayer[i].GetResult())
                        {
                            l++;
                        }
                    }

                    //проверяю условие соответствия
                    if (p > l / compareNeuronsCount)
                    {
                        //если не подходит - добавить новый
                        AddNeuron(symbol);
                        //и вернуть его номер
                        return recognizeNeuronCount;
                    }
                    else
                    {
                        //доучить

                        //Классификация завершена, вернуть образец 
                        return maxNum;
                    }
                   
                }
                else
                {
                    //если не распознали  добавить новый обученный нейрон
                    AddNeuron(symbol);
                    //и вернуть его номер
                    return recognizeNeuronCount;
                }
            }
        }

        //создание нейросети заданного размера
        //по умолчанию - размер 12 элементов
        //сразу добавляю один распознающий нейрон
        public void CreateNet(int inputSize = 12)
        {
            //изменить количество нейронов
            compareNeuronsCount = inputSize;

            //инициализация весов распознающего
            //предыдущие данные, если они были, исчезнут
            recognizeToCompare.Add(new List<int>());
            for (int i = 0; i < compareNeuronsCount; i++)
            {
                recognizeToCompare[0].Add(1);
            }

            //инициализация весов сравнивающего
            for (int i = 0; i < compareNeuronsCount; i++)
            {
                compareToRecognize.Add(new List<double>());
                compareToRecognize[i].Add(L / (L - 1 + compareNeuronsCount));
            }
            //создаю сами нейроны сравнивающего слоя
            for (int i = 0; i < compareNeuronsCount; i++)
            {
                compareLayer.Add(new CompareNeuron());
            }
            int[] neuron = new int[compareNeuronsCount];
            for (int i = 0; i < compareNeuronsCount; i++)
            {
                neuron[i] = 1;
            }
            AddNeuron(neuron);
        }

        ////распознавание
        ////на вход подается массив из целых чисел
        //void recognize(List<int> input)
        //{
            //Подать на слоя сравнения параметры
            //G2 = 0;
            //for (int i = 0; i < compareNeuronsCount; i++)
            //{
            //    //заношу данные из входа на нейроны сравнения
            //    compareLayer[i].x = input[i];

            //    //если хоть один вход == 1 то G2 = 1
            //    if (input[i] == 1)
            //        G2 = 1;
                
            //    //добавить пр1 к нейронам сравнения
            //    compareLayer[i].g = G1;
            //}

            ////суммирование произведения выходов слоя сравнения на веса(свертка)
            //for (int i = 0; i < recognizeNeuronCount; i++)
            //{
            //    recognizeLayer[i].input = 0;
            //    for (int j = 0; j < compareNeuronsCount; j++)
            //    {
            //        recognizeLayer[i].input += compareLayer[i].GetResult() * compareToRecognize[j][i];
            //    }
            //}
           
            ////ищем максимум из всех выходов распознающего слоя
            //int MaxNum = 0;
            //for (int i = 0; i < recognizeNeuronCount; i++)
            //{
            //    if (recognizeLayer[i].input> recognizeLayer[MaxNum].input)
            //        MaxNum = i;
            //}
            ////вычисление выхода распознающего слоя
            //for (int i = 0; i < recognizeNeuronCount; i++)
            //{
            //    if (i == MaxNum)
            //        recognizeLayer[i].input = 1;
            //    else
            //        recognizeLayer[i].input = 0;
            //}

        //    //передаю данные из выхода слоя распознавания на слой сравнения
        //    for (int i = 0; i < compareNeuronsCount; i++)
        //    {
        //        compareLayer[i].p = recognizeLayer[MaxNum].output*recognizeToCompare[MaxNum][i];
        //    }
            
            
        //    G1 = 0;///////////////дописать метод переключения этого пр1
        //    //Установить G1 у всех нейронов сравнения
        //    for (int i = 0; i < compareNeuronsCount; i++)
        //    {
        //        //добавить пр1 к нейронам сравнения
        //        compareLayer[i].g = G1;
        //    }

        //    //блок сброса
        //    //сравнить вход и выход сравнивающего слоя
        //    int l = 0;//Кол-во совпавших компонент входа и выхода слоя сравнения
        //    for (int i = 0; i < compareNeuronsCount; i++)
        //    {
        //        //считаю выход нейрона сравнения и сравниваю с обратной связью от слоя распознавания
        //        if(compareLayer[i].p==compareLayer[i].GetResult())
        //        {
        //            l++;
        //        }
        //    }
        //    //проверяю условие сброса
        //    if(p>l/compareNeuronsCount)
        //    {
        //        //выработать сигнал сброса
        //    }
        //    else
        //    {
        //        //Классификация завершена, образец 
        //        return;
        //    }

        //}


        ////создаю матрицы весов и устанавливаю стартовые параметры
        //void init()
        //{
        //    //инициализация весов распознающего
        //    recognizeToCompare.Add(new List<int>());
        //    for (int i = 0; i < compareNeuronsCount; i++)
        //    {
        //        recognizeToCompare[0].Add(1);
        //    }

        //    //инициализация весов сравнивающего
        //    for (int i = 0; i < compareNeuronsCount; i++)
        //    {
        //        compareToRecognize.Add(new List<double>());
        //        compareToRecognize[i].Add(L / (L - 1 + compareNeuronsCount));
        //    }

        //    //параметры
        //    G2 = 0;
        //    G1 = 1;
        //    p = 0.85;

        //    //распознающий слой
        //    recognizeLayer.Add(new RecognizeNeuron());
        //    //и его выходы
        //    recognizeLayer[0].output = 0;

        //    //сравнивающий слой
        //    for (int i = 0; i < compareNeuronsCount; i++)
        //    {
        //        compareLayer.Add(new CompareNeuron());
        //    }

        //}

    }
}
