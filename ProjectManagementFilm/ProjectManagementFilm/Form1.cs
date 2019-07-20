using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectManagementFilm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            button1.Click += Button1_Click;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var path = @"C:\git\ForFilm\data\Đường Chuyên - Tang Dynasty Tour (2018) 2019-07-20.html";
            System.Diagnostics.Process.Start(path);
        }
    }
}
