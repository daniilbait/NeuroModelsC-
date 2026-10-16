namespace MO32_1_Daniil.NeuroNet
{
    class HiddenLayer : Layer
    {
        public HiddenLayer(int non, int nopn, NeuronType nt, string type) : base(non, nopn, nt, type) { }
        // Прямой проход
        public override void Recognize(Network net, Layer nextLayer)
        {
            double[] hidden_out = new double[numofneurons];
            for(int i=0;i<numofneurons;i++)
            {
                hidden_out[i] = neurons[i].Output;
            }
            nextLayer.Data = hidden_out;    //передача выходного сигнала на вход след.слоя
        }
        // Обратный проход
        public override double[] BackwardPass(double[] stuff)
        {
            double[] gr_sum = new double[numofneurons];
            return gr_sum;
        }




    }
}
