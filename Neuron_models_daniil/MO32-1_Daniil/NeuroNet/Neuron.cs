using System;
using static System.Math;

namespace MO32_1_Daniil.NeuroNet
{
    class Neuron
    {
        //По-о-оле, поле, мир бездбрежный
        private NeuronType type;
        private double[] weights;
        private double[] inputs;
        private double output;
        private double derivative;

        // константы для формул

        //
        public double[] Weights { get => weights; set => weights = value; }
        public double[] Inputs { get => inputs; set => inputs = value; }
        public double Output { get => output; }
        public double Derivative { get => derivative; }
        //
        public Neuron(double[] memoryWeights, NeuronType typeNeuron)
        {
            type = typeNeuron;
            weights = memoryWeights;
        }
        //
        public void Activator(double[] inp)
        {
            inputs = inp;
            double sum = weights[0];
            for(int i=0;i<inputs.Length;i++)
            {
                sum += weights[i+1]*inputs[i];
            }
            switch (type)
            {
                case NeuronType.Hidden:
                    output = Nechetnya_sigmoidal(sum);
                    derivative = Derivative_sigmoidal(sum);
                    break;
                case NeuronType.Output:
                    output = Exp(sum);
                    break;
                default: break;
            }
        }
        private double Nechetnya_sigmoidal(double sum)
        {
            return (Exp(sum) - Exp(-sum)) / (Exp(sum) + Exp(-sum));
        }
        private double Derivative_sigmoidal(double sum)
        {
            return 4.0 / Pow((Exp(sum) + Exp(-sum)), 2);
        }
    }
}
