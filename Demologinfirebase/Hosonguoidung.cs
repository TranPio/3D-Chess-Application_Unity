using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demologinfirebase
{
    public partial class Hosonguoidung : Form
    {
        IFirebaseConfig config = new FirebaseConfig()
        {
            AuthSecret = "a8jIccFDg7Ojyc5bwxV5wfDGEoFNe4uwCvvZBZyf",
            BasePath = "https://team14-covua-default-rtdb.firebaseio.com/",
        };
        IFirebaseClient client;
        public Hosonguoidung()
        {
            InitializeComponent();
        }

        private void Hosonguoidung_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = client.Get("Information/");
                Dictionary<string, register> result = response.ResultAs<Dictionary<string, register>>();
                foreach (var get in result)
                {
                    string usesrname = get.Value.Name;
                    string password = get.Value.Password;
                    string email = get.Value.Email;
                    if (email == Dangnhap.email)
                    {
                       textBox1.Text = usesrname;
                        textBox2.Text = email;
                    }
                }

            }
            catch
            {
                MessageBox.Show("Connection failed");
            }
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            new UpdateUsername().ShowDialog();
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            new UpdatePassword().ShowDialog();
        }

        private void bunifuButton4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bunifuButton5_Click(object sender, EventArgs e)
        {
            new Xoataikhoan().ShowDialog();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
