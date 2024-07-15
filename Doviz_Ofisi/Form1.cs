using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Data.SqlClient;

namespace Doviz_Ofisi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-AOEQHQU;Initial Catalog=DovizBurosu;Integrated Security=True");

        private void Form1_Load(object sender, EventArgs e)
        {
            string bugun = "https://www.tcmb.gov.tr/kurlar/today.xml";
            var xmlDosya = new XmlDocument();
            xmlDosya.Load(bugun);

            string dolaralis = xmlDosya.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteBuying").InnerXml;
            LblDolarAlıs.Text = dolaralis;

            string dolarsatis = xmlDosya.SelectSingleNode("Tarih_Date/Currency[@Kod='USD']/BanknoteSelling").InnerXml;
            LblDolarSatis.Text = dolarsatis;

            string euroalis = xmlDosya.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteBuying").InnerXml;
            LblEuroAlıs.Text = euroalis;


            string eurosatis = xmlDosya.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteSelling").InnerXml;
            LblEuroSatıs.Text = eurosatis;

            baglanti.Open();
            SqlCommand komut = new SqlCommand("Select * from TblDoviz", baglanti);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                label15.Text = dr[0].ToString();
                label16.Text = dr[1].ToString();
                label17.Text = dr[2].ToString();
            }
            baglanti.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                TxtKur.Text = LblDolarAlıs.Text;
            }
            if (comboBox1.SelectedIndex == 1)
            {
                TxtKur.Text = LblEuroAlıs.Text;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double kur, miktar, tutar;
            kur = Convert.ToDouble(TxtKur.Text);
            miktar = Convert.ToDouble(TxtMiktar.Text);
            tutar = kur * miktar;
            TxtTutar.Text = tutar.ToString();
        }

        private void TxtKur_TextChanged(object sender, EventArgs e)
        {
            TxtKur.Text = TxtKur.Text.Replace(".", ",");
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox1.BackColor = Color.Red;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.White;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 0)
            {
                textBox3.Text = LblDolarSatis.Text;
            }
            if (comboBox2.SelectedIndex == 1)
            {
                textBox3.Text = LblEuroSatıs.Text;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double kur, miktar, tutar;
            kur = Convert.ToDouble(textBox3.Text);
            miktar = Convert.ToDouble(textBox2.Text);
            tutar = kur * miktar;
            textBox1.Text = tutar.ToString();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox3.Text = textBox3.Text.Replace(".", ",");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                double kasa, tutar, miktar, kalan, tl, tlgelen;
                tl = Convert.ToDouble(label17.Text);
                miktar = Convert.ToDouble(TxtMiktar.Text);
                tutar = Convert.ToDouble(TxtTutar.Text);

                if (comboBox1.SelectedIndex == 0)
                {
                    kasa = Convert.ToDouble(label15.Text);
                    kalan = kasa - miktar;
                    label15.Text = kalan.ToString();
                    tlgelen = tl + tutar;
                    label17.Text = tlgelen.ToString();

                    baglanti.Open();
                    SqlCommand komut = new SqlCommand("Insert into TblDoviz (Dolar,Euro,Lira) values (@p1,@p2,@p3)", baglanti);
                    komut.Parameters.AddWithValue("@p1", kalan);
                    komut.Parameters.AddWithValue("@p2", double.Parse(label16.Text));
                    komut.Parameters.AddWithValue("@p3", tlgelen);
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                }
                if (comboBox1.SelectedIndex == 1)
                {
                    kasa = Convert.ToDouble(label16.Text);
                    kalan = kasa - miktar;
                    label16.Text = kalan.ToString();
                    tlgelen = tl + tutar;
                    label17.Text = tlgelen.ToString();

                    baglanti.Open();
                    SqlCommand komut = new SqlCommand("Insert into TblDoviz (Dolar,Euro,Lira) values (@p1,@p2,@p3)", baglanti);
                    komut.Parameters.AddWithValue("@p1", double.Parse(label15.Text));
                    komut.Parameters.AddWithValue("@p2", kalan);
                    komut.Parameters.AddWithValue("@p3", tlgelen);
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Lütfen boş alan bırakmayınız.");

            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                double kasa, tutar, miktar, gelen, tl, tlkalan;
                miktar = Convert.ToDouble(textBox2.Text);
                tutar = Convert.ToDouble(textBox1.Text);
                tl = Convert.ToDouble(label17.Text);

                if (comboBox2.SelectedIndex == 0)
                {
                    kasa = Convert.ToDouble(label15.Text);
                    gelen = miktar + kasa;
                    label15.Text = gelen.ToString();
                    tlkalan = tl - tutar;
                    label17.Text = tlkalan.ToString();

                    baglanti.Open();
                    SqlCommand komut = new SqlCommand("Insert into TblDoviz (Dolar,Euro,Lira) values (@p1,@p2,@p3)", baglanti);
                    komut.Parameters.AddWithValue("@p1", gelen);
                    komut.Parameters.AddWithValue("@p2", double.Parse(label16.Text));
                    komut.Parameters.AddWithValue("@p3", tlkalan);
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                }
                if (comboBox2.SelectedIndex == 1)
                {
                    kasa = Convert.ToDouble(label16.Text);
                    gelen = miktar + kasa;
                    label16.Text = gelen.ToString();
                    tlkalan = tl - tutar;
                    label17.Text = tlkalan.ToString();

                    baglanti.Open();
                    SqlCommand komut = new SqlCommand("Insert into TblDoviz (Dolar,Euro,Lira) values (@p1,@p2,@p3)", baglanti);
                    komut.Parameters.AddWithValue("@p1", double.Parse(label15.Text));
                    komut.Parameters.AddWithValue("@p2", gelen);
                    komut.Parameters.AddWithValue("@p3", tlkalan);
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Lütfen boş alan bırakmayınız.");
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DialogResult secim = new DialogResult();
            secim = MessageBox.Show("Çıkış yapmak istediğinizden emin misiniz?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
