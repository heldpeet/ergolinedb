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
    public partial class Levonas : Form
    {
        private int _id;
        private string _name;
        private int _percek;
        public Levonas(int id, string name)
        {
            InitializeComponent();
            _id = id;
            _name = name;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection("Data Source=.\\;Initial Catalog=ERGO;Integrated Security=True"))
            {
                string query = "select * from Customer where id=" + _id;
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    
                    conn.Open();


                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var z = reader.GetSqlString(reader.GetOrdinal("Nev"));
                            _percek = reader.GetInt32(reader.GetOrdinal("Percek"));
                            
                            Console.WriteLine(reader.GetSqlString(reader.GetOrdinal("Nev")));
                        }
                    }
                    conn.Close();
                }
                var levonando = Int32.Parse(textBox1.Text);
                if (_percek - levonando < 0)
                {
                    MessageBox.Show("Nem lehet negatív a percek száma levonás után!", "", MessageBoxButtons.OK);
                }
                else { 
                    using (SqlCommand cmd = new SqlCommand("UPDATE Customer SET Percek=Percek-@Perc WHERE ID = @cid", conn))
                    {

                        cmd.Parameters.AddWithValue("@cid", _id);
                        cmd.Parameters.AddWithValue("@Perc", Int32.Parse(textBox1.Text));
                        MessageBox.Show(_id + " lesz az ID, percek: " + textBox1.Text, "", MessageBoxButtons.OK);
                        conn.Open();

                        cmd.ExecuteScalar();

                        if (conn.State == System.Data.ConnectionState.Open)
                            conn.Close();

                        MessageBox.Show("Levonva: " + textBox1.Text +" "+_name+" perceiből", "", MessageBoxButtons.OK);
                    }
                }
            }
        }
    }
}
