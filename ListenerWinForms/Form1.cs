using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;

namespace ListenerWinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Listener.events.RemoteEventError += (esender, ex) => lblStatus.Text = $"ERRO: {ex.Error}";
            Listener.events.RemoteEventCounts += (esender, ex) =>
            {
                if (ex.Name == "ATUALIZAPROGRESSO")
                {
                    progressBar.Invoke((MethodInvoker)delegate
                    {
                        progressBar.Increment(1);
                    });
                    
                };

                if (ex.Name == "PROCESSOCONCLUIDO")
                {
                    lblStatus.Invoke((MethodInvoker)delegate
                    {
                        lblStatus.Text = "Processo Concluído";
                        lblStatus.Visible = true;
                    });

                };
                ;
            };
        }    
    }
}
