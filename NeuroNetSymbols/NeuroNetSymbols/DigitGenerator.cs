using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroNetSymbols
{
    class DigitGenerator
    {
        const int weight = 4;
        const int height = 3;
        //написано для облегчения генерации обучающего символа
        public static int[,] generateSymbol(int SymbolNum, int Noise)
        {
            if (SymbolNum == 0)
            {
                return generateSymbol0(Noise);
            }
            else if (SymbolNum == 1)
            {
                return generateSymbol1(Noise);
            }
            else if (SymbolNum == 2)
            {
                return generateSymbol2(Noise);
            }
            else if (SymbolNum == 3)
            {
                return generateSymbol3(Noise);
            }  
            else //if (SymbolNum == 4)
            {
                return generateSymbol4(Noise);
            }

        }

        private static void AddNoise(int[,] symbol,int noise)
        {
            Random random = new Random();
            while (noise > 0)
            {
                int i = random.Next(height);
                int j = random.Next(weight);
                if (symbol[i, j] == 0)
                    symbol[i, j] = 1;
                else
                    symbol[i, j] = 0;
                noise--;
            }
        }
        public static int[,] generateSymbol0(int addNoise = 0)
        {
            int[,] Symbol = new int[height,weight]
            {
               { 1,0,0,1},
                { 1,1,1,1},
                { 1,0,0,1}
            };
            AddNoise(Symbol,addNoise);
            return Symbol;

        }
        public static int[,] generateSymbol1(int addNoise = 0)
        {
            int[,] Symbol = new int[height, weight]
           {
                { 0,1,1,0},
                { 1,0,0,1},
                { 0,1,1,0}
           };
            AddNoise(Symbol, addNoise);
            return Symbol;

        }
        public static int[,] generateSymbol2(int addNoise = 0)
        {
            int[,] Symbol = new int[height, weight]
          {
               { 1,0,0,1},
                { 1,1,0,1},
                { 1,0,0,1}
          };
            AddNoise(Symbol, addNoise);
            return Symbol;

        }



        public static int[,] generateSymbol3(int addNoise = 0)
        {
            int[,] Symbol = new int[height, weight]
           {
                { 0,1,1,0},
                { 1,1,0,1},
                { 0,1,1,0}
           };
            AddNoise(Symbol, addNoise);
            return Symbol;

        }
        public static int[,] generateSymbol4(int addNoise = 0)
        {
            int[,] Symbol = new int[height, weight]
           {
                { 1,1,1,1},
                { 1,0,0,1},
                { 1,0,0,1}
           };
            AddNoise(Symbol, addNoise);
            return Symbol;

        }

    }
}
