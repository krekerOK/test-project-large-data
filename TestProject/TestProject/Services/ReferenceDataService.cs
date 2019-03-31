using System.IO;
using System.Linq;
using TestProject.Utils;

namespace TestProject.Services
{
    public class ReferenceDataService
    {
        public void Generate(int amountOfRows = 1_000_000)
        {
            var tempFilePath = @"C:\Data\t.csv";
            GenerateCSVFileWithReferenceData(tempFilePath, amountOfRows);

            var databaseHelper = new DataBaseHelper();
            databaseHelper.ExecuteNonQuery($" exec sp_GenerateReferenceData @passToFileWithData = '{tempFilePath}'");
        }

        private void GenerateCSVFileWithReferenceData(string filePath, int amountOfRows)
        {
            var latinAlphabetForCSV = new string[] { ",A_", ",B_", ",C_", ",D_", ",E_", ",F_",
                                                     ",G_", ",H_", ",I_", ",J_", ",K_", ",L_",
                                                     ",M_", ",N_", ",O_", ",P_", ",Q_", ",R_",
                                                     ",S_", ",T_", ",U_", ",V_", ",W_", ",X_",
                                                     ",Y_", ",Z_" };

            var amountOfCharacters = latinAlphabetForCSV.Count();

            int amountOfIterations = (amountOfRows / amountOfCharacters) + 1;

            using (var streamWriter = new StreamWriter(filePath))
            {
                for (int i = 0; i < amountOfIterations; i++)
                {
                    for (int j = 0; j < amountOfCharacters; j++)
                    {
                        string nextValue = latinAlphabetForCSV[j] + i.ToString();
                        streamWriter.WriteLine(nextValue);
                    }
                }
            }
        }
    }
}
