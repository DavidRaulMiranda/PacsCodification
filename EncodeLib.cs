using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace PacsCodification
{
    class EncodeLib
    {
        public event EventHandler<Exception> ErrorEvent;
        public event EventHandler<bool> FicherRdy;
        public event EventHandler<bool> Solucio;


        private const string pathUncoded = "";
        private const string pathCoded = "";

        FileLib fl = new FileLib();
        RandomLib rnd = new RandomLib();

        
        private Thread original;
        private Thread genera;
        private Thread zipeja;

        private Thread Resol;

        private Dictionary<int, string[]> dataCompara;

        ///falta event  pilla error de  filelib
        ///

        public void GeneraRandom(string[][] codeKey, int numFitxers, int numNodes, int lines, string nomFitxer, string directoriRandom, string directoriCodifica, string directoriSolved)
        {
            dataCompara = new Dictionary<int, string[]>();
            try
            {
                
                if (fl.NetejaDirectori(directoriCodifica) && fl.NetejaDirectori(directoriRandom) &&   fl.NetejaDirectori(directoriSolved))// neteja directori 
                {
                    genera = new Thread(() => GeneraFitxers(numFitxers, numNodes, lines, directoriRandom, directoriCodifica, nomFitxer, codeKey));
                    genera.Start();



                    zipeja = new Thread(() => ZipejaFitxers(directoriCodifica,nomFitxer));
                    zipeja.Start();
                    // genera fitxer comparació un cop s'ha zipejar y es llança l'even de envia fitxers
                    original = new Thread(() => FitxerPerComparar(directoriRandom, dataCompara));
                    original.Start();
                    //zipeja
                    // copia a nou directori
                    // codifica
                    // ajunta
                    //zipeja = new Thread(ZipejaFitxers);
                }
            }
            catch (Exception ex)
            {
                ErrorEvent?.Invoke(this, ex);
            }



            /*
            for (int j = 0; j < nombreVocals; j++)
            {
                a = GeneraRandom(0, vocals.Length - 1);
                seed = seed + vocals[a];
            }

            data[0] = seed;//+"\n"
            data[1] = "\r";
            FileStream fitxer = GenericFileStream(filePath);
            GenericWriteFile(fitxer, data);
            fitxer = GenericFileStream(filePath);
            ReplaceCode(fitxer);*/

            int a = dataCompara.Count();

        }
        private void ZipejaFitxers(string directori, string name)
        {
            genera.Join();
           fl.ZipejaFitxers(directori, name+"_All");
           FicherRdy?.Invoke(this, true);
        }
        private void FitxerPerComparar(string filePath, Dictionary<int, string[]> data) 
        {
            genera.Join();
            int lines = (data[0].Length * data.Count);
            string[] dataTot =new  string[lines];
            dataTot = data[0];
            for (int i = 1; i < data.Count; i++)
            {
                dataTot = dataTot.Concat(data[i]).ToArray();
            }
            string a = dataTot[dataTot.Length-1];
            var filestreamComprara = fl.GenericFileStream(filePath+"\\source");
            EscriuFitxer(filestreamComprara, dataTot);
            fl.CloseFilestram(filestreamComprara);
             a = dataTot[dataTot.Length-1];

        }
        public void compara(string fitxerOriginal, string FitxerResposta)
        {
            Resol = new Thread(() => CompraraFitxers(fitxerOriginal, FitxerResposta));
            Resol.Start();
        }
        private void GeneraFitxers(int numFitxers, int numNodes,int lines, string directoriCompara, string directoriCodifica, string nomFitxer, string[][] keyAndReplace)
        {
            Parallel.For(0, numFitxers, (i) =>
            {
                string[] data = GeneraRandom(numNodes, lines, keyAndReplace[0]);
                // addd data to dictionary

                dataCompara[i] = data;
                /*
                string filePathCompra = directoriCompara + "\\" + nomFitxer + (i + 1).ToString();
                var filestreamComprara=fl.GenericFileStream(filePathCompra);*/

                string filePathCodifica = directoriCodifica+ "\\" + nomFitxer + (i + 1).ToString();
                var filestreamCodifica = fl.GenericFileStream(filePathCodifica);

           
                EscriuFitxer(filestreamCodifica, data);
                fl.CloseFilestram(filestreamCodifica);


                 
                 filestreamCodifica = fl.GenericFileStream(filePathCodifica);

                Codifica(filestreamCodifica, keyAndReplace);
                //fl.CloseFilestram(filestreamCodifica);
            });
            //for (int i = 0; i < numFitxers; i++)
            //{
            //    string[] data = GeneraRandom(numNodes, lines, keyAndReplace[0]);
            //    // addd data to dictionary

            //    dataCompara[i] = data;
            //    /*
            //    //string filePathCompra = directoriCompara + "\\" + nomFitxer + (i + 1).ToString();
            //    var filestreamComprara=fl.GenericFileStream(filePathCompra);*/

            //    string filePathCodifica = directoriCodifica + "\\" + nomFitxer + (i + 1).ToString();
            //    var filestreamCodifica = fl.GenericFileStream(filePathCodifica);


            //    EscriuFitxer(filestreamCodifica, data);
            //    fl.CloseFilestram(filestreamCodifica);



            //    filestreamCodifica = fl.GenericFileStream(filePathCodifica);

            //    Codifica(filestreamCodifica, keyAndReplace);
            //}
            

        }
        private string[] GeneraRandom(int numNodes, int lines, string[] nodes)
        {
            //int seed = rnd.RandomSeed();
            
            int a;
            string line;
            string[] data = new string[lines];
            for (int i = 0; i < lines; i++)
            {
                line = "";
                for (int j = 0; j < numNodes; j++)
                {
                    a = rnd.GeneraRandom(0, nodes.Length - 1);
                    line = line + nodes[a];
                }
                data[i] = line;

            }/////////fallo  ndex out of range
            //data[data.Length] = data[data.Length] + "\r";
            return data;
        }
        private void EscriuFitxer(FileStream filestream,string [] data)
        {
            fl.GenericWriteFile(filestream, data);
        }
        private void Codifica(FileStream filestream, string[][] keyAndReplace)
        {
            fl.ReplaceCode(filestream, keyAndReplace);
        }
        private void CompraraFitxers(string fitxerOriginal, string FitxerResposta)
        {
            bool resultat;
            FileStream original = fl.GenericFileStream(fitxerOriginal+"\\source");
            FileStream resposta = fl.GenericFileStream(FitxerResposta+"\\Solved");
            resultat=fl.CompareFileData( original,  resposta);
            Solucio?.Invoke(this, resultat);
        }
        
    }
}
