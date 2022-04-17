using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacsCodification
{
    class CodifyLib
    {
        RandomLib rnd = new RandomLib();

        List<int> arr = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        private  char[] vocals = new char[] { 'a', 'e', 'i', 'o', 'u' };
        private string[][] codeKey;
        public string[][] GeneraCodificacio()
        {
            codeKey  =new string[][] { new string[5],new string[5]};
            char[] rndChar;
            List<string> usedCode = new List<string>();


            rndChar = RandomizaLletres(vocals);
            for (int i = 0; i < rndChar.Length; i++)
            {
                codeKey[0][i] = rndChar[i].ToString();
                codeKey[1][i] = CodiCaracter(usedCode);
            }

            return codeKey;
        }
        private char[] RandomizaLletres(char[]caracters)
        {
            List<char> randomized = new List<char>();
            List<char> caractersList = caracters.ToList();
            int index;
            while (caractersList.Count > 0)
            {

                index = rnd.GeneraRandom(0, caractersList.Count - 1);
                randomized.Add(caractersList[index]);
                caractersList.RemoveAt(index);
            }
            return randomized.ToArray();
        }
        private string CodiCaracter(List<string>usedCodes)
        {
            string code;
            List<int> values;
            Queue<int> code_q;
            int seed = rnd.RandomSeed();
            int index;
            do
            {
                code = "";
                values = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                code_q = new Queue<int>();

                while (values.Count > 0)
                {
                    index = rnd.GeneraRandom(0, values.Count - 1, seed);
                    code_q.Enqueue(values[index]);
                    values.RemoveAt(index);
                }
                
                code =string.Join("", code_q.ToArray());

            } while (usedCodes.Contains(code));
            usedCodes.Add(code);
            return code;
        }

    }
}
