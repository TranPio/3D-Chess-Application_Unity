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
            AuthSecret = "a8jIccFDg7Ojyc5bwxV5wfDGEoFNe4uwCvvZBZyf",
            BasePath = "https://team14-covua-default-rtdb.firebaseio.com/",
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
                string email = get.Value.Email;
               if(email == Dangnhap.email)
                MessageBox.Show("Name: " + usesrname + "\nEmail: " + email);
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
            string foundEmail = "";

            foreach (var get in result)
            {
                string usesrname = get.Value.Name;
                string password = get.Value.Password;
                string email = get.Value.Email;

                if (usesrname.ToLower().Contains(searchTerm.ToLower())) 
                {
                    userFound = true;
                    foundUsername = usesrname;
                    foundEmail = email;
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            new Hosonguoidung().ShowDialog();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            new Ban_be().ShowDialog();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            new Bangxephang().ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new Caidat().ShowDialog();
        }
    }
}
