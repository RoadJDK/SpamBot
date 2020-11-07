using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace SpamBot
{
    public partial class SpamBot : Form
    {
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        const int MYACTION_HOTKEY_ID = 1;
        private Timer _myTimer;

        bool isActive = false;

        public SpamBot()
        {
            InitializeComponent();
            RegisterHotKey(this.Handle, MYACTION_HOTKEY_ID, 0, (int)Keys.F11);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _myTimer = new Timer();
            _myTimer.Interval = 100; // 1 millisecond
            _myTimer.Tick += new EventHandler(MyTimer_Tick);
            _myTimer.Start();
        }

        private void MyTimer_Tick(object sender, EventArgs e)
        {
            if(isActive)
            {
                StartSpaming();
            }
        }

        private void StartSpaming()
        {
            var rnd = new Random();
            var builder = new StringBuilder();
            for (var i = 0; i < rnd.Next(2, 16);i++)
            {
                builder.Append(rnd.Next(9));
            }
            SendKeys.Send(builder.ToString());
            SendKeys.Send("{Enter}");
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312 && m.WParam.ToInt32() == MYACTION_HOTKEY_ID)
            {
                toggle();
            }
            base.WndProc(ref m);
        }

        private void toggle()
        {
            isActive = !isActive;

            //toggle color
            if (isActive)
            {
                Active.ForeColor = Color.Green;
            }
            else
            {
                Active.ForeColor = Color.Red;
            }
        }

    }
}
