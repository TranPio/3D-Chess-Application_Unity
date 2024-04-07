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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Demologinfirebase
{
    public partial class Xoataikhoan : Form
    {
        IFirebaseConfig config = new FirebaseConfig()
        {
            AuthSecret = "b2nFnPzL2nVG9NwzcjBtlInSGZXdtWFusy70UYur",
            BasePath = "https://group14demofirebase-default-rtdb.firebaseio.com/",
        };
        IFirebaseClient client;
        public Xoataikhoan()
        {
            InitializeComponent();
        }

        private void Xoataikhoan_Load(object sender, EventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            FirebaseResponse response = client.Get("Information/");
            Dictionary<string, register> result = response.ResultAs<Dictionary<string, register>>();

            foreach (var get in result)
            {
                string username = get.Value.Name;
                string password = get.Value.Password;
                string phone = get.Value.Phone;

                if (phone == Form1.phone)
                {
                    if (textBox1.Text == password)
                    {
                        DialogResult dialogResult = MessageBox.Show(
                            "Xác nhận xóa tài khoản " + username + "?",
                            "Thông báo",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (dialogResult == DialogResult.Yes)
                        {
                            
                            client.Delete("Information/" + phone);
                            MessageBox.Show("Xóa tài khoản thành công");
                            this.Close();
                            break;
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            MessageBox.Show("Xóa tài khoản đã bị hủy bỏ");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Mật khẩu cũ sai vui lòng nhập chính xác mật khẩu cũ");
                    }
                    break;
                }
            }
        }
    }
}
