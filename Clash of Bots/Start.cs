using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clash_of_Bots
{
    public partial class Start : Form
    {
        float opacity = 0;
        public Start()
        {
            InitializeComponent();
        }
        private void Start_Load(object sender, EventArgs e)
        {
            AllowTransparency = true;
            TransparencyKey = BackColor;
            timer_opacity.Start();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            opacity += 0.02f;
            if(opacity > 1)
            {
                this.Hide();
                timer_opacity.Stop();
                Home home = new Home();
                home.Show();
            } else
                Opacity = opacity;
        }
    }
}
