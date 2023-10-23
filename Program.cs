using System;
using System.IO;
using System.Threading.Tasks;

namespace ToUpper
{
    class Program
    {
        private const string _infilesFolder = "infiles";
        private const string _outfilesFolder = "outfiles";
        private const string _infilenameBasis = "splitfile";
        private const string _outfilenameBasis = "UPPERsplitfile";
        private const string _fileExtension = ".txt";
        private bool _jobIsRunning = default;
        private int _sheepCount = 0;

        static async Task Main(string[] args)
        {
            Program prog = new Program();
            await prog.MainThread();
        }

        private async Task MainThread()
        {
            int fileCount = Directory.GetFiles(_infilesFolder).Length;
            Console.WriteLine($"Starting job - {fileCount} files");
            await Job(fileCount);
            CountSheep();
            Console.WriteLine("Job is done");
            Console.ReadKey();
        }

        public async Task Job(int fileCount)
        {
            _jobIsRunning = true;

            for (int i = 1; i <= fileCount; i++)
            {
                string inputFile = Path.Combine(_infilesFolder, $"{_infilenameBasis}{i}{_fileExtension}");
                string outputFile = Path.Combine(_outfilesFolder, $"{_outfilenameBasis}{i}{_fileExtension}");

                char[] charbuf;

                using (StreamReader sr = File.OpenText(inputFile))
                {
                    int fileSize = (int)new FileInfo(inputFile).Length;
                    charbuf = new char[fileSize];
                    await sr.ReadAsync(charbuf, 0, fileSize);
                }

                // Convert the content to uppercase
                for (int j = 0; j < charbuf.Length; j++)
                {
                    charbuf[j] = Char.ToUpper(charbuf[j]);
                }

                // Write charbuf to the output file
                await File.WriteAllTextAsync(outputFile, new string(charbuf));
                _sheepCount++;
                CountSheep();

            }
            
            _jobIsRunning = false;
        }
        private void CountSheep()
        {
            if (_sheepCount <= 65)
            {
                Console.WriteLine(_sheepCount + " sheep");
            }
        }
    }
}
