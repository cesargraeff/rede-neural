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

            rn.train(input, expected);


            string file = System.IO.Path.GetFullPath("train.obj");
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(file, FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, rn);
            stream.Close();

        }



        [HttpPost("test")]
        public int Test(
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


            string file = System.IO.Path.GetFullPath("train.obj");
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(file, FileMode.Open, FileAccess.Read);
            var rn = (NeuralNetwork)formatter.Deserialize(stream);
            stream.Close();

            int cont = 0;

            for (int i = 0; i < nData; i++)
            {

                double[] output = rn.test(input[i]);


                Console.WriteLine("LETRA: " + dataset[i].letter);
                Console.Write("ESP: ");
                string esp = "";
                for (int j = 0; j < nOutput; j++)
                {
                    esp += expected[i][j].ToString();
                }
                Console.WriteLine(esp);


                Console.Write("RES: ");
                string res = "";
                for (int j = 0; j < nOutput; j++)
                {
                    res += output[j] >= 0.5 ? "1" : "0";
                }
                Console.WriteLine(res);
                Console.WriteLine();

                if (String.Equals(res, esp))
                {
                    cont++;
                }
            }

            return cont;
        }

    }
}