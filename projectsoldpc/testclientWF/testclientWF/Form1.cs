
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testclientWF
{
    public partial class Form1 : Form
    {
        HubConnection connection;
        public Form1()
        {
            InitializeComponent();
            connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:59091")
                .Build();

            //

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };
        }

        private  void Send_Click(object sender, EventArgs e)
        {
            Send();
        }
        public async void Send()
        {
            try
            {
                await connection.InvokeAsync("Test",
                    textBox1.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public async void Connect()
        {
          
            //connection.On<string, string>("ReceiveMessage", (user, message) =>
            //{

            //    textBox2.Text += "uhuu recebi message";

            //});

            try
            {
                await connection.StartAsync();
                MessageBox.Show("Connection started");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Connect();
        }
    }
}
