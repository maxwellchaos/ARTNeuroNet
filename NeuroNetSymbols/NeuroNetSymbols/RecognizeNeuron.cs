using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroNetSymbols
{
    class RecognizeNeuron
    {
        //вход
        public double input;
        //выход нейрона для старта
        //после старат будет использоваться GetResult()
        public int output;

        //отключен ли нейрон
        public bool reset = false;

        //вычисляет выход нейрона и возвращает этот выход
        //threshold - порог активации
        public int GetResult(double threshold)
        {
            if (input > threshold)
                output = 1;
            else
                output = 0;
            return output;
        }
    }
}
