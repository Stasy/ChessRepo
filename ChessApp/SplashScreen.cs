using System;
using System.Windows.Forms;

namespace Chess
{
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
            Timer t = new Timer();
            t.Interval = 3000;
            t.Start();
            t.Tick += new EventHandler(t_Tick);

            timer1.Start();

            Timer timer = new Timer();
            timer.Tick += new EventHandler((sender, e) =>
            {
                if ((Opacity += 0.005d) == 1) timer.Stop();
            });
            timer.Interval = 1;
            timer.Start();
        }

        void t_Tick(object sender, EventArgs e)
        {
            Close();
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {

        }
    }
}
