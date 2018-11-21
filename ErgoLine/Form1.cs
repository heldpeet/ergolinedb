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
    public partial class Form1 : Form
    {
        private int percek;
        private string name;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var exists = false;

            var id = textBox1.Text;
            if (id == null || id == "")
            {

            }
            else
            {
                // Open a connection to the database.
                // Replace the value of connectString with a valid 
                // connection string to a Northwind database accessible 
                // to your system.
                string connectString ="Data Source=.\\;Initial Catalog=ERGO;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectString))
                {

                    string query = "select * from Customer where id="+textBox1.Text;


                    SqlCommand cmd = new SqlCommand(query, connection);

                    connection.Open();


                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            
                            var z = reader.GetSqlString(reader.GetOrdinal("Nev"));
                            Console.WriteLine(reader.GetSqlString(reader.GetOrdinal("Nev")));
                            percek = reader.GetInt32(reader.GetOrdinal("Percek"));
                            name = z.ToString();
                            exists = true;
                        }
                    }
                }

                if (!exists)
                {
                    string message = "Nincs találat ezzel az azonosítóval!";
                    var result = MessageBox.Show(message, "", MessageBoxButtons.OK);
                }

                if (exists)
                {
                    Form2 form = new Form2(Int32.Parse(this.textBox1.Text),name, percek);
                    
                    form.ShowDialog();
                }

                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NewCustomer newCustomer = new NewCustomer();
            newCustomer.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
