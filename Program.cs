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
        private int _sheepCount = 0;

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
            Console.WriteLine("Job is done");
            Console.ReadKey();
        }

        public void Job(int fileCount)
        {
            for (int i = 1; i <= fileCount; i++)
            {
                int fileNumber = i; // Capture the current file number

                Task.Factory.StartNew(() =>
                {
                    string inputFile = Path.Combine(_infilesFolder, $"{_infilenameBasis}{fileNumber}{_fileExtension}");
                    string outputFile = Path.Combine(_outfilesFolder, $"{_outfilenameBasis}{fileNumber}{_fileExtension}");

                    char[] charbuf;

                    using (StreamReader sr = File.OpenText(inputFile))
                    {
                        int fileSize = (int)new FileInfo(inputFile).Length;
                        charbuf = new char[fileSize];
                        sr.Read(charbuf, 0, fileSize);
                    }

                    // Convert the content to uppercase
                    for (int j = 0; j < charbuf.Length; j++)
                    {
                        charbuf[j] = Char.ToUpper(charbuf[j]);
                    }

                    // Write charbuf to the output file
                    File.WriteAllText(outputFile, new string(charbuf));

                    // Call CountSheep to update the count
                    CountSheep();
                });
            }
        }

        private void CountSheep()
        {
            _sheepCount++;
            Console.WriteLine($"{_sheepCount} sheep");
        }
    }
}
