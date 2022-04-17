using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PacsCodification
{
    public partial class Form1 : Form
    {

        private string FileName = "PACS";
        private string ZpName = "PACS_all";
        private readonly string pathCoded = Environment.CurrentDirectory + "\\Resources\\Server\\Coded";
        private readonly string pathOriginal = Environment.CurrentDirectory + "\\Resources\\Server\\Original";
        private readonly string pathZip = Environment.CurrentDirectory + "\\Resources\\Server\\Zipped";
        private readonly string pathUnzip = Environment.CurrentDirectory + "\\Resources\\Client\\unzip";
        private readonly string pathSolved = Environment.CurrentDirectory + "\\Resources\\Server\\Solved";
        public Form1()
        {
            InitializeComponent();
            enc.FicherRdy += PopSendRdy;
            enc.Solucio += PopCompara;
            dec.FicherRdy+= PopSendRdy;
        }
        Dictionary<int, string[]> data;
        string[][] codeKey;
        CodifyLib code = new CodifyLib();
        EncodeLib enc = new EncodeLib();
        DecodeLib dec = new DecodeLib();
        FileLib fl = new FileLib();
        private void PopSendRdy(object sender, bool sts)
        {
            MessageBox.Show("file rdy send");
        }
        private void PopCompara(object sender, bool sts)
        {
            MessageBox.Show("compare result = " +sts.ToString());
        }


        private void btnGeneraCodi_Click(object sender, EventArgs e)
        {
           codeKey= code.GeneraCodificacio();
        }

        private void btn_codifica_Click(object sender, EventArgs e)
        {
            fl.NetejaDirectori(pathUnzip);
            enc.GeneraRandom(codeKey, 3, 10, 100, FileName, pathOriginal, pathCoded , pathSolved); //files, col, row

        }

        private void button1_Click(object sender, EventArgs e)
        {
            enc.compara(pathOriginal, pathSolved);
        }
        /// client
        private void btnDecodifica_Click(object sender, EventArgs e)
        {
            dec.DecodeFile2(pathSolved,pathCoded, ZpName, pathUnzip, codeKey);
        }
    }
}
