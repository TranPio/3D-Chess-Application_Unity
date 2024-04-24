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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace Demologinfirebase
{
    public partial class Dangnhap : Form
    {
        public Dangnhap()
        {
            InitializeComponent();
        }
        IFirebaseConfig config = new FirebaseConfig()
        {
            AuthSecret = "a8jIccFDg7Ojyc5bwxV5wfDGEoFNe4uwCvvZBZyf",
            BasePath = "https://team14-covua-default-rtdb.firebaseio.com/",
        };
        IFirebaseClient client;

        private void button2_Click(object sender, EventArgs e)
        {
            new Dangki().Show();
            this.Hide();
        }
        public static string email = "";
        public static string coins;
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUser.Text) || string.IsNullOrEmpty(txtPass.Text))
            {
                MessageBox.Show("Please fill all the fields");
                return;
            }
            else
            {
                FirebaseResponse response = client.Get("Information/");
                Dictionary<string, register> result = response.ResultAs<Dictionary<string, register>>();
                foreach (var get in result)
                {
                    string usesrname = get.Value.Name;
                    string password = get.Value.Password;
                    if (usesrname == txtUser.Text && password == txtPass.Text)
                    {
                        email = get.Value.Email;
                        MessageBox.Show(" Đăng nhập thành công. Chào mừng " + txtUser.Text, "Thông báo ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        usesrname = txtUser.Text;
                        new home().ShowDialog();
                        txtUser.Text = "";
                        txtPass.Text = "";
                        break;
                    }
                    //if (usesrname != txtUser.Text && password != txtPass.Text)
                    //{
                    //    MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu"); break;
                    //}
                    if ((usesrname == txtUser.Text && password != txtPass.Text))
                    {
                        MessageBox.Show("Sai mật khẩu.", "Thông báo ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    }
                    if ((usesrname != txtUser.Text && password == txtPass.Text))
                    {
                        MessageBox.Show("Sai tên đăng nhập", "Thông báo ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    }


                }



            }
        }

        private void Dangnhap_Load(object sender, EventArgs e)
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
