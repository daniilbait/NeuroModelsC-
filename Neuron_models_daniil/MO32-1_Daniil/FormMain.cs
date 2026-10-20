using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MO32_1_Daniil.NeuroNet;
using System.IO;

namespace MO32_1_Daniil
{
    public partial class FormMain : Form
    {
        private double[] inputPixel;
        private Network network;
        public FormMain()
        {
            InitializeComponent();

            inputPixel = new double[15];
            network = new Network();
        }

        private void matrix_digit_button_Click(object sender, EventArgs e)
        {   // Метод для изменения
            if (((Button)sender).BackColor==Color.White)
            {
                ((Button)sender).BackColor = Color.Black;
                inputPixel[((Button)sender).TabIndex] = 1d;
            }
            else
            {
                ((Button)sender).BackColor = Color.White;
                inputPixel[((Button)sender).TabIndex] = 0d;
            }
        }

        private void save_studying_material_button_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "studying.txt";
            string to_output = numeric_values_output.Value.ToString();
            for (int i = 0; i < inputPixel.Length; i++)
                to_output += " " + inputPixel[i].ToString();
            to_output += "\n";
            File.AppendAllText(path, to_output);
        }

        private void save_test_material_button_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "test.txt";
            string to_output = numeric_values_output.Value.ToString();
            for (int i = 0; i < inputPixel.Length; i++)
                to_output += " " + inputPixel[i].ToString();
            to_output += "\n";
            File.AppendAllText(path, to_output);
        }

        private void button_recognize_Click(object sender, EventArgs e)
        {
            network.ForwardPass(inputPixel);
            label_output.Text = network.Fact.ToList().IndexOf(network.Fact.Max()).ToString();
            label_probability.Text = (100 * network.Fact.Max()).ToString("0.00") + " %";
        }
    }
}
