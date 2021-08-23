using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Quobject.SocketIoClientDotNet.Client;
using Newtonsoft.Json;

namespace WindowsFormsApp3
{
    public delegate void UpdateTextBoxMethod(string text);
    public partial class Form1 : Form
    {
        public class socketData
        {
            public static Socket socket = IO.Socket("http://localhost");
        }
 
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            this.comboBox1.SelectedIndex = 0;
            socketData.socket.Connect();
            socketData.socket.On(Socket.EVENT_CONNECT, () =>
            {
                this.label2.Text = "Conectado";
               /* Se puede subscribir a mas conexiones
                socket.On(bascula, (data) =>
                {
                  ...data
                }
                */
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string bascula = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);
            // Se selecciona la bascula para tomar lectura
            socketData.socket.On(bascula, (data) =>
            {
                UpdateValor(JsonConvert.SerializeObject(data));
                //socket.Disconnect(); // Es utilizado para cerrar la conexion con el socket
            });

        }

        private void UpdateValor(string text)
        {
            if (this.textBox1.InvokeRequired)
            {
                UpdateTextBoxMethod del = new UpdateTextBoxMethod(UpdateValor);
                this.Invoke(del, new object[] { text });
            }
            else
            {
                this.textBox1.Text = text;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            socketData.socket.Off();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            socketData.socket.Disconnect();
            socketData.socket.Close();
            this.label2.Text = "Desconectado";
            socketData.socket = null;
        }
    }
}
