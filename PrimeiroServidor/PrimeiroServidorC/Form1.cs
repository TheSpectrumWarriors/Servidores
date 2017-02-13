using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
using System.IO;

namespace PrimeiroServidorC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName()); // saber o meu proprio IP

            foreach(IPAddress address in localIP)
            {
                if(address.AddressFamily == AddressFamily.InterNetwork)
                {
                    textBox3.Text = address.ToString();

                }

            }


        }

        private void button2_Click(object sender, EventArgs e)    // Ligar o Servidor
        {



        }
    }
}
