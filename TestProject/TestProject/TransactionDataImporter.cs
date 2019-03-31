using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TestProject.Models;
using TestProject.Utils;

namespace TestProject
{
    public class TransactionDataImporter
    {
        private Dictionary<string, ReferenceData> _allReferenceDataLookUps = new Dictionary<string, ReferenceData>();

        public void Import(string folderPath, bool removeFilesAfterImport = true)
        {
            var dbHelper = new DataBaseHelper();
            _allReferenceDataLookUps =
                dbHelper.ExecuteReader("SELECT * FROM ReferenceData", sqlDataReader => new ReferenceData
                {
                    Id = sqlDataReader.GetInt32(0),
                    LookupName = sqlDataReader.GetString(1)
                }).ToDictionary(x => x.LookupName);

            var files = Directory.GetFiles(folderPath);

            Parallel.ForEach(files, x => PrepareFileForImport(x));

            files = Directory.GetFiles(folderPath);

            foreach (var file in files)
            {
                dbHelper.ExecuteNonQuery($"exec sp_ImportTransactionalData @passToFileWithData = '{file}'");
                File.Delete(file);
            }
        }

        private void PrepareFileForImport(string file)
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

                    var lookUpId = _allReferenceDataLookUps[lookupName].Id;
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
    }
}
