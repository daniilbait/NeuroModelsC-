using System;
using System.IO;

namespace MO32_1_Daniil.NeuroNet
{
    class InputLayer
    {
        // Поля
        private double[,] trainset;
        private double[,] testset;
        // Свойства
        public double[,] Trainset { get => trainset; }
        public double[,] Testset { get => testset; }
        // Конструктор
        public InputLayer(NetworkMod nm)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string[] tmpArrStr; // Считываем все строки файла сюда
            string[] tmpStr;    //

            switch (nm)
            {
                case NetworkMod.Train:
                    tmpArrStr = File.ReadAllLines(path + "studying.txt");
                    Shuffle(tmpArrStr);
                    trainset = new double[tmpArrStr.Length, 16];
                    for (int i = 0; i < tmpArrStr.Length; i++)
                    {
                        tmpStr = tmpArrStr[i].Split(' ');
                        for (int j = 0; j < 16; j++)
                            trainset[i, j] = double.Parse(tmpStr[j]);
                    }
                    break;

                case NetworkMod.Test:
                    tmpArrStr = File.ReadAllLines(path + "test.txt");
                    Shuffle(tmpArrStr);
                    testset = new double[tmpArrStr.Length, 16];
                    for (int i = 0; i < tmpArrStr.Length; i++)
                    {
                        tmpStr = tmpArrStr[i].Split(' ');
                        for (int j = 0; j < 16; j++)
                            testset[i, j] = double.Parse(tmpStr[j]);
                    }
                    break;
            }
        }
        // Перемешивание строк
        //[ThreadStatic] private static Random _rng; // Если через потоки вызывается InputLayer
        //private static Random Rng => _rng ?? (_rng = new Random());
        private static readonly Random rng = new Random();
        public void Shuffle(string[] tmpArrStr)
        {
            if (tmpArrStr == null) throw new ArgumentNullException(nameof(tmpArrStr));

            for (int i = tmpArrStr.Length - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                string temp = tmpArrStr[i];
                tmpArrStr[i] = tmpArrStr[j];
                tmpArrStr[j] = temp;
            }
        }
    }
}
