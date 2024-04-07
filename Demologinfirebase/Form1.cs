using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
namespace Demologinfirebase
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        IFirebaseConfig config = new FirebaseConfig()
        {
            AuthSecret = "b2nFnPzL2nVG9NwzcjBtlInSGZXdtWFusy70UYur",
            BasePath = "https://group14demofirebase-default-rtdb.firebaseio.com/",
        };
        IFirebaseClient client;
        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void bunifuLabel1_Click(object sender, EventArgs e)
        {

        }

        private void LOGIN_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage("tabPage1");
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage("tabPage2");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
            } catch
            {
                MessageBox.Show("Connection failed");
            }
        }
        public static string phone = "";
        public static string coins;

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty (txtRegName.Text) || string.IsNullOrEmpty(txtRegPass.Text) || string.IsNullOrEmpty(txtRegPhone.Text))
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
                    Phone = txtRegPhone.Text
                };
                FirebaseResponse response = client.Set("Information/" + txtRegPhone.Text, register);
                register res = response.ResultAs<register>();
                register todo = new register()
                {
                    Phone = txtRegPhone.Text,

                };
                var setter = client.SetAsync("Rewards/" + txtRegPhone.Text, todo);
                MessageBox.Show("Registration successful");
            }
        }
      
        private void bunifuButton2_Click(object sender, EventArgs e)
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
                        phone = get.Value.Phone;
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
                            MessageBox.Show("Sai mật khẩu.","Thông báo ",MessageBoxButtons.OK,MessageBoxIcon.Warning);
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

        private void txtUser_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bunifuButton4_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
