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
        double p;


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
        List<List<double>> recognizeToCompare = new List<List<double>>();

        //[номер сравнивающего][номер распознающего]
        //от сравнивающего к распознающему
        List<List<double>> compareToRecognize = new List<List<double>>();

        //начальное количество нейронов
        //в сравнивающем слое - по количеству входов
        int compareNeuronsCount = 12;
        //в выходном слое по количеству распознанных образцов
        //начнем с одного нейрона
        int recognizeNeuronCount = 1;

        //это констркутор класса  
        public ARTNeuroNet()
        {
            init()


        }

        //создаю матрицы весов и устанавливаю стартовые параметры
        void init()
        {
            //инициализация весов распознающего
            recognizeToCompare.Add(new List<double>());
            for (int i = 0; i < compareNeuronsCount; i++)
            {
                recognizeToCompare[0].Add(L / (L - 1 + compareNeuronsCount));
            }

            //инициализация весов сравнивающего
            for (int i = 0; i < compareNeuronsCount; i++)
            {
                compareToRecognize.Add(new List<double>());
                compareToRecognize[i].Add(1);
            }

            //параметры
            G2 = 0;
            G1 = 1;
            p = 0.85;

            //распознающий слой
            recognizeLayer.Add(new RecognizeNeuron());
            //и его выходы
            recognizeLayer[0].Out = 0;

            //сравнивающий слой
            for (int i = 0; i < compareNeuronsCount; i++)
            {
                compareLayer.Add(new CompareNeuron());
            }
           
        }

        //распознавание
        //на вход подается массив из целых чисел
        void recognize(List<int> input)
        {
            //Подать на вход параметры
            G2 = 0;
            for (int i = 0; i < compareNeuronsCount; i++)
            {
                //заношу данные из входа на нейроны сравнения
                compareLayer[i].x = input[i];

                //если хоть один вход == 1 то G2 = 1
                if (input[i] == 1)
                    G2 = 1;

                //добавить пр1 к нейронам сравнения
                compareLayer[i].g = G1;
            }

            //суммирование произведения выходов слоя сравнения на веса(свертка)
            for (int i = 0; i < recognizeNeuronCount; i++)
            {
                recognizeLayer[i].Out = 0;
                for (int j = 0; j < compareNeuronsCount; j++)
                {
                    recognizeLayer[i] += compareLayer[i].GetResult()*;
            }
        }
    }
}
