using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace Demologinfirebase
{
    public partial class Dangki : Form
    {
        public Dangki()
        {
            InitializeComponent();
        }
        IFirebaseConfig config = new FirebaseConfig()
        {
            AuthSecret = "a8jIccFDg7Ojyc5bwxV5wfDGEoFNe4uwCvvZBZyf",
            BasePath = "https://team14-covua-default-rtdb.firebaseio.com/",
        };
        IFirebaseClient client;

        private void showpass_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Dangnhap().Show();
             this.Hide();
        }
        public static string email = "";
        public static string coins;

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtRegName.Text) || string.IsNullOrEmpty(txtRegPass.Text) || string.IsNullOrEmpty(txtEmail.Text))
            {
                MessageBox.Show("Please fill all the fields");
                return;
            }
            else
            {
                var register = new register
                {
                    Name = txtRegName.Text,
                    Password = txtRegPass.Text,
                    Email = txtEmail.Text
                };
                FirebaseResponse response = client.Set("Information/" + txtEmail.Text, register);
                register res = response.ResultAs<register>();
                register todo = new register()
                {
                    Email = txtEmail.Text,

                };
                var setter = client.SetAsync("Rewards/" + txtEmail.Text, todo);
                MessageBox.Show("Registration successful");
            }
        }

        private void Dangki_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
            }
            catch
            {
                MessageBox.Show("Connection failed");
            }
        }
    }
}
