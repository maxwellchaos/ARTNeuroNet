using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroNetSymbols
{
    class CompareNeuron
    {
        //сигнал от слоя распознавания
        public int p = 0;

        //входной сигнал
        public int x = 0;
        //общий сигнал
        public int g = 0;
        public int GetResult()
        {
            //если больше чем на одном входе 1
            if (p + x + g > 1)
                return 1;//вернуть 1

            //В остальных случаях
            return 0;

        }
    }
}
