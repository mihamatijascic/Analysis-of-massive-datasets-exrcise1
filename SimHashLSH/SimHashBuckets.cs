using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimHashLSH
{
    public class SimHashBuckets
    {
        private Dictionary<int, HashSet<int>> _candidates = new Dictionary<int, HashSet<int>>();
        public SimHash Hash { get; set; }
        public int NumberOfBands { get; set; }

        public SimHashBuckets(int numberOfBands, Func<string> readLine, Action<string> writeLine)
        {
            Hash = new SimHash(readLine, writeLine);
            this.NumberOfBands = numberOfBands;
        }

        public void RunSimHashBuckets()
        {
            Hash.ClearFingerprints();
            Hash.ReadText();
            LshAlgorithm();
            Hash.QueryCandidateIds = (id) => _candidates[id] != null ? _candidates[id] : new HashSet<int>();
            Hash.ReadQueries();
            InitializeParameters();
        }

        public void LshAlgorithm()
        {
            int bandLength = Hash.FingerPrints.First().Length/NumberOfBands;
            for (int band = 0; band < NumberOfBands; band++)
            {
                Dictionary<int, HashSet<int>> buckets = new Dictionary<int, HashSet<int>>();
                for (int fingerPrintId = 0; fingerPrintId < Hash.FingerPrints.Count; fingerPrintId++)
                {
                    AddFingerprintToBucket(fingerPrintId, band, bandLength, buckets);
                }
            }
        } 

        private void AddFingerprintToBucket(int fingerPrintId, int band, int bandLength, Dictionary<int, HashSet<int>> buckets)
        {
            var fingerPrint = Hash.FingerPrints[fingerPrintId];
            int bucketId = HashToInt(fingerPrint, bandLength * band, bandLength * band + bandLength);
            HashSet<int> fingerprintIdsInBucket = new HashSet<int>();
            
            if (buckets.ContainsKey(bucketId))
            {
                fingerprintIdsInBucket = buckets[bucketId];
                foreach (var id in fingerprintIdsInBucket)
                {
                    AddNewCandidate(fingerPrintId, id);
                    AddNewCandidate(id, fingerPrintId);
                }
            }
            else
            {
                buckets.Add(bucketId, null);
            }

            fingerprintIdsInBucket.Add(fingerPrintId);
            buckets[bucketId] = fingerprintIdsInBucket;
        }

        private void AddNewCandidate(int id, int candidateId)
        {
            
            if (!_candidates.ContainsKey(id)) _candidates.Add(id, new HashSet<int>());
            _candidates[id].Add(candidateId);
        }

        public int HashToInt(BitArray bits, int startIndex, int endIndex)
        {
            int pow = 1;
            int result = 0;
            for (int index = startIndex; index < endIndex; index++)
            {
                int bit = bits[index] ? 1 : 0;
                result += pow * bit;
                pow *= 2;
            }
            return result;
        }

        public void InitializeParameters()
        {
            _candidates = new Dictionary<int, HashSet<int>>();
        }
    }
}
