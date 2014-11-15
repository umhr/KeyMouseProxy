using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
//using System.Threading;

namespace KeyMouseTCPServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(label1.Text.Length < 8){
                label1.Text = _tcpAccessor.ipAddress + ":" + _tcpAccessor.portNumber;
            }
            // = DateTime.Now.ToString();
            if (isLog)
            {
                textBox1.Text += _tcpAccessor.data;
            }
            if (isKeyMouse)
            {
                toSetKey(_tcpAccessor.data);
            }
            
            _tcpAccessor.data = "";
            _inputDevice.execute();
            //setTime();
        }
        private TCPAccessor _tcpAccessor;
        public void setTCP()
        {
            string ipAddress = getIPAddress();
            Console.WriteLine(ipAddress);
            //label1.Text = ipAddress;
            _tcpAccessor = new TCPAccessor(ipAddress, 50057);
            _tcpAccessor.connect();
        }

        private InputDevice _inputDevice = new InputDevice();
        private void toSetKey(string data)
        {
            if (data != null && data != "")
            {
                _inputDevice.setKey(data);
            }
        }

        private string getIPAddress()
        {
            // http://techoh.net/get-ipv4-local-ip/
            string ipaddress = "";
            IPHostEntry ipentry = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ip in ipentry.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ipaddress = ip.ToString();
                    //Console.WriteLine(ipaddress);
                }
            }
            return ipaddress;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // C#、Vb2005 マルチスレッド
            // http://www.geocities.jp/hatanero/mulitithred.html
            Thread t1 = new Thread(new ThreadStart(setTCP));
            t1.Start();
            timer1.Start();
        }
        private bool isKeyMouse = true;
        private bool isLog = true;
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            isKeyMouse = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            isLog = checkBox2.Checked;
        }



    }
}
