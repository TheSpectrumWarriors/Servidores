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
        public List<TcpClient> listClients;
        public List<StreamReader> listSTR;
        public List<StreamWriter> listSTW;
        static public string receive;
        public String text_to_send;
        public string nick;
        public static TextBox textBox;

        public Form1() {
            InitializeComponent();

            listClients = new List<TcpClient>();
            listSTR = new List<StreamReader>();
            listSTW = new List<StreamWriter>();


            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName());         // saber o meu proprio IP

            foreach (IPAddress address in localIP) {
                if (address.AddressFamily == AddressFamily.InterNetwork) {
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
                    listClients.Add(client);
                    break;
                }
            }

            STR = new StreamReader(client.GetStream());
            STW = new StreamWriter(client.GetStream());
            listSTR.Add(STR);
            listSTW.Add(STW);
            STW.AutoFlush = true;

            Thread threadClient = new Thread(ClientThread);
            threadClient.Start();
            // Começar a receber Data em background
            backgroundWorker2.WorkerSupportsCancellation = true;                 // Abilidade para cancelar this thread

        }

        private void ClientThread() {
            int i = listClients.Count -1;

            while (listClients[i].Connected) {
                try {
                    Form1.receive = listSTR[i].ReadLine();
                    textBox2.Invoke(new MethodInvoker(delegate () { textBox2.AppendText(Form1.receive + "\n"); }));  //ERRRRRRROOOOO
                    Form1.receive = "";
                } catch (Exception x) {
                    MessageBox.Show(x.Message.ToString());
                }
            }

        }

    }

    public class ClientClass {

        private TcpClient client;
        public StreamReader STR;
        public StreamWriter STW;

        ClientClass(TcpClient client, StreamReader input, StreamWriter output) {

            this.client = client;
            this.STR = input;
            this.STW = output;

            Thread threadClient = new Thread(Run);
            threadClient.Start();

        }

        private void Run() {
            
        }
    }

}

    
