using System;
using System.IO;
using System.Windows.Forms;

namespace MO32_1_Daniil.NeuroNet
{
    abstract class Layer
    {
        //Поля
        protected string name_Layer;                // Наименование слоя
        string pathDirWeights;                      // Путь, где будут храниться веса
        string pathFileWeights;                     // Имя файла хранения синапт.весов
        protected int numofneurons;                 // Число нейронов текущего слоя
        protected int numofprevneurons;             // Число нейронов пред.слоя
        protected const double learningrate = 0.05; // Скорость обучения
        protected const double momuntum = 0.5;      // Момент инерции
        protected double[,] lastdeltaweights;       // Веса предыдущиго слоя
        protected Neuron[] neurons;                 // Массив нейронов
        //Свойства
        public double[] Data
        {
            set
            {
                for (int i = 0; i < numofneurons; i++)
                    neurons[i].Activator(value);
            }
        }
        //Конструтор
        public Layer(int non, int nopn, NeuronType nt,string nm_Layer)
        {
            numofneurons = non;
            numofprevneurons = nopn;
            neurons = new Neuron[non];
            name_Layer = nm_Layer;
            pathDirWeights=AppDomain.CurrentDomain.BaseDirectory + "memory\\";
            pathFileWeights = pathDirWeights + name_Layer + "_memory.csv";

            lastdeltaweights = new double[non, nopn + 1];
            double[,] Weights;

            if(File.Exists(pathFileWeights))
            {
                Weights = WeightInitialize(MemoryMode.GET, pathFileWeights);
            }
            else
            {
                Directory.CreateDirectory(pathDirWeights);
                Weights = WeightInitialize(MemoryMode.GET, pathFileWeights);
            }

            for(int i=0;i<non;i++)
            {
                double[] tmp_weight = new double[nopn+1];
                for(int j=0;j<nopn+1;j++)
                {
                    tmp_weight[j] = Weights[i, j];
                }
                neurons[i] = new Neuron(tmp_weight, nt);
            }
        }
        //Методы
        public double[,] WeightInitialize(MemoryMode mm, string path)
        {
            char[] delim = new char[] { ';', ' ' };
            string tmpStr;
            string[] tmpStrWeights;
            double[,] weights = new double[numofneurons, numofprevneurons + 1];

            switch (mm)
            {
                case MemoryMode.GET:
                    tmpStrWeights = File.ReadAllLines(path);
                    string[] memory_elemnt;
                    for(int i=0;i<numofneurons;i++)
                    {
                        memory_elemnt = tmpStrWeights[i].Split(delim);
                        for(int j=0;j<numofprevneurons+1;j++)
                        {
                            weights[i,j]=double.Parse(memory_elemnt[j].Replace(',','.'),
                                System.Globalization.CultureInfo.InvariantCulture);
                        }
                    }
                    break;
                case MemoryMode.SET:
                    tmpStrWeights = new string[numofneurons];

                    if(!File.Exists(path))
                    {
                        MessageBox.Show("!!!ВНИМАНИЕ!!!\nФайл " + name_Layer + "_memory.csv синаптических весов" +
                            "\nНЕ НАЙДЕН!!!\nПосле нажатия кнопки ОК автоматически создается отсутствующий файл и все значения",
                            "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    for(int i=0;i<numofneurons;i++)
                    {
                        tmpStr = neurons[i].Weights[0].ToString();
                        for (int j = 1; j < numofprevneurons + 1; j++)
                            tmpStr += delim[0] + neurons[i].Weights[j].ToString();
                        tmpStrWeights[i] = tmpStr;
                    }
                    File.WriteAllLines(path, tmpStrWeights);
                    break;
                case MemoryMode.INIT:
                    MessageBox.Show("!!!ВНИМАНИЕ!!!\nФайл " + name_Layer + "_memory.csv синаптических весов\nНЕ НАЙДЕН!!!" +
                            "\nПосле нажатия кнопки ОК автоматически инициализируются веса и создастся отсутствующий файл, "+
                            "нейросеть вернется к исходному состоянию <<<новорожденности>>>, и потребуется обучать её ЗАНОВО",
                            "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    Random random = new Random();
                    //Инициализация исходных значений
                    double tmpRatio;    //Коэффициент
                    double tmpShift;    //Смещение
                    double[] tmpArr = new double[numofprevneurons + 1];
                    tmpStrWeights = new string[numofneurons];

                    tmpRatio= Math.Sqrt(3.0 / Math.Sqrt(numofprevneurons));
                    //double variaty = 1.0 / Math.Sqrt(numofprevneurons);
                    //double disperce = Math.Sqrt(variaty * 3.0);
                    for (int i = 0; i < numofneurons; i++)
                    {
                        double sum = 0.0;
                        for (int j = 0; j < numofprevneurons + 1; j++)
                        {
                            tmpArr[j] = random.NextDouble() * tmpRatio;
                            sum += tmpArr[j]; //Проверка СрЗнач=0
                        }
                        tmpShift = sum / (numofprevneurons + 1);
                        for (int j = 0; j < numofprevneurons + 1; i++) 
                        {
                            tmpArr[j] -= tmpShift;
                        }
                    }
                    File.WriteAllLines(path, tmpStrWeights);
                    break;
            }
            return weights;
        }


        abstract public void Recognize(Network net, Layer nextLayer);
        abstract public double[] BackwardPass(double[] stuff);

    }
}
