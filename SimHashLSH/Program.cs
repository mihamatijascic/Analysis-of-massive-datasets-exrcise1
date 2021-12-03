using System;
using System.IO;

namespace SimHashLSH
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CheckTaskA();
            CheckTaskB();
        }

        public static void CheckTaskA()
        {
            StreamReader read = new StreamReader("lab1A_examples/test2.in");
            StreamWriter write = new StreamWriter("lab1A_examples/result.out");
            SimHash simhash = new SimHash(read.ReadLine, write.WriteLine);

            simhash.RunSimHash();
            read.Close();
            write.Close();

            var linesSolution = File.ReadAllLines("lab1A_examples/test2.out");
            var linesResult = File.ReadAllLines("lab1A_examples/result.out");

            int correctCounter = 0;
            for(int i = 0; i < linesResult.Length; i++)
            {
                var solution = linesSolution[i];
                var result = linesResult[i];   

                if(result == solution) correctCounter++;
            }
            double accuracy = ((double)correctCounter) / linesResult.Length;
            Console.WriteLine($"Task A, test 2: accuracy: {correctCounter}/{linesSolution.Length}");
        }

        public static void CheckTaskB()
        {
            StreamReader read = new StreamReader("lab1B_examples/test2.in");
            StreamWriter write = new StreamWriter("lab1B_examples/result.out");
            SimHashBuckets simHashBuckets= new SimHashBuckets(8, read.ReadLine, write.WriteLine);

            simHashBuckets.RunSimHashBuckets();
            read.Close();
            write.Close();

            var linesSolution = File.ReadAllLines("lab1B_examples/test2.out");
            var linesResult = File.ReadAllLines("lab1B_examples/result.out");

            int correctCounter = 0;
            for (int i = 0; i < linesResult.Length; i++)
            {
                var solution = linesSolution[i];
                var result = linesResult[i];

                if (result == solution) correctCounter++;
            }
            double accuracy = ((double)correctCounter) / linesResult.Length;
            Console.WriteLine($"Task B, test 2: accuracy: {correctCounter}/{linesSolution.Length}");
        }
    }
}
