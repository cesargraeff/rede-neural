using System.Linq;
using System;

namespace neuralnetwork.Core
{

    [Serializable]
    class NeuralNetwork
    {

        private int nInput;
        private int nHidden;
        private int nOutput;
        private int nEpoch;
        private double maxError;


        private double learningRate;
        private double momentum;

        private double[] inputValues;
        private double[] hiddenValues;
        private double[] outputValues;


        private double[][] hiddenWeigth;
        private double[][] outputWeigth;


        private double[] hiddenError;
        private double[] outputError;


        private double[] expectedOutput;

        public NeuralNetwork(int nInput, int nHidden, int nOutput, int nEpoch, double maxError, double learningRate, double momentum)
        {

            this.nInput = nInput;
            this.nHidden = nHidden;
            this.nOutput = nOutput;
            this.nEpoch = nEpoch;
            this.maxError = maxError;
            this.learningRate = learningRate;
            this.momentum = momentum;

        }


        private void init()
        {

            this.inputValues = new double[this.nInput];
            this.hiddenValues = new double[this.nHidden];
            this.outputValues = new double[this.nOutput];

            this.hiddenError = new double[this.nHidden];
            this.outputError = new double[this.nOutput];

            this.initWeigth();
        }


        private void initWeigth()
        {

            var rand = new Random();

            this.hiddenWeigth = new double[nHidden][];
            for (int i = 0; i < this.nHidden; i++)
            {
                this.hiddenWeigth[i] = new double[nInput];
                for (int j = 0; j < nInput; j++)
                {
                    this.hiddenWeigth[i][j] = rand.NextDouble() / 2;
                }
            }


            this.outputWeigth = new double[nOutput][];
            for (int i = 0; i < this.nOutput; i++)
            {
                this.outputWeigth[i] = new double[nHidden];
                for (int j = 0; j < nHidden; j++)
                {
                    this.outputWeigth[i][j] = rand.NextDouble() / 2;
                }
            }

        }

        public void train(double[][] input, double[][] expected)
        {

            this.init();

            if (input.Length != expected.Length)
            {
                throw new Exception("Os arrays de entrada e saida devem possuir o mesmo tamanho!");
            }


            int epoch = 0;
            double err = 100.0;

            while (epoch < nEpoch && err > this.maxError)
            {

                for (int i = 0; i < input.Length; i++)
                {
                    this.inputValues = input[i];
                    this.expectedOutput = expected[i];

                    this.forward();
                    this.backward();
                    
                    Console.WriteLine("Epoca: " + epoch + " - Erro: " + outputError.Max().ToString());
                }

                epoch++;
            }

        }


        public double[] test(double[] input)
        {
            this.inputValues = input;

            this.forward();

            return this.outputValues;
        }


        private void forward()
        {

            for (int i = 0; i < nHidden; i++)
            {
                this.hiddenValues[i] = 0;

                for (int j = 0; j < nInput; j++)
                {
                    this.hiddenValues[i] += this.inputValues[j] * this.hiddenWeigth[i][j];
                }

                this.hiddenValues[i] = this.sigmoid(this.hiddenValues[i]);
            }


            for (int i = 0; i < nOutput; i++)
            {
                this.outputValues[i] = 0;

                for (int j = 0; j < nHidden; j++)
                {
                    this.outputValues[i] += this.hiddenValues[j] * this.outputWeigth[i][j];
                }

                this.outputValues[i] = this.sigmoid(this.outputValues[i]);
            }
        }


        private void backward()
        {

            for (int i = 0; i < nOutput; i++)
            {
                double errorFactor = this.expectedOutput[i] - this.outputValues[i];

                this.outputError[i] = this.outputValues[i] * (1 - this.outputValues[i]) * errorFactor;
            }


            for (int i = 0; i < nHidden; i++)
            {
                this.hiddenError[i] = 0;

                for (int j = 0; j < nOutput; j++)
                {
                    this.hiddenError[i] += this.outputError[j] * this.outputWeigth[j][i];
                }
                this.hiddenError[i] = this.hiddenValues[i] * (1 - this.hiddenValues[i]) * this.hiddenError[i];
            }


            for (int i = 0; i < nHidden; i++)
            {
                for (int j = 0; j < nInput; j++)
                {
                    this.hiddenWeigth[i][j] *= this.momentum;
                    this.hiddenWeigth[i][j] += this.learningRate * this.inputValues[j] * this.hiddenError[i];
                }

            }

            for (int i = 0; i < nOutput; i++)
            {
                for (int j = 0; j < nHidden; j++)
                {
                    this.outputWeigth[i][j] *= this.momentum;
                    this.outputWeigth[i][j] += this.learningRate * this.hiddenValues[j] * this.outputError[i];
                }

            }

        }


        private double sigmoid(double value)
        {
            return 1.0 / (1.0 + Math.Exp(-value));
        }


    }

}
