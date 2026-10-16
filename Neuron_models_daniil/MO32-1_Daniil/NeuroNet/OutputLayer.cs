namespace MO32_1_Daniil.NeuroNet
{
    class OutputLayer : Layer
    {
        public OutputLayer(int non, int nopn, NeuronType nt, string type) : base(non, nopn, nt, type) { }


        // Прямой проход
        public override void Recognize(Network net, Layer nextLayer)
        {
            double e_sum = 0;
            for (int i = 0; i < neurons.Length; i++)
                e_sum += neurons[i].Output;
            for (int i = 0; i < neurons.Length; i++)
                net.Fact[i] = neurons[i].Output / e_sum;
        }
        // Обратный проход
        public override double[] BackwardPass(double[] stuff)
        {
            double[] gr_sum = new double[numofneurons];
            return gr_sum;
        }
    }
}
