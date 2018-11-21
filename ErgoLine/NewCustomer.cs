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
    public partial class NewCustomer : Form
    {
        private Dictionary<int, int> Berletek = new Dictionary<int, int>();
        public NewCustomer()
        {
            InitializeComponent();
            var z = comboBox1;
            Berletek.Add(1, 60);
            Berletek.Add(2, 80);
            Berletek.Add(3, 120);
            Berletek.Add(4, 240);
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        

        private string Save(Customer customer, Berlet berlet)
        {
            

            return "OK";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int returnedid;
            var berlet = comboBox1.SelectedItem.ToString().Replace(" perces", "");
            var berlet_val = Int32.Parse(berlet);
            var berlet_id = Berletek.Where(x => x.Value == berlet_val).FirstOrDefault().Key;
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("A név nem lehet üres!", "", MessageBoxButtons.OK);
            }
            else
            {
                using (SqlConnection conn = new SqlConnection("Data Source=.\\;Initial Catalog=ERGO;Integrated Security=True"))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Customer(Nev, Percek, Regisztracio) output INSERTED.ID VALUES(@Nev,@Percek,@Regisztracio)", conn))
                    {

                        cmd.Parameters.AddWithValue("@Nev", textBox1.Text);
                        cmd.Parameters.AddWithValue("@Percek", berlet_val);

                        cmd.Parameters.AddWithValue("@Regisztracio", DateTime.Now);
                        conn.Open();

                        int modified = (int)cmd.ExecuteScalar();

                        if (conn.State == System.Data.ConnectionState.Open)
                            conn.Close();
                        returnedid = modified;
                        MessageBox.Show("Bértlere kerülő azonosító: #" + modified, "", MessageBoxButtons.OK);
                    }
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO CustomerBerlet(Berlet_ID, Customer_ID, Lejarat) VALUES(@BID, @CID, @Lejarat)", conn))
                    {

                        cmd.Parameters.AddWithValue("@BID", berlet_id);
                        cmd.Parameters.AddWithValue("@CID", returnedid);

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

                    }
                }
            }
            Console.WriteLine(comboBox1.SelectedText);
        }
    }

    public enum BerletTipus
    {
        Harminc = 1,
        Hatvan = 2,
        Szazhusz =3,
        Ketszaznegyven =4

    }
}
