using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TestProject.Utils;

namespace TestProject
{
    public class TransactionDataGenerator
    {
        public int BatchCount { get; private set; }
        public int RowsInFile { get; private set; }

        public string FolderPath { get; private set; }

        private List<string> _allReferenceDataLookUps = new List<string>();

        public TransactionDataGenerator(int batchCount, int rowsInFile, string folderPath)
        {
            BatchCount = batchCount;
            RowsInFile = rowsInFile;
            FolderPath = folderPath;
        }

        public void GenerateTransactionData()
        {
            var dbHelper = new DataBaseHelper();
            _allReferenceDataLookUps = dbHelper.ExecuteReader("SELECT LookupName FROM ReferenceData").ToList();

            Parallel.For(0, BatchCount, index =>
            {
                string filePath = Path.Combine(FolderPath, $"{index.ToString()}.txt");
                GenerateTransactionDataFile(filePath);
            });
        }

        private void GenerateTransactionDataFile(string filePath)
        {
            using (var streamWriter = new StreamWriter(filePath))
            {
                for (int j = 0; j < RowsInFile; j++)
                {
                    var randomReferenceDataIndex = ThreadSafeRandomGenerator.Next(_allReferenceDataLookUps.Count);
                    var referenceDataLookUp = _allReferenceDataLookUps[randomReferenceDataIndex];

                    var randomString = StringUtils.GenerateRandomString(5);

                    string nextValue = referenceDataLookUp + ", " + randomString;

                    streamWriter.WriteLine(nextValue);
                }
            }
        }
    }
}