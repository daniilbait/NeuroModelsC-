using System.Windows.Forms.VisualStyles;

namespace MO32_1_Daniil.NeuroNet
{
    class Network
    {
        //Поля: все 4 слоя нейрона (входной, 2 скрытых, выходной)
        private InputLayer input_layer = null;
        private HiddenLayer hidden_layer_1 = new HiddenLayer(70, 15, NeuronType.Hidden, nameof(hidden_layer_1));
        private HiddenLayer hidden_layer_2 = new HiddenLayer(32, 70, NeuronType.Hidden, nameof(hidden_layer_2));
        private OutputLayer output_layer = new OutputLayer(10, 32, NeuronType.Output, nameof(output_layer));
        // 
        private double[] fact = new double[10]; // "Вероятности" того, что это та или иная цифра - ответ нейросети после SoftMax
        private double[] e_error_avr; // Среднее значение энергии ошибки каждой эпохи обучения (размер = количество эпох)
        // Свойства
        public double[] Fact { get => fact; }
        public double[] E_error_avr { get => e_error_avr; set => e_error_avr = value; }
        //Конструктор - ничего не принимает, т.к. объявление слоев происходит при инициализации без дополнительных параметров
        public Network() { }
        //Прямой проход: 
        public void ForwardPass(double[] newInput)
        {   // На вход приходит массив из 15 пикселей - то, что надо распознать
            hidden_layer_1.Data = newInput; // Т.к. входной слой не "активировал" первый скрытый, то делаем это вручную
            hidden_layer_1.Recognize(null, hidden_layer_2); // "Активируем" второй скрытый
            hidden_layer_2.Recognize(null, output_layer);   // "Активируем" выходной слой
            output_layer.Recognize(this, null); // Получаем результат работы
        }
        public void Train(Network net)
        {   // Режима тренировки нейросети, объявляет входной слой в режиме Train для считывания файла studying.txt
            input_layer = new InputLayer(NetworkMod.Train);
            // Задаем количество эпох
            int epoches = 100;
            double tmpSumError;
            double[] errors;
            double[] temp_gsums1;
            double[] temp_gsums2;

            e_error_avr = new double[epoches];
            for (int k = 0; k < epoches; k++) 
            {
                e_error_avr[k] = 0;
                input_layer.Shuffle(input_layer.Trainset);
                for (int i = 0; i < input_layer.Trainset.GetLength(0); i++)
                {   // Для каждой эпохи прогоняем НС через 100 примеров из файла, перемешав их ранее.
                    double[] tmpTrain = new double[15]; // Отдельный массив для пикселей
                    for (int j = 0; j < tmpTrain.Length; j++)
                        tmpTrain[j] = input_layer.Trainset[i, j + 1];
                    // Запускаем прямой проход
                    ForwardPass(tmpTrain);
                    // Вычисляем энергию ошибки
                    tmpSumError = 0;
                    errors = new double[fact.Length];
                    for (int x = 0; x < errors.Length; x++) 
                    {   // Вычисляем ошибку каждого выходного нейрона и суммируем: для правильного ожидаемый отклик равен 1, у остальных - 0
                        if (x == input_layer.Trainset[i, 0])
                            errors[x] = 1.0 - fact[x];
                        else
                            errors[x] = -fact[x]; //0-fact[x]
                        // Для того, чтобы отрицательные и положительные числа не занулялись берем половину квадрата ошибки
                        tmpSumError += errors[x] * errors[x] / 2;
                    }
                    e_error_avr[k] += tmpSumError / errors.Length; // Берем среднее значение ошибки для всех выходов = ошибка / 10
                    // Обратный проход
                    temp_gsums2 = output_layer.BackwardPass(errors);        // Получаем ошибки для второго скрытого, корректируем веса выходного слоя
                    temp_gsums1 = hidden_layer_2.BackwardPass(temp_gsums2); // Получаем ошибки для первого скрытого, корректируем веса первого скрытого
                    hidden_layer_1.BackwardPass(temp_gsums1);               // Корректируем веса первого скрытого, полученный массив "ошибок" входного слоя бесполезен
                }
                e_error_avr[k] /= input_layer.Trainset.GetLength(0); // Вычисляем среднюю величину ошибок на одну строку файл в данную эпоху
            }
            input_layer = null;
            //запись скорректированных весов в память
            hidden_layer_1.WeightInitialize(MemoryMode.SET, "memory\\" + nameof(hidden_layer_1) + "_memory.csv");
            hidden_layer_2.WeightInitialize(MemoryMode.SET, "memory\\" + nameof(hidden_layer_2) + "_memory.csv");
            output_layer.WeightInitialize(MemoryMode.SET, "memory\\" + nameof(output_layer) + "_memory.csv");
            // Еще где-то сохранять/отображать полученные ошибки, сейчас этого нет
        }
    }
}
