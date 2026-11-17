using System;
using static System.Math;

namespace MO32_1_Daniil.NeuroNet
{
    class Neuron
    {   // Класс нейрона. Представляет собой один нейрон, которых в каждом слое не меньше десятка: 70-32-10
        // Поля класса
        private NeuronType type;    // Тип нейрона (к какому типу слоя он принадлежит)
        private double[] weights;   // Массив синаптических весов нейрона (насколько важен тот или иной вход) размером предыдущий слой + 1, w[0] - порог нейрона!
        private double[] inputs;    // Сигналы (отклики), которые пришли на с предыдущего слоя, размер = предыдущий слой. Для первого скрытого - 15 пикселей
        private double output;      // Выходной сигнал нейрона, т.е. что он передает на следующий слой
        private double derivative;  // Значение производной функции активации, подсчитано при прямом проходе, используется при обучении

        // Константы для формул (пока их нет)

        // Свойства класса (получение/заполенении полей без прямого доступа)
        public double[] Weights { get => weights; set => weights = value; }
        public double[] Inputs { get => inputs; set => inputs = value; }
        public double Output { get => output; }
        public double Derivative { get => derivative; }
        
        //Конструктор
        public Neuron(double[] memoryWeights, NeuronType typeNeuron)
        {   // При создании нейрон получает от слоя свои веса (прочитанные из файла) и свой тип
            type = typeNeuron;
            weights = memoryWeights;
        }

        // Методы
        public void Activator(double[] inp)
        {   // "Что делает нейрон": Срабатывает после того, как пришли сигналы от нейронов предыдущего слоя
            inputs = inp;   // Сохраняем сигналы в поле
            double sum = weights[0];// Считываем порог (* 1)
            for (int i = 0; i < inputs.Length; i++)
            {   // Сумма равна порог + сумма (отклик * его вес)
                sum += weights[i + 1] * inputs[i];
            }
            switch (type)
            {   // Меняем выходное значение в зависмости от типа слоя
                case NeuronType.Hidden: // Скрытый слой: применяем функцию активации tanh, сохраняем значение производной для градиента
                    output = TanhSigmoidal(sum);
                    derivative = TanhDerivative(sum);
                    break;
                case NeuronType.Output: // Выходной слой: получаем положительное число для softmax (даст положительные дробные числа - вероятности) 
                    output = Exp(sum);
                    break;
                default: break;
            }
        }
        private double TanhSigmoidal(double sum)
        {   // Функция активации: tanh x = (e^x - e^-x) / (e^x + e^-x)
            return (Exp(sum) - Exp(-sum)) / (Exp(sum) + Exp(-sum));
        }
        private double TanhDerivative(double sum)
        {   // Производная функции активации: tanh x = 4 / (e^x + e^-x)^2
            return 4.0 / Pow((Exp(sum) + Exp(-sum)), 2);
        }
    }
}
