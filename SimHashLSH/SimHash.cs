using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace SimHashLSH
{
    public class SimHash
    {
        public static MD5 Md5 = System.Security.Cryptography.MD5.Create();

        public List<BitArray> FingerPrints { get; set; }
        public Func<string> ReadLine { get; set; }
        public Action<string> WriteLine { get; set; }
        //before ReadQuery function you should set your own QueryCandidateIds function
        public Func<int, IEnumerable<int>> QueryCandidateIds { get; set; }

        public SimHash(Func<string> readLine, Action<string> writeLine)
        {
            this.ReadLine = readLine;
            this.WriteLine = writeLine;
            ClearFingerprints();
        }

        public void RunSimHash()
        {
            ClearFingerprints();
            ReadText();
            ReadQueries();
        }

        public void ClearFingerprints()
        {
            FingerPrints = new List<BitArray>();
        }

        public void ReadText()
        {
            int numberOfTexts = int.Parse(ReadLine());
            //default function that gets candidates for similar texts,
            //simple way to implement func is to say that all text's are candidates
            QueryCandidateIds = (id) => Enumerable.Range(0, numberOfTexts - 1);
            for (int i = 0; i < numberOfTexts; i++)
            {
                string text = ReadLine();
                FingerPrints.Add(CreateFingerprint(text));
            }
        }

        public void ReadQueries()
        {
            int numberOfQueries = int.Parse(ReadLine());
            for (int i = 0; i < numberOfQueries; i++)
            {
                var parameters = ReadLine().Split(null).Select(int.Parse).ToArray();
                var indexOfText = parameters[0];
                var hammingDistance = parameters[1];
                SolveQuery(indexOfText, hammingDistance, QueryCandidateIds(indexOfText));
            }
        }

        private void SolveQuery(int indexOfText, int maxHammingDistance, IEnumerable<int> candidates)
        {
            BitArray textFingerprint = FingerPrints[indexOfText];
            int passedCondition = 0;

            foreach (int candidate in candidates)
            {
                if(candidate == indexOfText) continue;

                int hammingDistance = HammingDistance(textFingerprint, FingerPrints[candidate], maxHammingDistance);

                if (hammingDistance <= maxHammingDistance) passedCondition++;
            }

            WriteLine(passedCondition.ToString());
        }

        public int HammingDistance(BitArray first, BitArray second, int maxHamming = int.MaxValue)
        {
            int hammingDistance = 0;
            for (int bitIndex = 0; bitIndex < first.Length; bitIndex++)
            {
                if (first[bitIndex] != second[bitIndex]) hammingDistance++;
                if (hammingDistance > maxHamming) break;
            }

            return hammingDistance;
        }

        public static BitArray CreateFingerprint(string text)
        {
            int[] integerFingerprint = new int[128];
            var tokens = text.TrimEnd().Split(null);

            foreach (var token in tokens)
            {
                var byteToken = Encoding.ASCII.GetBytes(token);
                var hash = new BitArray(Md5.ComputeHash(byteToken));

                int index = 0;
                foreach (bool bit in hash)
                {
                    if (bit)
                    {
                        integerFingerprint[index]++;
                    }
                    else
                    {
                        integerFingerprint[index]--;
                    }

                    index++;
                }
            }

            BitArray fingerPrint = new BitArray(128);
            for (int index = 0; index < integerFingerprint.Length; index++)
            {
                fingerPrint[index] = integerFingerprint[index] >= 0;
            }

            return fingerPrint;
        }

        public static string ConvertToHex(BitArray bitArray)
        {
            byte[] convert = new byte[bitArray.Length / 8];
            bitArray.CopyTo(convert, 0);
            return BitConverter.ToString(convert).Replace("-", string.Empty).ToLower();
        }
    }
}
