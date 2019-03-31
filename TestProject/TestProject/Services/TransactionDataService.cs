using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TestProject.Models;
using TestProject.Utils;

namespace TestProject.Services
{
    public class TransactionDataService
    {
        public const string FolderPath = @"C:\Data\TransactionData";

        #region Import feature
        public void Import()
        {
            var dbHelper = new DataBaseHelper();
            var allReferenceDataLookUps =
                dbHelper.ExecuteReader("SELECT * FROM ReferenceData", sqlDataReader => new ReferenceData
                {
                    Id = sqlDataReader.GetInt32(0),
                    LookupName = sqlDataReader.GetString(1)
                }).ToDictionary(x => x.LookupName);

            var files = Directory.GetFiles(FolderPath);

            Parallel.ForEach(files, x => PrepareFileForImport(x, allReferenceDataLookUps));

            files = Directory.GetFiles(FolderPath);

            foreach (var file in files)
            {
                dbHelper.ExecuteNonQuery($"exec sp_ImportTransactionalData @passToFileWithData = '{file}'");
                File.Delete(file);
            }
        }

        private void PrepareFileForImport(string file, Dictionary<string, ReferenceData> allReferenceDataLookUps)
        {
            var tempTransactionData = new List<string>();

            using (var streamReader = new StreamReader(file))
            {
                while (!streamReader.EndOfStream)
                {
                    var rowFromFile = streamReader.ReadLine();
                    var rowElements = rowFromFile.Split(new string[] { ", " }, StringSplitOptions.None);

                    var lookupName = rowElements[0];
                    var transactionValue = rowElements[1];

                    var lookUpId = allReferenceDataLookUps[lookupName].Id;
                    var newString = ", " + lookUpId.ToString() + ", " + transactionValue;

                    tempTransactionData.Add(newString);
                }
            }

            var newFileName = file.Replace(".", "_n.");
            using (var streamWriter = new StreamWriter(newFileName))
            {
                foreach (var x in tempTransactionData)
                {
                    streamWriter.WriteLine(x);
                }
            }

            File.Delete(file);
        }

        #endregion

        #region Generate feature

        public void Generate(int batchCount, int rowsInFile)
        {
            var dbHelper = new DataBaseHelper();
            var allReferenceDataLookUps = dbHelper.ExecuteReader("SELECT LookupName FROM ReferenceData").ToList();

            Parallel.For(0, batchCount, index =>
            {
                string filePath = Path.Combine(FolderPath, $"{index.ToString()}.txt");
                GenerateTransactionDataFile(filePath, rowsInFile, allReferenceDataLookUps);
            });
        }

        private void GenerateTransactionDataFile(string filePath, int rowsInFile, List<string> allReferenceDataLookUps)
        {
            using (var streamWriter = new StreamWriter(filePath))
            {
                for (int j = 0; j < rowsInFile; j++)
                {
                    var randomReferenceDataIndex = ThreadSafeRandomGenerator.Next(allReferenceDataLookUps.Count);
                    var referenceDataLookUp = allReferenceDataLookUps[randomReferenceDataIndex];

                    var randomString = StringUtils.GenerateRandomString(5);

                    string nextValue = referenceDataLookUp + ", " + randomString;

                    streamWriter.WriteLine(nextValue);
                }
            }
        }

        #endregion

        public IEnumerable<TransactionalData> FetchTransactionalData(int skip, int take)
        {
            string query = "SELECT * " +
                           "FROM TransactionalData " +
                           "ORDER BY Id " +
                          $"OFFSET({skip}) ROWS FETCH NEXT({take}) ROWS ONLY";

            var dbHelper = new DataBaseHelper();
            var transactionalDataItems = dbHelper.ExecuteReader(query, sqlDataReader => new TransactionalData
            {
                Id = sqlDataReader.GetInt32(0),
                ReferenceDataId = sqlDataReader.GetInt32(1),
                DataValue = sqlDataReader.GetString(2)
            });

            return transactionalDataItems;
        }
    }
}
