using System;
using System.IO;
using System.Windows.Forms;

namespace MO32_1_Daniil.NeuroNet
{
    class InputLayer
    {
        // Поля
        private double[,] trainset; // Матрица для хранения значений тренировочных данных
        private double[,] testset;  // Матрица для хранения данных файла теста (10 строк, по 1 на цифру)
        // Свойства
        public double[,] Trainset { get => trainset; }
        public double[,] Testset { get => testset; }
        // Конструктор
        public InputLayer(NetworkMod nm)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string[] tmpArrStr; // Массив строк для хранения всех строк из файла (тренировочного или файла теста)
            string[] tmpStr;    // Массив, в который считываются поэлементно каждая строка в цикле
            // Запускаем нейросеть в том режиме, в котором мы ее запустили (какую кнопку нажали)
            switch (nm)
            {
                case NetworkMod.Train:  // Считываем данные (память), на которых мы собираемся обучить нейросеть
                    tmpArrStr = File.ReadAllLines(path + "studying.txt");
                    //Shuffle(tmpArrStr); // Перемешивание строк в массиве
                    trainset = new double[tmpArrStr.Length, 16];//Инициализируем матрицу цифр и их 15 пикселей
                    // Цикл считывания данных с файла в матрицу путем парсинга и преобразования типов
                    for (int i = 0; i < tmpArrStr.Length; i++)
                    {
                        tmpStr = tmpArrStr[i].Split(' ');
                        for (int j = 0; j < 16; j++)
                            trainset[i, j] = double.Parse(tmpStr[j]);
                    }
                    break;
                // Пока не вызывается
                case NetworkMod.Test:  // Считываем примеры, на которых мы хотим проверить работу нашей нейросети (работает таким же образом)
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
        // Перемешивание строк, чтобы обучение не происходило через одинаковую послежовательность строк
        //[ThreadStatic] private static Random _rng; // Если через потоки вызывается InputLayer
        //private static Random Rng => _rng ?? (_rng = new Random());
        private static readonly Random rng = new Random();
        public void Shuffle(string[] tmpArrStr)
        {   // Вариант с перемешиванием сразу при получении из файла (сейчас не используется)
            if (tmpArrStr == null) throw new ArgumentNullException(nameof(tmpArrStr));

            for (int i = tmpArrStr.Length - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                string temp = tmpArrStr[i];
                tmpArrStr[i] = tmpArrStr[j];
                tmpArrStr[j] = temp;
            }
        }
        public void Shuffle(double[,] tmpArr)
        {   // Вариант с перемешиванием при запуске функции тренировки
            if (tmpArr == null) throw new ArgumentNullException(nameof(tmpArr));
            int row_length = tmpArr.GetLength(0);
            int col_length = tmpArr.GetLength(1);
            for (int i = row_length - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                if (j == i) continue;
                for (int l = 0; l < col_length; l++)
                {
                    double temp = tmpArr[i, l];
                    tmpArr[i, l] = tmpArr[j, l];
                    tmpArr[j, l] = temp;
                }
            }
        }
    }
}
