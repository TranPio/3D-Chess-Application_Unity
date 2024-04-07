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
    public partial class Ketban : Form
    {
        IFirebaseConfig config = new FirebaseConfig()
        {
            AuthSecret = "b2nFnPzL2nVG9NwzcjBtlInSGZXdtWFusy70UYur",
            BasePath = "https://group14demofirebase-default-rtdb.firebaseio.com/",
        };
        IFirebaseClient client;
        public Ketban()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Ketban_Load(object sender, EventArgs e)
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
                    string phone = get.Value.Phone;
                    if (usesrname == Timkiem.User)
                    {
                        textBox1.Text = usesrname;
                        textBox2.Text = phone;
                    }
                }

            }
            catch
            {
                MessageBox.Show("Connection failed");
            }
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {

        }

        private void bunifuButton4_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
