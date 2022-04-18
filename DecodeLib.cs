using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace PacsCodification
{
    class DecodeLib
    {
        FileLib fl = new FileLib();
        private Thread unzip;
        private Thread decode;
        private Thread compose;

        private ConcurrentDictionary<string, string[]> solution;

        public event EventHandler<Exception> ErrorEvent;
        public event EventHandler<bool> FicherRdy;

        public void DecodeFile2(string pathRestul, string pathZip, string zipName, string pathUnzip , string[][] code)
        {

            solution = new ConcurrentDictionary<string, string[]>();
            List<FileStream> files = new List<FileStream>();
            
            unzip=new Thread(() => UnnZipFile(pathZip, zipName, pathUnzip, files));
            decode = new Thread(() => DecodeFile(files,code,pathUnzip));
            compose = new Thread(() => MergeSolution(pathRestul));
            unzip.Start();
            decode.Start();
            compose.Start();


        }
        private void UnnZipFile(string pathzIP, string filename, string pathUnzip, List<FileStream> files)
        {
            fl.NetejaDirectori(pathUnzip);
            fl.DesZipejaFitxers(pathzIP+"\\"+filename+".zip", pathUnzip);
            files = fl.FilesInfolder(pathUnzip);
        }
        private void DecodeFile(List<FileStream> files, string[][] code, string pathUnzip)
        {
            unzip.Join();
            files = fl.FilesInfolder(pathUnzip);
            string[] coded;
            string[] uncoded;
            string filename;
            string pattern = "\\d*$";
            Regex rx = new Regex(pattern);
            /*Parallel.For(0, files.Count, (i) =>
            {
                filename = files[i].Name.Substring(files[i].Name.Length - 1, 1);
                coded = fl.GetDataFromFile(files[i]);
                uncoded = DecodeData(coded,code, filename);
           
                //solution.TryAdd(filename, uncoded);

                
            });*/
            foreach (FileStream f in files)
            {
                Match m = Regex.Match(f.Name, pattern, RegexOptions.IgnoreCase);

               
                filename = m.Value;
               
               
                //filename = f.Name.Substring(f.Name.Length - 1, 1);
                coded = fl.GetDataFromFile(f);
                uncoded = DecodeData(coded, code, filename);

                solution.TryAdd(filename, uncoded);
            }

            int a = solution.Count();
            string b = solution.Keys.ToString();
        }
        private void MergeSolution(string pathSolution)
        {
            decode.Join();
            string[] print = new string[0];
            string[] data;
            
            
            for (int i = 1; i <= solution.Count; i++)
            {
                if (solution.ContainsKey(i.ToString()))
                {
                    data = solution[i.ToString()];
                    print = print.Concat(data).ToArray();
                }
                
            }
            FileStream f = fl.GenericFileStream(pathSolution + "\\Solved");
            fl.GenericWriteFile(f, print);
            f.Close();
            FicherRdy?.Invoke(this, true);

        }
  
        private string[] DecodeData(string[]data, string[][] code, string filename)
        {
            try
            {
                int cedSize = code[1][0].Length;
                string decoded;
                string word;
                for (int i = 0; i < data.Length; i++)
                {
                    decoded = "";
                    //while(data[i].Length>= cedSize)
                    for (int j = 0; j+cedSize <= data[i].Length; j += cedSize)
                    {

                        word = data[i].Substring(j , cedSize);

                        if (code[1].Contains(word))
                        {
                            decoded = decoded + code[0][Array.IndexOf(code[1], word)];
                        }

                    }
                    data[i] = decoded;
                }
            }
            catch (Exception ex)
            {
                ErrorEvent?.Invoke(this, ex);
               
            }

            //solution.TryAdd(filename, data);
            return data;
        }
    }
}



        /*
        public void DecodeFile(string path,string[][]codeKey)
        {
            
            
            string[][] codeKeyFlip = new string[][] { new string[codeKey[0][0].Length], new string[codeKey[0][0].Length] };
            codeKeyFlip[0] = codeKey[1];
            codeKeyFlip[1] = codeKey[0];
            codeKey = codeKeyFlip;

            FileStream v = fl.GenericFileStream(path);
            fl.ReplaceCode(v, codeKeyFlip);
            fl.CloseFilestram(v);


            // ddecodifica fitxer
            
            FileStream v = fl.GenericFileStream(path);
            fl.GenericWriteFile();

        }*/
