using System;
using System.IO;
using System.Threading.Tasks;

namespace ToUpper
{
    class Program
    {
        private const string _infilesFolder = "infiles";
        private const string _outfilesFolder = "outfiles";
        private const string _infilenameBasis = _infilesFolder + "\\splitfile";
        private const string _outfilenameBasis = _outfilesFolder + "\\UPPERsplitfile";
        private const string _fileExtension = ".txt";
        private bool _jobIsRunning = false;

        static void Main(string[] args)
        {
            Program prog = new Program();
            prog.MainThread();
        }

        private void MainThread()
        {
            int fileCount = Directory.GetFiles(_infilesFolder).Length;
            Console.WriteLine($"Starting job - {fileCount} files");
            Job(fileCount);
            CountSheep();
            Console.WriteLine("Job is done");
            Console.ReadKey();
        }



        async public Task Job(int fileCount)
        {
            _jobIsRunning = true;
            for (int i = 1; i <= fileCount; i++)
            {
                FileInfo fi = new FileInfo(_infilenameBasis + i + _fileExtension);
                int fileSize = (int)fi.Length;
                char[] charbuf = new char[fileSize];

                // Create StreamReader and StreamWriter so the method runs asynchronously
                // as soon as the StreamWriter is created.
                // Read file into charbuf, convert to upper case, and write using the
                // StreamWriter exactly as before
                using (StreamReader reader = new StreamReader(fi.OpenRead()))
                using (StreamWriter writer = new StreamWriter(_outfilenameBasis + i + _fileExtension))
                {
                    string content = await reader.ReadToEndAsync();
                    await writer.WriteAsync(content.ToUpper());
                }
            }
            _jobIsRunning = false;
        }

        private void CountSheep()
        {
            int i = 1;
            while (_jobIsRunning)
            {
                Console.WriteLine(i + " sheep");
                i++;
            }
        }
    }
}
