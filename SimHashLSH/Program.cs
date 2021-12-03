using System;
using System.IO;

namespace SimHashLSH
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(Environment.CurrentDirectory);
            StreamReader read = new StreamReader("lab1A_examples/test2.in");
            StreamWriter write = new StreamWriter("lab1A_examples/programout.txt");
            SimHash simhash = new SimHash(read.ReadLine, Console.WriteLine);
            simhash.RunSimHash();
            read.Close();
            write.Close(); 
            //SimHashBuckets lsh = new SimHashBuckets(8, read.ReadLine, Console.WriteLine);
            //lsh.RunSimHashBuckets();
        }
    }
}
