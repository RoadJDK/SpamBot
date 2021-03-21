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
        bool twitchToggler = true;

        public SpamBot()
        {
            InitializeComponent();
            RegisterHotKey(this.Handle, MYACTION_HOTKEY_ID, 0, (int)Keys.F11);
        }

        private void MyTimer_Tick(object sender, EventArgs e)
        {
            if (TwitchModeActive())
            {
                TwitchSpammer();
            }
            else
            {
                StandardSpammer();
            }
        }

        private void StandardSpammer()
        {
            var output = GetSpamInfo();
            SendKeys.Send(output);
            SendKeys.Send("{Enter}");
        }

        private bool TwitchModeActive()
        {
            if (TwitchMode.CheckState == CheckState.Checked)
            {
                return true;
            } else
            {
                return false;
            }
        }

        private void TwitchSpammer()
        {
            var output = GetSpamInfo();
            if (twitchToggler == true)
            {
                SendKeys.Send(output);
                twitchToggler = false;
            } else
            {
                SendKeys.Send(output + output);
                twitchToggler = true;
            }
            SendKeys.Send("{Enter}");
        }

        private string GetSpamInfo() {
            var input = SpamText.Text;
            var rnd = new Random();
            var builder = new StringBuilder();
            
            if (input.Equals("$") || input.Equals(""))
            {
                for (var i = 0; i < rnd.Next(2, 16); i++)
                {
                    builder.Append(rnd.Next(9));
                }
                return builder.ToString();
            } else
            {
                builder.Append(SpamText.Text + " ");
                return builder.ToString();
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312 && m.WParam.ToInt32() == MYACTION_HOTKEY_ID)
            {
                if (!isActive)
                {
                    _myTimer = new Timer();
                    if (GetDelayInfo() != 0)
                    {
                        _myTimer.Interval = GetDelayInfo();
                    }
                    else
                    {
                        _myTimer.Interval = 100; //Default 100ms
                    }
                    if (TwitchModeActive())
                    {
                        _myTimer.Interval = 1000; //Default 100ms
                    }
                    _myTimer.Tick += new EventHandler(MyTimer_Tick);
                    _myTimer.Start();
                }
                else
                {
                    _myTimer.Stop();
                }
                toggle();
            }
            base.WndProc(ref m);
        }

        private int GetDelayInfo()
        {
            return Convert.ToInt32(Delay.Value);
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
