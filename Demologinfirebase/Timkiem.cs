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
    public partial class Timkiem : Form
    {
        IFirebaseConfig config = new FirebaseConfig()
        {
            AuthSecret = "a8jIccFDg7Ojyc5bwxV5wfDGEoFNe4uwCvvZBZyf",
            BasePath = "https://team14-covua-default-rtdb.firebaseio.com/",
        };
        IFirebaseClient client;
        public Timkiem()
        {
            InitializeComponent();
        }
        public static string User = "";
        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            FirebaseResponse response = client.Get("Information/");
            Dictionary<string, register> result = response.ResultAs<Dictionary<string, register>>();
            foreach (var get in result)
            {
                string usesrname = get.Value.Name;
                string password = get.Value.Password;
                string email = get.Value.Email;

                
                    if(usesrname==textBox1.Text)
                    {
                        User = usesrname;
                        MessageBox.Show("Tìm kiếm thành công.", "Thông báo ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        new Ketban().ShowDialog();
                        break;
                    }
                    else {                       
                    MessageBox.Show("Không tìm thấy tên người dùng.", "Thông báo ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                           break;
                                       }
                
            }
        }

        private void Timkiem_Load(object sender, EventArgs e)
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
