using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;

namespace TP1_INF6404
{
    public partial class SMTPForm : Form
    {
        const string CONNECT = "CONNECT";
        const string HELO    = "HELO";
        const string MAIL    = "MAIL From:";
        const string RCPT    = "RCPT To:";
        const string DATA    = "DATA";
        const string POINT   = ".";
        const string QUIT    = "QUIT";
        const string ENTER   = "\r\n";
        const string SUBJECT = "Subject:";

        string domain;
        
        public SMTPForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
            
            try
            {

                //Connection a smtp.polymtl.ca 25

                domain = textBox4.Text.Substring(textBox4.Text.IndexOf(".")+1 );

                clientSocket.Connect(textBox4.Text, Convert.ToInt32(textBox5.Text));
                msg("Connect " + textBox4.Text + " " + textBox5.Text);

                NetworkStream serverStream = clientSocket.GetStream();
                
                byte[] inStream = new byte[10025];
                
                readSocket(clientSocket, serverStream, inStream);

               //Envoie HELO

                byte[] outStream = System.Text.Encoding.ASCII.GetBytes(HELO + " " + domain + ENTER);
                serverStream.Write(outStream, 0, outStream.Length);
                msg(HELO + " " + domain);

                readSocket(clientSocket, serverStream, inStream);
                
                //Envoie MAIL

                outStream = System.Text.Encoding.ASCII.GetBytes(MAIL+" <" + textBox2.Text + ">" + ENTER);
                serverStream.Write(outStream, 0, outStream.Length);
                msg(MAIL + " <" + textBox2.Text + ">");

                readSocket(clientSocket, serverStream, inStream);
                
                //Envoie RCPT

                outStream = System.Text.Encoding.ASCII.GetBytes(RCPT+" <" + textBox1.Text + ">" + ENTER);
                serverStream.Write(outStream, 0, outStream.Length);
                msg(RCPT + " <" + textBox1.Text + ">");

                readSocket(clientSocket, serverStream, inStream);

                //Envoie CC

                if (textBox6.Text != "" )
 
                {
                    outStream = System.Text.Encoding.ASCII.GetBytes(RCPT + " <" + textBox6.Text + ">" + ENTER);
                    serverStream.Write(outStream, 0, outStream.Length);
                    msg(RCPT + " <" + textBox6.Text + ">");

                    readSocket(clientSocket, serverStream, inStream);
                }
                
                //Envoie DATA

                outStream = System.Text.Encoding.ASCII.GetBytes(DATA + ENTER);
                serverStream.Write(outStream, 0, outStream.Length);
                msg(DATA);

                readSocket(clientSocket, serverStream, inStream);

                //Envoie donnees

                outStream = System.Text.Encoding.ASCII.GetBytes(SUBJECT + " " + textBox3.Text + ENTER);
                serverStream.Write(outStream, 0, outStream.Length);
                msg(SUBJECT + " " + textBox3.Text);

                outStream = System.Text.Encoding.ASCII.GetBytes(richTextBox1.Text + ENTER);
                serverStream.Write(outStream, 0, outStream.Length);
                msg(richTextBox1.Text);

                //Envoie . - la fin de donnees

                outStream = System.Text.Encoding.ASCII.GetBytes(POINT + ENTER);
                serverStream.Write(outStream, 0, outStream.Length);
                msg(POINT);

                readSocket(clientSocket, serverStream, inStream);

                //Envoie QUIT

                outStream = System.Text.Encoding.ASCII.GetBytes(QUIT + ENTER);
                serverStream.Write(outStream, 0, outStream.Length);
                msg(QUIT);

                readSocket(clientSocket, serverStream, inStream);

                serverStream.Close();
                clientSocket.Close();
                
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }


        public void readSocket(TcpClient clientSocket, NetworkStream serverStream, byte[] inStream)
        {
            serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
            string returndata = System.Text.Encoding.ASCII.GetString(inStream);
            msg(returndata);
            clear_inStream(inStream);   
        }
        
        public void msg(string mesg)
        {
            richTextBox3.Text = richTextBox3.Text + Environment.NewLine + mesg + ENTER;
        }

        public void clear_inStream(byte[] inStream)
        {
            for (int i = 0; i < inStream.Length; i++)
            {
                inStream[i] = 0;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            richTextBox3.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
