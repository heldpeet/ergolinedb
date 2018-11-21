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
    public partial class Form2 : Form
    {
        private Dictionary<int, string> Berletek = new Dictionary<int, string>();
        private int current_id;
        private string current_name;
        public Form2(int id, string name, int percek)
        {
            Berletek.Add(1, "60");
            Berletek.Add(2, "80");
            Berletek.Add(3, "120");
            Berletek.Add(4, "240");
            current_id = id;
            current_name = name;
            InitializeComponent();
            label1.Text = "#" + id + " - " + name + ", hátralévő percek: "+percek;

            using (SqlConnection conn = new SqlConnection("Data Source=.\\;Initial Catalog=ERGO;Integrated Security=True"))
            {
                string query = "select * from CustomerBerlet where Customer_ID=" + current_id;
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    conn.Open();


                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var lejarat = reader.GetDateTime(reader.GetOrdinal("Lejarat"));
                            var kezdet = new DateTime();
                            
                            var tipus_s = Berletek.Where(x=>x.Key == reader.GetInt32(reader.GetOrdinal("Berlet_ID"))).FirstOrDefault().Value;

                            var lejart = (lejarat<DateTime.Now) ? "LEJÁRT" : "AKTÍV";

                            DataGridViewRow row = new DataGridViewRow();
                            

                           
                            switch (tipus_s)
                            {
                                case "4":
                                    kezdet = lejarat.AddYears(-1);
                                    break;
                                default:
                                    kezdet = lejarat.AddMonths(-6);
                                    break;
                            }
                            
                                
                            dataGridView1.Rows.Add(tipus_s +" perces", kezdet.ToShortDateString(), lejarat.ToShortDateString(), lejart);

                        }
                    }
                    conn.Close();
                }
            }



                
            var con_wid = dataGridView1.Size.Width;
            var col_ = con_wid / 4.0;
            dataGridView1.Columns[0].Width = (int)Math.Floor(col_)-2;
            dataGridView1.Columns[1].Width = (int)Math.Floor(col_)-2;
            dataGridView1.Columns[2].Width = (int)Math.Floor(col_)-2;
            dataGridView1.Columns[3].Width = (int)Math.Floor(col_)-2;
            dataGridView1.RowHeadersVisible = false;
           
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hozzaaad hozzaaad = new Hozzaaad(current_id, current_name);
            hozzaaad.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Levonas levonas = new Levonas(current_id, current_name);
            levonas.Show();
        }
    }
}
