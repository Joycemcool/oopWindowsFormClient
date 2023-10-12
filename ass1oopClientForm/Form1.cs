using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace ass1oopClientForm
{
    public partial class Form1 : Form
    {
        public TcpClient tcpClient;
        public TcpListener server;
        public NetworkStream stream;
        public Thread receiveThread;
        String message = string.Empty;
        bool runServer;
        public Form1()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void textBoxMessage_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxConversation_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void textBoxMessage_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void buttonSend_Click_1(object sender, EventArgs e)
        {
            runServer = false;
            if (!runServer)
            {
                message = textBoxMessage.Text;
                RunClient("127.0.0.1", message, this);
                textBoxMessage.Clear();
            }
            else
            {
                AppendToConversationTextBox("Connect first");
            }
        }


        private void AppendToConversationTextBox(string message)
        {
            //if (textBoxConversation.InvokeRequired)
            //{
            //    textBoxConversation.Invoke(new Action<string>(AppendToConversationTextBox), message);
            //}
            //else
            //{
                textBoxConversation.AppendText(message + Environment.NewLine);
            //}
        }

        //When connect start 
        public static void RunServer(int port, TcpListener server, Form1 myForm, String message)
        {

            //server = null;
            //server = new TcpListener(IPAddress.Any, port);
            server = null;
            //myForm.Invoke(new Action(() => myForm.AppendToConversationTextBox("Server started. Waiting for a connection...")));
            try
            {
                // Set the TcpListener on port 13000.
                //Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                myForm.server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                myForm.server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                //String data = null;

                // Enter the listening loop.
                //while (true)
               // {
                    //Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = myForm.server.AcceptTcpClient();//need to start client in another app
                    //Console.WriteLine("Connected!");
                    myForm.AppendToConversationTextBox("Server started. Waiting for a connection...");

                    message = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        message = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        // Console.WriteLine("Received: {0}", message);


                        // Process the data sent by the client.
                        message = message.ToUpper();

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);

                        //Console.WriteLine("Sent: {0}", data); put the function in event listener
                        myForm.AppendToConversationTextBox(message);

                    }

                    // Shutdown and end connection
                    client.Close();
                //}
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            //finally
            //{
            //    // Stop listening for new clients.
            //    server.Stop();
            //}


            //Console.WriteLine("\nHit enter to continue...");
            //Console.Read();
        }

        public static void RunClient(String server, String message, Form1 myForm)
        {
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                Int32 port = 13000;
                TcpClient client = new TcpClient(server, port);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                //Console.WriteLine("Sent: {0}", message);
                myForm.AppendToConversationTextBox(message);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                //Console.WriteLine("Received: {0}", responseData);
                myForm.AppendToConversationTextBox(responseData);

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

           // Console.WriteLine("\n Press Enter to continue...");
            //Console.Read();
        }

        private void toolStripTextBoxConnect_Click(object sender, EventArgs e)
        {
            //RunClient("127.0.0.1", message,this);
            runServer = false;
            AppendToConversationTextBox("Client started. Waiting for a connection...");

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void toolStripTextBoxExit_Click(object sender, EventArgs e)
        {
            Application.Exit(); 
        }

        private void toolStripTextBoxDicsonnect_Click(object sender, EventArgs e)
        {
            runServer = false;
            //server.Stop();
        }
    }//end class
}//end namespace
