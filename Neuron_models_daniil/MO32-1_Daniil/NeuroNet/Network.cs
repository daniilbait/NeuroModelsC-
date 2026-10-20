namespace MO32_1_Daniil.NeuroNet
{
    class Network
    {
        //Поля 
        private InputLayer input_layer = null;
        private HiddenLayer hidden_layer_1 = new HiddenLayer(70, 15, NeuronType.Hidden, nameof(hidden_layer_1));
        private HiddenLayer hidden_layer_2 = new HiddenLayer(32, 70, NeuronType.Hidden, nameof(hidden_layer_2));
        private OutputLayer output_layer = new OutputLayer(10,32,NeuronType.Output, nameof(output_layer));

        private double[] fact = new double[10];
        private double[] e_error_avr; // среднее значение энергии ошибки эпохи обучения
        // Свойства

        public double[] Fact { get => fact; }
        public double[] E_error_avr { get => e_error_avr; set => e_error_avr = value; }
        //Конструктор
        public Network() { }
        //Прямой проход
        public void ForwardPass(double[] newInput)
        {
            hidden_layer_1.Data = newInput;
            hidden_layer_1.Recognize(null, hidden_layer_2);
            hidden_layer_2.Recognize(null, output_layer);
            output_layer.Recognize(this, null);
        }
    }
}
