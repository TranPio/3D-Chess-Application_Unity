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
    public partial class UpdateUsername : Form
    {
        IFirebaseConfig config = new FirebaseConfig()
        {
            AuthSecret = "a8jIccFDg7Ojyc5bwxV5wfDGEoFNe4uwCvvZBZyf",
            BasePath = "https://team14-covua-default-rtdb.firebaseio.com/",
        };
        IFirebaseClient client;
        public UpdateUsername()
        {
            InitializeComponent();
            
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            FirebaseResponse response = client.Get("Information/");
            Dictionary<string, register> result = response.ResultAs<Dictionary<string, register>>();
            foreach (var get in result)
            {
                string usesrname = get.Value.Name;
                string password = get.Value.Password;
                string email = get.Value.Email;
                
                if (email == Dangnhap.email)
                {
                    var data = new register
                    {
                        Name = textBox2.Text,
                        Password = password,
                        Email = email
                    };
                    SetResponse set = client.Set("Information/" + email, data);
                    MessageBox.Show("Đổi tên thành công cho  " + email);
                    Close();
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private async void UpdateUsername_Load(object sender, EventArgs e)
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
                        if (textBox2.Text == usesrname)
                        { MessageBox.Show("Vui lòng nhập tên khác tên hiện tại "); }
                        else
                        {
                            textBox1.Text = usesrname;
                        }
                    }
                }

                }
            catch
            {
                MessageBox.Show("Connection failed");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void bunifuButton4_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
