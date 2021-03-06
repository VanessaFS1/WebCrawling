using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebCrawling
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void AtualizarButton_Click(object sender, EventArgs e)
        {
            var wc = new WebClient();
            string palavraDigitada = textBox1.Text.Replace(" ", "_");
            string pagina = wc.DownloadString($"https://dicionariocriativo.com.br/{palavraDigitada}");

            var htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(pagina);

            dataGridView1.Rows.Clear();

            string palavra = string.Empty;
            string significados = string.Empty;
            List<string> relacionadas = new List<string>();
            string sinonimos = string.Empty;

            var sections = htmlDocument.DocumentNode.Descendants("section").Where(node => node.GetAttributeValue("id", "").Equals("mainContent")).ToList();
            foreach (var section in sections)
            {
                 var sects = htmlDocument.DocumentNode.Descendants("section").Where(node => node.GetAttributeValue("id", "").Equals("significado")).ToList();

                foreach (var sec in sects)
                {

                    significados = sec.Descendants("a").FirstOrDefault(x => x.Attributes["class"].Value.Equals("c_primary_hover")).InnerText.Replace("\n", "");
                    dataGridView1.Rows.Add(significados);
                }

                var sectis = htmlDocument.DocumentNode.Descendants("section").Where(node => node.GetAttributeValue("id", "").Equals("sinant")).ToList();
                foreach (var sec in sectis)
                {
                    var lis = sec.Descendants("li").ToList();
                    foreach (var s in lis)
                    {
                        sinonimos = s.Descendants("a").FirstOrDefault(x => x.Attributes["class"].Value.Equals("c_primary_hover")).InnerText.Replace("\n", "");
                        dataGridView1.Rows.Add("", sinonimos);
                    }
                }

                var sect = htmlDocument.DocumentNode.Descendants("section").Where(node => node.GetAttributeValue("id", "").Equals("analogico")).ToList();
                foreach (var sec in sect)
                {
                    var lis = sec.Descendants("li").ToList();
                    foreach (var l in lis)
                    {
                        var rel = l.Descendants("a").FirstOrDefault(x => x.Attributes["class"].Value.Equals("c_primary_hover c_border_hover analogico")).InnerText.Replace("\n", "");
                        relacionadas.Add(rel);
                        dataGridView1.Rows.Add("", "", rel);
                    }
                    
                }

                

            }

        }
    }
}
