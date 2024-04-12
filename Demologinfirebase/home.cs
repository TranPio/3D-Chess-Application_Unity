using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Demologinfirebase
{
    public partial class home : Form
    {
        public home()
        {
            InitializeComponent();
        }
        IFirebaseConfig config = new FirebaseConfig()
        {
            AuthSecret = "b2nFnPzL2nVG9NwzcjBtlInSGZXdtWFusy70UYur",
            BasePath = "https://group14demofirebase-default-rtdb.firebaseio.com/",
        };
        IFirebaseClient client;
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FirebaseResponse response = client.Get("Information/");
            Dictionary<string, register> result = response.ResultAs<Dictionary<string, register>>();
            foreach (var get in result)
            {
                string usesrname = get.Value.Name;
                string phone = get.Value.Phone;
               if(phone == Form1.phone)
                MessageBox.Show("Name: " + usesrname + "\nPhone Number: " + phone);
                }
        }

        private void home_Load_1(object sender, EventArgs e)
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

        private async void button2_Click(object sender, EventArgs e)
        {
            //new Update().ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();

        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            new Hosonguoidung().ShowDialog();
        }
        public static string User = "";

        private async void bunifuButton3_Click(object sender, EventArgs e)
        {
            string searchTerm = txtTimkiem.Text; 

            FirebaseResponse response = client.Get("Information/");
            Dictionary<string, register> result = response.ResultAs<Dictionary<string, register>>();

            bool userFound = false;
            string foundUsername = "";
            string foundPhone = "";

            foreach (var get in result)
            {
                string usesrname = get.Value.Name;
                string password = get.Value.Password;
                string phone = get.Value.Phone;

                if (usesrname.ToLower().Contains(searchTerm.ToLower())) 
                {
                    userFound = true;
                    foundUsername = usesrname;
                    foundPhone = phone;
                    break;  
                }
            }

            if (userFound)
            {
                User = foundUsername;
                MessageBox.Show("Tìm kiếm thành công.", "Thông báo ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                new Ketban().ShowDialog();
            }
            else
            {
                MessageBox.Show("Không tìm thấy tên người dùng.", "Thông báo ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
           
        

        private void txtRegName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
