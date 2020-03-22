using Nuclear.Channels.Invoker.Entities;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Nuclear.Channels.Invoker
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Initialize Form options
        /// </summary>
        /// 
        public Form1()
        {
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.None;
            InitializeComponent();
        }

        /// <summary>
        /// Initialize Form Controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "ChannelMethod Endpoint";
            label2.Text = "ChannelMethod Request Body";
            label3.Text = "ChannelMethod Response";
            label4.Text = "Http Method";
            label5.Text = "Authentication Type";
            label6.Text = "Username";
            label7.Text = "Password";
            label8.Text = "Token";

            textBox3.ReadOnly = true; //ChannelMethod response (not editable)
            textBox6.PasswordChar = '*'; //Encode Password

            button1.Text = "Send request";

            comboBox1.Items.Add("GET");
            comboBox1.Items.Add("POST");
            comboBox1.SelectedItem = "GET"; //Default

            comboBox2.Items.Add("JSON");
            comboBox2.Items.Add("XML");
            comboBox2.SelectedItem = "JSON";

            toolStrip1.ForeColor = Color.Black;
            toolStrip1.BackColor = Color.DeepSkyBlue;
            toolStripLabel1.Text = "Nuclear Channels Invoker";
        }

        /// <summary>
        /// Sending the request to a Channel 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("URL Parameter is not provided");
                return;
            }

            if (String.IsNullOrEmpty(textBox2.Text) && comboBox1.SelectedItem.ToString() == "POST")
            {
                MessageBox.Show("Input body is neccessary for POST request");
                return;
            }

            ChannelRequest request = new ChannelRequest()
            {
                Url = textBox1.Text,
                Method = comboBox1.SelectedItem.ToString(),
                InputBody = textBox2.Text,
                ContentType = comboBox2.SelectedItem.ToString()
            };

            if (!String.IsNullOrEmpty(textBox4.Text))
            {
                request.AuthType = textBox4.Text;
                if (request.AuthType.Equals("Basic", StringComparison.OrdinalIgnoreCase))
                {
                    request.Username = textBox5.Text;
                    request.Password = textBox6.Text;
                }
                else if (request.AuthType.Equals("Bearer", StringComparison.OrdinalIgnoreCase))
                    request.Token = textBox7.Text;
            }


            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(request.Url);
            httpRequest.Method = request.Method;

            if (request.ContentType == "XML")
            {
                XmlDocument XML_Request = new XmlDocument();
                try
                {
                    XML_Request.LoadXml(request.InputBody);
                }
                catch (Exception ex)
                {
                    textBox3.Text = ex.Message;
                    return;
                }
                httpRequest.Accept = "application/xml";
            }
            else
                httpRequest.Accept = "application/json";

            if (request.AuthType != null)
                httpRequest.PreAuthenticate = true;


            if (request.Method == "POST")
            {
                byte[] body = Encoding.UTF8.GetBytes(request.InputBody);
                httpRequest.ContentLength = body.Length;
                Stream requestStream = httpRequest.GetRequestStream();
                requestStream.Write(body, 0, body.Length);
                requestStream.Close();
            }

            if (request.AuthType != null && request.AuthType.Equals("basic", StringComparison.OrdinalIgnoreCase))
            {
                string encodedAuthorization = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes($"{request.Username}:{request.Password}"));
                httpRequest.Headers.Add("Authorization", $"{request.AuthType} {encodedAuthorization}");
            }
            else if (request.AuthType.Equals("bearer", StringComparison.OrdinalIgnoreCase))
            {
                string encodedAuthorization = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes($"{request.Token}"));
                httpRequest.Headers.Add("Authorization", $"{request.AuthType} {encodedAuthorization}");
            }

            string read = string.Empty;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse())
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    read = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                textBox3.Text = ex.Message;
            }
        }

        private void Label6_Click(object sender, EventArgs e)
        {

        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "GET")
                textBox2.ReadOnly = true;
            else
                textBox2.ReadOnly = false;
        }

        private void ToolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem.ToString() == "XML")
            {
                textBox2.Text = "<channel> </channel>";
            }
            else
                textBox2.Text = "{  }";
        }
    }
}
