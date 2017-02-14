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
using System.Threading;

namespace PrimeiroServidorC
{
    public partial class Form1 : Form
    {
        private TcpClient client;
        public StreamReader STR;
        public StreamWriter STW;
        public string receive;
        public String text_to_send;
        public string nick;

        public Form1()
        {
            InitializeComponent();


            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName());         // saber o meu proprio IP

            foreach(IPAddress address in localIP)
            {
                if(address.AddressFamily == AddressFamily.InterNetwork)
                {
                    textBox3.Text = address.ToString();

                }

            }

        }

        private void button2_Click(object sender, EventArgs e)                    // Ligar o Servidor
        {
            nick = textBox7.Text;
            backgroundWorker3.RunWorkerAsync();

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) // Receber Data
        {
            while(client.Connected)
            {
                try
                {
                    receive = STR.ReadLine();
                    this.textBox2.Invoke(new MethodInvoker(delegate () { textBox2.AppendText(receive + "\n"); }));
                    receive = "";
                }
                catch(Exception x)
                {
                    MessageBox.Show(x.Message.ToString());
                }
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)  // Enviar Data
        {

            if(client.Connected)
            {
                STW.WriteLine(nick + ": " + text_to_send);
                this.textBox2.Invoke(new MethodInvoker(delegate () { textBox2.AppendText(nick + ": " + text_to_send + "\n"); }));
                
            }
            else
            {
                MessageBox.Show("Envio Falhado!");
            }
        }

        private void button3_Click(object sender, EventArgs e)                   // Conectar ao Servidor
        {
            nick = textBox7.Text;
            client = new TcpClient();
            IPEndPoint IP_End = new IPEndPoint(IPAddress.Parse(textBox5.Text), int.Parse(textBox6.Text));

                try
            {
                client.Connect(IP_End);
                if(client.Connected)
                {
                    textBox2.AppendText("Connected to Server" + "\n");
                    STR = new StreamReader(client.GetStream());
                    STW = new StreamWriter(client.GetStream());
                    STW.AutoFlush = true;

                    STW.WriteLine(nick + " juntou-se ao chat!");

                    backgroundWorker1.RunWorkerAsync();                                  // Começar a receber Data em background
                    backgroundWorker2.WorkerSupportsCancellation = true;                 // Abilidade para cancelar this thread


                }
            }
            catch(Exception x)
            {
                MessageBox.Show(x.Message.ToString());
            }


        }

        private void button1_Click(object sender, EventArgs e)            // Botao de Enviar
        {
            if(textBox1.Text != "")
            {
                text_to_send = textBox1.Text;
                backgroundWorker2.RunWorkerAsync();
            }

            textBox1.Text = "";
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e) {
            TcpListener listener = new TcpListener(IPAddress.Any, int.Parse(textBox4.Text));
            listener.Start();
            while (true) {
                if (listener.Pending()) {
                    client = listener.AcceptTcpClient();
                    break;
                }
            }

            STR = new StreamReader(client.GetStream());
            STW = new StreamWriter(client.GetStream());
            STW.AutoFlush = true;

            backgroundWorker1.RunWorkerAsync();                                  // Começar a receber Data em background
            backgroundWorker2.WorkerSupportsCancellation = true;                 // Abilidade para cancelar this thread

        }

    }
}
