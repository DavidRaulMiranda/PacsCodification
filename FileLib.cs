using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacsCodification
{
    class FileLib
    {
        public event EventHandler<Exception> ErrorEvent;

        RandomLib rnd = new RandomLib();

        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>[ZIP]<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//
        public void ZipejaFitxers(string pathFitxer, string fileName)
        {
            if (Directory.Exists(pathFitxer))
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(pathFitxer);
                /* foreach (FileInfo file in di.EnumerateFiles())
                 {

                 }*/
                var zipFile = pathFitxer + "\\"+fileName+".zip"; //@"C:\data\myzip.zip";
                var files = Directory.GetFiles(pathFitxer);

                using (var archive = ZipFile.Open(zipFile, ZipArchiveMode.Update))
                {
                    foreach (var fPath in files)
                    {
                        archive.CreateEntryFromFile(fPath, Path.GetFileName(fPath));
                    }
                }
                //ProgramMessage?.Invoke(this, "Tasca finalitzada amb éxit!");
            }

        }
        public void DesZipejaFitxers(string pathZip, string pathUnzip)
        {
            ZipFile.ExtractToDirectory(pathZip, pathUnzip);
        }
        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>[FILE OPERATIONS]<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//
        public List<FileStream> FilesInfolder(string directori)
        {
            List<FileStream> files = new List<FileStream>();
            System.IO.DirectoryInfo di = new DirectoryInfo(directori);
            foreach (FileInfo file in di.EnumerateFiles())
            {
                files.Add(GenericFileStream(file.FullName));
            }
            return files;
        }




        public bool NetejaDirectori(string directori)
        {
            bool completed = false;
            try
            {
                if (Directory.Exists(directori))
                {
                    System.IO.DirectoryInfo di = new DirectoryInfo(directori);
                    foreach (FileInfo file in di.EnumerateFiles())
                    {
                        file.Delete();
                    }

                    foreach (DirectoryInfo dir in di.EnumerateDirectories())
                    {
                        dir.Delete(true);
                    }
                }
                else
                {
                    Directory.CreateDirectory(directori);
                }
                completed = true;
                return true;
            }
            catch (Exception ex)
            {
                ErrorEvent?.Invoke(this, ex);
                return false;
            }
        }


     
        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>[FILE IO]<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<//
        public void CloseFilestram(FileStream f )
        {
            f.Close();
        }
        public FileStream GenericFileStream(string filePath)
        {
            FileStream fitxer = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            return fitxer;
        }
        public string[] readFile(FileStream fitxer)
        {
            string line;
            List<string> data= new List<string>();
            StreamReader lector = new StreamReader(fitxer);
            do
            {
                line = lector.ReadLine();
                data.Add(line);
            }
            while (!string.IsNullOrEmpty(line));



            return data.ToArray();
        }
        public void GenericWriteFile(FileStream fitxer, string[] lines)//String filePath,
        {
            //FileStream fitxer = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter escriptor = new StreamWriter(fitxer);
            foreach (string line in lines)
            {
                escriptor.Write(line);
                escriptor.Write("\n");
            }
            escriptor.Write("\r");
            escriptor.Close();
        }
        public void ReplaceCode(FileStream fitxer, string[][]keyAndReplace)  ///////test
        {
            
            int segmentSize = keyAndReplace[0][1].Length;
            int keySize = keyAndReplace[1][0].Length;
            char ignore =' ';
            string filler = "";
            int rw = 0;

            StreamWriter escriptor = new StreamWriter(fitxer);
            StreamReader lector = new StreamReader(fitxer);
            string line;
            string word, replacement;
            long row = 0;
            List<string> result = new List<string>();
            string replz;
            escriptor.BaseStream.Seek(row,SeekOrigin.Begin);
            //lineas en fichero
            do //readPos != '\r' //&& readPos!= '\uffff')  // hacer to char[] mirar el char[0] por el 
            {
                //while
                int z = 0;
                replacement = "";
                // while peeek = true
                //usar un bucle leer caracter a caracter espear /n o /r hacer dowile co n el ultimo de la lista
                //peek para ver al siguiente
                line = lector.ReadLine();
                if (line != null && line != "")
                {
                    for (int i = segmentSize; i <= line.Length; i += segmentSize)
                    {
                        int a = i - segmentSize;
                        word = line.Substring(i - segmentSize, segmentSize);
                        if (keyAndReplace[0].Contains(word))
                        {
                            replacement += keyAndReplace[1][Array.IndexOf(keyAndReplace[0], word)];
                        }
                        z++;
                    }
                }
                if (segmentSize > keySize)
                {

                    replacement += new string(ignore, z * (segmentSize - keySize));
                }
                
                replz = replacement+"\n";
                result.Add(replz);
                escriptor.Flush();
                escriptor.Write(replz);  //escriptor.Write(replz);replz

                escriptor.BaseStream.Seek(row, SeekOrigin.Begin);

                row = row + replz.Length;
                rw++;
                if (rw == 93)
                {
                    int a = 3 + 1;
                }
                    

            } while (!string.IsNullOrEmpty(line));

            lector.Close();
            result.Count();
        }
        public void RevCode(FileStream fitxer, string[][] keyAndReplace)
        {
            int segmentSize = keyAndReplace[0][1].Length;
            int keySize = keyAndReplace[1][0].Length;
            char ignore = ' ';
            string filler = "";

            StreamWriter escriptor = new StreamWriter(fitxer);
            StreamReader lector = new StreamReader(fitxer);
            string line;
            string word, replacement;
            long row = 0;
            List<string> result = new List<string>();
            string replz;
            escriptor.BaseStream.Seek(row, SeekOrigin.Begin);
            //lineas en fichero
            do //readPos != '\r' //&& readPos!= '\uffff')  // hacer to char[] mirar el char[0] por el 
            {
                //while
                int z = 0;
                replacement = "";
                // while peeek = true
                //usar un bucle leer caracter a caracter espear /n o /r hacer dowile co n el ultimo de la lista
                //peek para ver al siguiente
                line = lector.ReadLine();
                if (line != null && line != "")
                {
                    for (int i = segmentSize; i <= line.Length; i += segmentSize)
                    {
                        int a = i - segmentSize;
                        word = line.Substring(i - segmentSize, segmentSize);
                        if (keyAndReplace[0].Contains(word))
                        {
                            replacement += keyAndReplace[1][Array.IndexOf(keyAndReplace[0], word)];
                        }
                        z++;
                    }
                }
                if (segmentSize > keySize)
                {

                    replacement += new string(ignore, z * (segmentSize - keySize));
                }

                replz = replacement + "\n";
                result.Add(replz);
                escriptor.Flush();
                escriptor.Write(replz);  //escriptor.Write(replz);replz

                escriptor.BaseStream.Seek(row, SeekOrigin.Begin);

                row = row + replz.Length;



            } while (!string.IsNullOrEmpty(line));

            lector.Close();
            result.Count();
        }
        public string[] GetDataFromFile(FileStream f)
        {
            List<string> data = new List<string>();
            StreamReader read = new StreamReader(f);
            string line;
            do
            {
                line=read.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    data.Add(line);
                }         
            } while (!string.IsNullOrEmpty(line)); // detect end of file??



            return data.ToArray(); ;
        }
        public bool CompareFileData(FileStream fileOne, FileStream fileTwo)
        {
            string stringOne, stringTwo;
            StreamReader lectorOne = new StreamReader(fileOne);
            StreamReader lectorTwo = new StreamReader(fileTwo);
            do
            {
                
                stringOne = lectorOne.ReadLine();
                stringTwo = lectorTwo.ReadLine();
                if (!stringOne.Equals(stringTwo))
                {
                    return false;
                }
            } while (!string.IsNullOrEmpty(stringOne)); // detect end of file??

            return true;
        }
    }
   

    
}
  /*  public string[] SubstituteAndGet(FileStream fitxer, string[][] keyAndReplace)
    {
        string[] data;




        return data;*/
    //}

