namespace neuralnetwork.Models
{
    public class TestModel
    {

        public int acertos { get; set; }

        public double acuracia { get; set; }

        public double erro { get; set; }

        public int[][] confusao { get; set; }

        public double[] precision { get; set; }

        public double[] recall { get; set; }

        public double[] specificity { get; set; }

        public int[] FN { get; set; }

        public int[] FP { get; set; }

        public int TN { get; set; }

    }
}