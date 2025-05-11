using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApi
{
    public class FileManager
    {
        public static void WriteFile(string path, byte[] text)
        {
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            fs.Write(text, 0, text.Length);
            fs.Dispose();
        }
        public static string ReadFile(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    return sr.ReadToEnd();
                };
            }

        }
    }
}
