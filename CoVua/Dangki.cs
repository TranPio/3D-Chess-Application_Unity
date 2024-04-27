using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace CoVua
{
    public partial class Dangki : Form
    {
        public Dangki()
        {
            InitializeComponent();
        }
        IFirebaseConfig config = new FirebaseConfig()
        {
            AuthSecret = "NTJq136Kdo8cmFkPh3Fml88nvgEXl2Md6Bw6JbFS",
            BasePath = "https://team14-database-default-rtdb.firebaseio.com/",
        };
        IFirebaseClient client;
        public static string email = "";
        public static string coins;
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
       // private bool IsValidEmail(string email)
        //{
        //    string regex = @"^[\w\.-]+@[\w\.-]+\.[a-zA-Z]{2,}$";
            //return Regex.Match(email, regex).Success;
        //}
        private bool IsValidPassword(string password)
        {
            string regex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[^\s]{8,}$";
            return Regex.Match(password, regex).Success;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtRegName.Text) || string.IsNullOrEmpty(txtRegPass.Text) || string.IsNullOrEmpty(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin ","Lỗi",MessageBoxButtons.OK,MessageBoxIcon.Hand);
                return;
            }
           // else
           // if (!IsValidEmail(txtEmail.Text))
            //{
              //  TBAOEMAIL.Text = "Vui lòng điền đúng định dạng email abc@gmail.com";
                //return;
            //}
            else if (!IsValidPassword(txtRegPass.Text))
            {
                TBPASS.Text = "Mật khẩu phải chứa ít nhất 8 ký tự, 1 chữ hoa, 1 chữ thường và 1 số";
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
                //string encodedEmail = System.Uri.EscapeDataString(txtEmail.Text);
                string encodedEmail = txtEmail.Text.Replace(".", "-");
                FirebaseResponse response = client.Set("Information/" + encodedEmail, register);
               // FirebaseResponse response = client.Set("Information/" + txtEmail.Text, register);
                register res = response.ResultAs<register>();
                register todo = new register()
                {
                    Email = txtEmail.Text,

                };
                var setter = client.SetAsync("Rewards/" + txtEmail.Text, todo);
                MessageBox.Show("Đăng kí thành công");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Dangnhap().Show();
            this.Hide();
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

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
