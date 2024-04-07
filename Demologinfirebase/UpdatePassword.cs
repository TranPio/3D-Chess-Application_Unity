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
    public partial class UpdatePassword : Form
    {
        public UpdatePassword()
        {
            InitializeComponent();
        }
        IFirebaseConfig config = new FirebaseConfig()
        {
            AuthSecret = "b2nFnPzL2nVG9NwzcjBtlInSGZXdtWFusy70UYur",
            BasePath = "https://group14demofirebase-default-rtdb.firebaseio.com/",
        };
        IFirebaseClient client;
        private async void button1_Click(object sender, EventArgs e)
        {
            FirebaseResponse response = client.Get("Information/");
            Dictionary<string, register> result = response.ResultAs<Dictionary<string, register>>();
            foreach (var get in result)
            {
                string usesrname = get.Value.Name;
                string password = get.Value.Password;
                string phone = get.Value.Phone;
                if (phone == Form1.phone)
                {
                    if (textBox1.Text == password)
                    {
                        var data = new register
                        {
                            Name = usesrname,
                            Password = textBox2.Text,
                            Phone = phone
                        };
                        if (textBox2.Text != password)
                        {
                            SetResponse set = client.Set("Information/" + phone, data);
                            MessageBox.Show("Đổi mật khẩu thành công cho  " + phone);
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("Mật khẩu mới không được trùng với mật khẩu cũ");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Mật khẩu cũ sai vui lòng nhập chính xác mật khẩu cũ ");
                    }
                }
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void UpdatePassword_Load(object sender, EventArgs e)
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

        private void bunifuButton4_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
