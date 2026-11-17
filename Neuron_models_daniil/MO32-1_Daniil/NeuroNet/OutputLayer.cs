using System.Runtime.Remoting;

namespace MO32_1_Daniil.NeuroNet
{
    class OutputLayer : Layer
    {
        // Конструктор выходного слоя вызывает конструктор родителя и всё (не нужен дополнительный функционал)
        public OutputLayer(int non, int nopn, NeuronType nt, string type) : base(non, nopn, nt, type) { }

        // Прямой проход: у нас есть выходы, преобразуем их по правилу SoftMax (отклики уже представляют собой e^num)
        public override void Recognize(Network net, Layer nextLayer)
        {   // Получает на вход всю нейросеть, а также следующий слой (отсутствует, так что null)
            double e_sum = 0;
            for (int i = 0; i < neurons.Length; i++) 
                e_sum += neurons[i].Output;
            for (int i = 0; i < neurons.Length; i++) // Получение "вероятностей" всех цифр
                net.Fact[i] = neurons[i].Output / e_sum;
        }
        // Обратный проход: то же самое, что и для скрытого слоя, только не нужно хранить значение производной
        public override double[] BackwardPass(double[] errors)
        {
            // Переменная для сохранения ошибок предыдущего слоя (в последствии вычисляется градиент), ввозвращается функцией
            double[] gr_sum = new double[numofprevneurons];
            // Вычисление ошибок
            for (int j = 0; j < numofprevneurons; j++)
            {
                double sum = 0;
                for (int k = 0; k < numofneurons; k++)
                    sum += neurons[k].Weights[j + 1] * errors[k];
                gr_sum[j] = sum;
            }
            // После получения ошибок для предыдущего слоя корректирует веса (оценки) нейронов текущего слоя 
            for (int i = 0; i < numofneurons; i++)
                for (int n = 0; n < numofprevneurons + 1; n++)
                {
                    double deltaw;
                    if (n == 0) // Порог: поправка = инерция * предыдущее обучение + скорость обучения * ошибка
                        deltaw = momentum * lastdeltaweights[i, 0] + learningrate * errors[i];
                    else        // Веса: поправка =  инерция * предыдущее обучение + скорость обучения * входной сигнал (отклик) * ошибка 
                        deltaw = momentum * lastdeltaweights[i, n] + learningrate * neurons[i].Inputs[n - 1] * errors[i];

                    lastdeltaweights[i, n] = deltaw;
                    neurons[i].Weights[n] += deltaw;
                }
            //
            return gr_sum;
        }
    }
}
