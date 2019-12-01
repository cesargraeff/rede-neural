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
        public void Get(
            [FromBody] InputModel[] dataset
        )
        {

            int nData = dataset.Length;

            if (nData == 0)
            {
                throw new Exception("Informe no minimo um cenário de teste!");
            }

            int nInput = dataset[0].input.Length;
            int nOutput = dataset[0].expected.Length;

            var input = new double[nData][];
            var expected = new double[nData][];

            for (int i = 0; i < nData; i++)
            {
                input[i] = new double[nInput];
                char[] charsI = new String(dataset[i].input).ToCharArray();
                for (int j = 0; j < nInput; j++)
                {
                    input[i][j] = Convert.ToDouble(charsI[j] - 48);
                }

                expected[i] = new double[nOutput];
                char[] charsO = new String(dataset[i].expected).ToCharArray();
                for (int j = 0; j < nOutput; j++)
                {
                    expected[i][j] = Convert.ToDouble(charsO[j] - 48);
                }
            }

            var rn = new NeuralNetwork(nInput, 40, nOutput, 1000, 0.01, 0.3, 1);
            rn.train(input, expected);

            for (int i = 0; i < nData; i++)
            {

                double[] output = rn.test(input[i]);


                Console.WriteLine("LETRA: "+dataset[i].letter);
                Console.Write("ESP: ");
                for (int j = 0; j < nOutput; j++)
                {
                    Console.Write(expected[i][j].ToString());
                }
                Console.WriteLine();


                Console.Write("RES: ");
                for (int j = 0; j < nOutput; j++)
                {
                    Console.Write(output[j] >= 0.5 ? "1" : "0");
                }
                Console.WriteLine();
                Console.WriteLine();
            }

        }


    }
}