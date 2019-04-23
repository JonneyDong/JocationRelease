using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iMobileDevice;
using iMobileDevice.iDevice;
using iMobileDevice.Lockdown;
using iMobileDevice.Service;

namespace LocationCleaned
{
    public partial class frmMain : Form
    {
        frmMap map = new frmMap();
        LocationService service;
        public frmMain()
        {
            InitializeComponent();
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            NativeLibraries.Load();
            service = LocationService.GetInstance();
            service.PrintMessageEvent = PrintMessage;
            service.ListeningDevice();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            map.ShowDialog();
            txtLocation.Text = $"{map.Location.Longitude} : {map.Location.Latitude}";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //var location = new Location(txtLocationTest.Text);
            //service.UpdateLocation(location);
            service.UpdateLocation(map.Location);
        }

        public void PrintMessage(string msg)
        {
            if (rtbxLog.InvokeRequired)
            {
                this.Invoke(new Action<string>(PrintMessage), msg);
            }
            else
            {
                rtbxLog.AppendText($"{DateTime.Now.ToString("HH:mm:ss")}： {msg}\r\n");
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            service.ClearLocation();
        }
    }

}
