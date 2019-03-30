using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using TestProject.Utils;

namespace TestProject
{
    public class TransactionDataGenerator
    {
        private readonly Random _random = new Random();

        public Action SuccessCallback { get; private set; }
        public int BatchCount { get; private set; }
        public int RowsInFile { get; private set; }

        public string FolderPath { get; private set; }


        private int _currentFileIndex = 0;
        private int _amountOfActiveThreads = 0;

        private int _amountOfSimultaneousThreads = 8;
        public int AmountOfSimultaneousThreads
        {
            get
            {
                return _amountOfSimultaneousThreads;
            }
            set
            {
                if (value < 1 || value > 8)
                {
                    _amountOfSimultaneousThreads = 1;
                }
            }
        }

        private List<string> _allReferenceDataLookUps = new List<string>();

        public TransactionDataGenerator(int batchCount, int rowsInFile, string folderPath, Action successCallback)
        {
            BatchCount = batchCount;
            RowsInFile = rowsInFile;
            FolderPath = folderPath;

            SuccessCallback = successCallback;
        }

        public void GenerateTransactionData()
        {
            _currentFileIndex = 0;

            var dbHelper = new DataBaseHelper();
            _allReferenceDataLookUps = dbHelper.ExecuteReader("SELECT LookupName FROM ReferenceData").ToList();

            var random = new Random();

            for (int i = 0; i < Math.Min(_amountOfSimultaneousThreads, BatchCount); i++)
            {
                GenerateNextFileIfNeeded();
            }
        }

        private void GenerateNextFileIfNeeded()
        {
            if (_currentFileIndex + 1 < BatchCount)
            {
                _currentFileIndex++;

                string filePath = Path.Combine(FolderPath, $"{_currentFileIndex.ToString()}.txt");

                var thread = new Thread(() => GenerateTransactionDataFile(filePath));
                thread.Start();
            }
            if (_amountOfActiveThreads == 0 && _currentFileIndex >= BatchCount - 1)
            {
                SuccessCallback();
            }
        }

        private void GenerateTransactionDataFile(string filePath)
        {
            _amountOfActiveThreads++;

            using (var streamWriter = new StreamWriter(filePath))
            {
                for (int j = 0; j < RowsInFile; j++)
                {
                    var randomReferenceDataIndex = _random.Next(_allReferenceDataLookUps.Count);
                    var referenceDataLookUp = _allReferenceDataLookUps[randomReferenceDataIndex];

                    var randomString = StringUtils.GenerateRandomString(5);
                    //var randomString = "rando";

                    string nextValue = referenceDataLookUp + ", " + randomString;

                    streamWriter.WriteLine(nextValue);
                }

                _amountOfActiveThreads--;
                GenerateNextFileIfNeeded();
            }
        }
    }
}