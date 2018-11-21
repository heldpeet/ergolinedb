using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ErgoLine
{
    public partial class Hozzaaad : Form
    {
        private Dictionary<int, string> Berletek = new Dictionary<int, string>();
        private int current_id;
        private string current_name;
        public Hozzaaad(int id, string name)
        {
            Berletek.Add(1, "60 perces");
            Berletek.Add(2, "80 perces");
            Berletek.Add(3, "120 perces");
            Berletek.Add(4, "240 perces");
            InitializeComponent();
            current_id = id;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Nem lehet üres a percek száma!", "", MessageBoxButtons.OK);
            }
            else
            {
                using (SqlConnection conn = new SqlConnection("Data Source=.\\;Initial Catalog=ERGO;Integrated Security=True"))
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE Customer SET Percek=Percek+@Perc WHERE ID = @cid", conn))
                    {

                        cmd.Parameters.AddWithValue("@cid", current_id);
                        cmd.Parameters.AddWithValue("@Perc", Int32.Parse(textBox1.Text));
                        MessageBox.Show(current_id + " lesz az ID, percek: " + textBox1.Text, "", MessageBoxButtons.OK);
                        conn.Open();

                        cmd.ExecuteScalar();

                        if (conn.State == System.Data.ConnectionState.Open)
                            conn.Close();

                        MessageBox.Show("Hozzáadva: " + textBox1.Text, "", MessageBoxButtons.OK);
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var berlet_value = comboBox1.SelectedItem.ToString();
            var plusz_perc_s = berlet_value.Replace(" perces", "");
            var plusz_perc = Int32.Parse(plusz_perc_s);
            var berlet_id = Berletek.Where(x => x.Value == berlet_value).FirstOrDefault().Key;
            using (SqlConnection conn = new SqlConnection("Data Source=.\\;Initial Catalog=ERGO;Integrated Security=True"))
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO CustomerBerlet(Berlet_ID, Customer_ID, Lejarat) VALUES(@BID, @CID, @Lejarat)", conn))
                {

                    cmd.Parameters.AddWithValue("@CID", current_id);
                    cmd.Parameters.AddWithValue("@BID", berlet_id);
                    var lejarat = new DateTime();

                    switch (berlet_id)
                    {
                        case 4:
                            lejarat = DateTime.Now.AddYears(1);
                            break;
                        default:
                            lejarat = DateTime.Now.AddMonths(6);
                            break;
                    }


                    cmd.Parameters.AddWithValue("@Lejarat", lejarat);
                    conn.Open();

                    cmd.ExecuteScalar();

                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();

                    MessageBox.Show(current_name + " számára " + berlet_value, " bérlet jóváírva", MessageBoxButtons.OK);
                }

                using (SqlCommand cmd = new SqlCommand("UPDATE Customer SET Percek=Percek+@Perc WHERE ID = @cid", conn))
                {

                    cmd.Parameters.AddWithValue("@cid", current_id);
                    cmd.Parameters.AddWithValue("@Perc",plusz_perc);

                    conn.Open();
                    cmd.ExecuteScalar();
                    if (conn.State == System.Data.ConnectionState.Open)
                        conn.Close();

                }

            }
        }
    }
}
