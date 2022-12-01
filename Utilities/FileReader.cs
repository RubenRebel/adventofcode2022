using System;
namespace AdventofCode2022
{
    public class FileReader
    {
        public FileReader()
        { }

        public string ConvertFileContentToString(string fileLocation)
        {
            try
            {
                using (var sr = new StreamReader(fileLocation))
                {
                   return sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"The file could not be read: {ex.Message}");
                return "";
            }
        }
    }
}

