using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duplicate_File_Finder
{
    class Program
    {
        private static Dictionary<string, List<FileAttr>> fileInfoDict;
        private static FileStream fs;
        private static StreamWriter sw;

        static void Main(string[] args)
        {
            fileInfoDict = new Dictionary<string, List<FileAttr>>();
            //DirectoryInfo TheDir = new DirectoryInfo(@"E:\Good Old Dell\");
            DirectoryInfo TheDir = new DirectoryInfo(@"F:\Documents");

            fs = new FileStream("Result.txt", FileMode.CreateNew);
            sw = new StreamWriter(fs);

            ProcTheDir(TheDir);

            sw.Flush();
            sw.Close();
            fs.Close();
        }

        static void ProcTheDir(DirectoryInfo dirInfo)
        {
            // Debugging
            Console.WriteLine("Entering Directory: " + dirInfo.FullName);
            //sw.WriteLine("Entering Directory: " + dirInfo.FullName);

            try
            {
                var childDirs = dirInfo.GetDirectories();

                foreach (var theDir in childDirs)
                {
                    ProcTheDir(theDir);
                    ProcFilesInDir(theDir);
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e);
            }
        }

        static void ProcFilesInDir(DirectoryInfo dirInfo)
        {
            var filesInDir = dirInfo.GetFiles();

            foreach (var file in filesInDir)
            {
                // Debugging

                if (fileInfoDict.ContainsKey(file.Name))
                {
                    fileInfoDict[file.Name].Add(new FileAttr(file.FullName, "", file.Length));

                    // Debugging
                    Console.WriteLine("- Now File: " + file.FullName + " - DUPLICATE FOUND!!!");
                    sw.WriteLine(file.FullName);
                }
                else
                {
                    fileInfoDict.Add(file.Name, new List<FileAttr> { new FileAttr(file.FullName, "", file.Length) });

                    // Debugging
                    Console.WriteLine("- Now File: " + file.FullName);
                }
            }
        }

        class FileAttr
        {
            string filePath = "";
            string fileHash = "";
            long fileSize = -1;

            public FileAttr(string path, string hash, long size)
            {
                filePath = path;
                fileHash = hash;
                fileSize = size;
            }
        }
    }
}
