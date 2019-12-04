using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System;
using Microsoft.AspNetCore.Mvc;
using neuralnetwork.Core;
using neuralnetwork.Models;

namespace neuralnetwork.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NeuralController : ControllerBase
    {

        [HttpPost("train")]
        public void Train(
            [FromBody] InputModel[] dataset,
            [FromQuery] int iteracoes,
            [FromQuery] int intermediaria,
            [FromQuery] double aprendizado,
            [FromQuery] double momentum
        )
        {

            int nData = dataset.Length;

            if (nData == 0)
            {
                throw new Exception("Informe no minimo um cenário de teste!");
            }

            int nInput = dataset[0].input.Length;
            int nOutput = dataset[0].expected.Length;


            var rn = new NeuralNetwork(nInput, intermediaria, nOutput, iteracoes, 0.01, aprendizado, momentum);


            var input = new double[nData][];
            var expected = new double[nData][];

            for (int i = 0; i < nData; i++)
            {
                input[i] = Utils.ToDoubleArray(dataset[i].input);
                expected[i] = Utils.ToDoubleArray(dataset[i].expected);
            }


            rn.train(input, expected);


            string file = System.IO.Path.GetFullPath("train.obj");
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(file, FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, rn);
            stream.Close();

        }



        [HttpPost("test")]
        public TestModel Test(
            [FromBody] InputModel[] dataset
        )
        {

            string file = System.IO.Path.GetFullPath("train.obj");
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(file, FileMode.Open, FileAccess.Read);
            var rn = (NeuralNetwork)formatter.Deserialize(stream);
            stream.Close();

            int nData = dataset.Length;

            if (nData == 0)
            {
                throw new Exception("Informe no minimo um cenário de teste!");
            }

            int nInput = dataset[0].input.Length;
            int nOutput = dataset[0].expected.Length;

            int[][] confusao = new int[nOutput + 1][];
            for (int i = 0; i < nOutput + 1; i++)
            {
                confusao[i] = new int[nOutput + 1];
            }

            int acertos = 0;
            for (int i = 0; i < nData; i++)
            {

                double[] input = Utils.ToDoubleArray(dataset[i].input);
                double[] output = rn.test(input);

                string esp = dataset[i].expected;
                string res = Utils.DoubleArrayToString(output);

                int real = esp.IndexOf('1') + 1;
                int predict = res.IndexOf('1') + 1;
                confusao[real][predict]++;

                if (String.Equals(res, esp))
                {
                    acertos++;
                }
            }


            int[] FN = new int[confusao.Length];
            int[] FP = new int[confusao.Length];
            int TN = 0;

            for (int i = 0; i < confusao.Length; i++)
            {
                for (int j = 0; j < confusao.Length; j++)
                {
                    if (i != j)
                    {
                        FN[i] += confusao[i][j];
                        FP[j] += confusao[i][j];
                    }
                    else
                    {
                        TN += confusao[i][j];
                    }
                }
            }

            double[] precision = new double[confusao.Length];
            double[] recall = new double[confusao.Length];
            double[] specificity = new double[confusao.Length];
            for (int i = 0; i < confusao.Length; i++)
            {

                if (confusao[i][i] + FP[i] > 0)
                {
                    precision[i] = Convert.ToDouble(confusao[i][i]) / Convert.ToDouble(confusao[i][i] + FP[i]);
                }
                else
                {
                    precision[i] = 1;
                }

                if (confusao[i][i] + FN[i] > 0)
                {
                    recall[i] = Convert.ToDouble(confusao[i][i]) / Convert.ToDouble(confusao[i][i] + FN[i]);
                }
                else
                {
                    recall[i] = 0;
                }

                if (TN - confusao[i][i] + FP[i] > 0)
                {
                    specificity[i] = Convert.ToDouble(TN - confusao[i][i]) / Convert.ToDouble(TN - confusao[i][i] + FP[i]);
                }
                else
                {
                    specificity[i] = 1;
                }
            }

            double acuracy = Convert.ToDouble(acertos) / Convert.ToDouble(nData);

            return new TestModel
            {
                confusao = confusao,
                acertos = acertos,
                acuracia = acuracy,
                erro = (1 - acuracy),
                precision = precision,
                recall = recall,
                specificity = specificity,
                FN = FN,
                FP = FP,
                TN = TN
            };
        }

    }
}