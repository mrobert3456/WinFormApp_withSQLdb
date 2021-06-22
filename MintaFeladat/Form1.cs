using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Odbc;

namespace MintaFeladat
{
  
    public partial class Form1 : MaterialSkin.Controls.MaterialForm
    {
        
        string megnev;
        DateTime datumm;
        int allapot;
        int tipus;
        string azonos;
        int kivsor;
        int tid;
        public  string ConnectionString="";
        string sql = null;
        login log;
        int kivtip;
       
        public Form1()
        {
            InitializeComponent();

        }
        SqlCommand cmd = new SqlCommand();
        SqlConnection cnn;
        

        private void RekordÚj(string Megnevezés, DateTime Dátum, int Állapot, int Típus, string Azonosítószám)
        {
            try
            {
                
                megnev = txtmegn.Text;
                datumm = datum.Value;
                allapot = 1;
                tipus = Convert.ToInt32(((ComboItem)cbtipus.SelectedItem).Key);
                azonos = "Nincs";

                var cmd = cnn.CreateCommand();
                cmd.CommandText = "INSERT into Adatok (Megnevezés,Dátum,ÁllapotID,TípusID,Azonosítószám)";
                cmd.CommandText += " VALUES (@Megnevezés,@Dátum,@ÁllapotID,@TípusID,@Azonosítószám)";

                cmd.Parameters.Add("@Megnevezés", SqlDbType.NVarChar).Value = txtmegn.Text;
                cmd.Parameters.Add("@Dátum", SqlDbType.SmallDateTime).Value = datum.Value;
                cmd.Parameters.Add("@ÁllapotID", SqlDbType.Int).Value = allapot;
                cmd.Parameters.Add("@TípusID", SqlDbType.Int).Value = tipus;
                cmd.Parameters.Add("@Azonosítószám", SqlDbType.NVarChar).Value = azonos;

                cmd.ExecuteNonQuery();
                ListaFrissítés();
            }
           catch(System.Data.ConstraintException)
           {
               MessageBox.Show("Ezzel az ID azonosítóval már szerepel egy dokumentum.","Figyelmeztetés!",MessageBoxButtons.OK,MessageBoxIcon.Warning);
           }
           catch(System.Data.SqlClient.SqlException)
           {
                MessageBox.Show("Ezzel az ID azonosítóval már szerepel egy dokumentum.", "Figyelmeztetés!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
           }
           catch(System.FormatException)
           {
                MessageBox.Show("Hibás kitöltés! Ellenőrizze a bevitt adatokat.", "Figyelmeztetés!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
           }
            catch(NullReferenceException)
            {
                
                MessageBox.Show("Hibás kitöltés! Ellenőrizze a bevitt adatokat.", "Figyelmeztetés!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Figyelmeztetés!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void RekordSzerk(string Megnevezés, DateTime Dátum, int Típus,int Állapot, string Azonos)
        {
            try
            {
                megnev = txtmegn.Text;
                datumm = datum.Value;
                allapot = Convert.ToInt32(((ComboItem)cballap.SelectedItem).Key);
                string azonosseged = tid.ToString();

                while (azonosseged.Length != 4)
                {
                    azonosseged = "0" + azonosseged;
                }

                tipus = Convert.ToInt32(((ComboItem)cbtipus.SelectedItem).Key);
                if (allapot == 1)
                {
                    azonos = "Nincs";
                }
                else
                {
                    azonos = DateTime.Now.Year.ToString().Substring(2,2) + "/" + azonosseged;
                }

                SqlConnection mycon = new SqlConnection(ConnectionString);

                SqlCommand cmd = new SqlCommand("UPDATE Adatok SET [Megnevezés] = @Megnevezés, [Dátum] = @Dátum, [ÁllapotID] = @ÁllapotID, [TípusID] = @TípusID, [Azonosítószám] = @Azonosítószám WHERE ID =" + tid, mycon);
                cmd.Parameters.Add("@Megnevezés", SqlDbType.NVarChar).Value = txtmegn.Text;
                cmd.Parameters.Add("@Dátum", SqlDbType.SmallDateTime).Value = datum.Value;
                cmd.Parameters.Add("@ÁllapotID", SqlDbType.Int).Value = allapot;
                cmd.Parameters.Add("@TípusID", SqlDbType.Int).Value = tipus;
                cmd.Parameters.Add("@Azonosítószám", SqlDbType.NVarChar).Value = azonos;

                mycon.Open();

                cmd.ExecuteNonQuery();
                ListaFrissítés();
           

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Figyelmeztetés!",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private void RekordTöröl(int ID)
        {

            using (SqlCommand command = new SqlCommand("DELETE FROM Adatok WHERE ID= "+tid,cnn))
            {
                command.ExecuteNonQuery();
            }
           
        }

        private void Táblaúj()
        {
            try
            {
                // Open the connection
                if (cnn.State == ConnectionState.Open)
                    cnn.Close();
                cnn.ConnectionString = ConnectionString;
                cnn.Open();
            

                sql = "CREATE TABLE Adatok" +
                "(ID INTEGER CONSTRAINT PKeyID PRIMARY KEY IDENTITY(1,1)," +
                "Megnevezés NVARCHAR(100), Dátum SMALLDATETIME, ÁllapotID INTEGER,TípusID INTEGER,Azonosítószám Nvarchar(10))";
            cmd = new SqlCommand(sql, cnn);
            cmd.ExecuteNonQuery();
       


                sql = "CREATE TABLE Típusok" +
               "(ID INTEGER," +
               "Típus NVARCHAR(50))";
                cmd = new SqlCommand(sql, cnn);
                cmd.ExecuteNonQuery();

                sql = "INSERT INTO Típusok(ID, Típus) " +
                    "VALUES ('1', 'fekete') ";
                    cmd = new SqlCommand(sql, cnn);
                    cmd.ExecuteNonQuery();

                sql = "INSERT INTO Típusok(ID, Típus) " +
                  "VALUES ('2', 'fehér') ";
                cmd = new SqlCommand(sql, cnn);
                cmd.ExecuteNonQuery();

                sql = "INSERT INTO Típusok(ID, Típus) " +
                  "VALUES ('3', 'zöld') ";
                cmd = new SqlCommand(sql, cnn);
                cmd.ExecuteNonQuery();



                sql = "CREATE TABLE Állapotok" +
               "(ID INTEGER," +
               "Állapot NVARCHAR(20))";
                cmd = new SqlCommand(sql, cnn);
                cmd.ExecuteNonQuery();

                sql = "INSERT INTO Állapotok(ID, Állapot) " +
                "VALUES ('1', 'Folyamatban') ";//
                cmd = new SqlCommand(sql, cnn);
                cmd.ExecuteNonQuery();

                sql = "INSERT INTO Állapotok(ID, Állapot) " +
                "VALUES ('2', 'Zárt') ";
                cmd = new SqlCommand(sql, cnn);
                cmd.ExecuteNonQuery();

                sql = "INSERT INTO Állapotok(ID, Állapot) " +
                "VALUES ('3', 'Törölt') ";
                cmd = new SqlCommand(sql, cnn);
                cmd.ExecuteNonQuery();

                sql = "CREATE VIEW[dbo].[View_1] AS SELECT dbo.Adatok.ID, dbo.Adatok.Megnevezés, dbo.Adatok.Dátum, dbo.Állapotok.Állapot, dbo.Típusok.Típus, dbo.Adatok.Azonosítószám FROM dbo.Adatok INNER JOIN dbo.Állapotok ON dbo.Adatok.ÁllapotID = dbo.Állapotok.ID INNER JOIN dbo.Típusok ON dbo.Adatok.TípusID = dbo.Típusok.ID";
                cmd = new SqlCommand(sql, cnn);
                cmd.ExecuteNonQuery();
               ListaFrissítés();
                }
                catch (SqlException ae)
                {
                    MessageBox.Show(ae.Message.ToString());
                }
            
        }

        private void ListaFrissítés()
        {
            try
            {
                listView1.Items.Clear();
                SqlCommand cm = new SqlCommand("SELECT* FROM View_1 ORDER BY ID ASC", cnn);

                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    ListViewItem item = new ListViewItem(dr["ID"].ToString());
                    item.SubItems.Add(dr["Megnevezés"].ToString());
                    item.SubItems.Add(dr["Dátum"].ToString());
                    item.SubItems.Add(dr["Állapot"].ToString());
                    item.SubItems.Add(dr["Típus"].ToString());
                    item.SubItems.Add(dr["Azonosítószám"].ToString());


                    listView1.Items.Add(item);
                }
                dr.Close();
                cm.Cancel();
            }
            catch(InvalidOperationException)
            {
                MessageBox.Show("Ilyen adatbázis nincs!", "Figyelmeztetés!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }
        private void TípusTölt()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT* FROM Típusok ORDER BY ID ASC", cnn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow ds in dt.Rows)
            {
                ListViewItem item = new ListViewItem(ds["ID"].ToString());
                item.SubItems.Add(ds["Típus"].ToString());
                ComboItem cbi;
                cbi = new ComboItem(ds["Típus"].ToString(), Convert.ToInt32(ds["ID"]));
                cbtipus.Items.Add(cbi);
                cbtipuskeres.Items.Add(cbi);
              

                listView3.Items.Add(item);
            }
        }

        private void ÁllapotTölt()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT* FROM Állapotok ORDER BY ID ASC", cnn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow ds in dt.Rows)
            {
                ListViewItem item = new ListViewItem(ds["ID"].ToString());
                item.SubItems.Add(ds["Állapot"].ToString());
               

                ComboItem cbi;
                cbi = new ComboItem(ds["Állapot"].ToString(), Convert.ToInt32(ds["ID"]));
                cballapotkeres.Items.Add(cbi);
                cballap.Items.Add(cbi);
                listView2.Items.Add(item);
            }
           

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            log = new login();
            gblezart.Visible = false;
            gbszerk.Visible = true;
            gbtorolt.Visible = false;
            cnn = new SqlConnection(ConnectionString);
            try
            {
                cnn.Open();
                MessageBox.Show("Sikeres csatlakozás!","Csatlakozva",MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                //SqlCommand cm = new SqlCommand("SELECT* FROM Adatok ORDER BY ID ASC", cnn);
                //SqlDataReader dr = cm.ExecuteReader();
                //while (dr.Read())
                //{
                //    ListViewItem item = new ListViewItem(dr["ID"].ToString());
                //    item.SubItems.Add(dr["Megnevezés"].ToString());
                //    item.SubItems.Add(dr["Dátum"].ToString());
                //    item.SubItems.Add(dr["ÁllapotID"].ToString());
                //    item.SubItems.Add(dr["TípusID"].ToString());
                //    item.SubItems.Add(dr["Azonosítószám"].ToString());


                //    listView1.Items.Add(item);
                //}
                //dr.Close();
                //cm.Cancel();
                ÁllapotTölt();
                TípusTölt();
                ListaFrissítés();

            }
            catch  (System.Data.SqlClient.SqlException)
            {
                Táblaúj();
                ÁllapotTölt();
                TípusTölt();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Figyelem!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
    
        }

        private void btnujrek_Click(object sender, EventArgs e)
        {
            RekordÚj(megnev,datumm,1,tipus,azonos);         
            txtmegn.Text = "";
            datum.Value = DateTime.Now;
            cballap.Text = "";
            cbtipus.SelectedItem = null;
        }

        private void btntorol_Click(object sender, EventArgs e)
        {
            cballap.SelectedItem = cballap.Items[2];
            int seged =Convert.ToInt32(((ComboItem)cballap.SelectedItem).Key);
            RekordSzerk(megnev, datumm, tipus, seged, azonos);
            gblezart.Visible = false;
            gbszerk.Visible = false;
            gbtorolt.Visible = true;
            listView1.Items[kivsor].Selected = true;
            listView1.Select();
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            btnmod.Enabled = true;
            lzr2.Enabled = true;
            btntorol.Enabled = true;

            kivsor = e.ItemIndex;

            tid = int.Parse(listView1.Items[kivsor].SubItems[0].Text);

            SqlCommand cm = new SqlCommand("SELECT* FROM Adatok ORDER BY ID ASC", cnn);
            SqlDataReader dr = cm.ExecuteReader();
            txtmegn.Text = listView1.Items[kivsor].SubItems[1].Text;
            datum.Value = Convert.ToDateTime(listView1.Items[kivsor].SubItems[2].Text);
            cballap.Text = listView1.Items[kivsor].SubItems[3].Text;
            cbtipus.Text= listView1.Items[kivsor].SubItems[4].Text;

            for (int i=0;i<cbtipus.Items.Count;i++)
            {
                if(Convert.ToInt32(((ComboItem)cbtipus.Items[i]).Key) == kivtip)
                {
                    cbtipus.Text = cbtipus.Items[i].ToString();
                }
            }

            btnujrek.Enabled = false;

            if (listView1.Items[kivsor].SubItems[3].Text.Contains("Zárt"))
            {
                gblezart.Visible = true;
                gbszerk.Visible = false;
                gbtorolt.Visible = false;
                gbszerk.Enabled = false;
                gbtorolt.Enabled = false;
                gblezart.Enabled = true;
                btntorol.Enabled = true;
                lzr2.Enabled = false;
               
            }
            else if (listView1.Items[kivsor].SubItems[3].Text.Contains("Törölt"))
            {
                gblezart.Visible = false;
                gbszerk.Visible = false;
                gbtorolt.Visible = true;
                gblezart.Enabled = false;
                gbszerk.Enabled = false;
                gbtorolt.Enabled = true;
                btntorol.Enabled = false;
                lzr2.Enabled = true;
              
            }
            else
            {
                gblezart.Visible = false;
                gbszerk.Visible = true;
                gbtorolt.Visible = false;
                gblezart.Enabled = false;
                gbszerk.Enabled = true;
                gbtorolt.Enabled = false;
                lzr2.Enabled = true;
                btnmod.Enabled = true;
                btntorol.Enabled = true;


            }
            dr.Close();
        }

        private void btnfriss_Click(object sender, EventArgs e)
        {
            ListaFrissítés();
        }

        private void btnmod_Click(object sender, EventArgs e)
        {
            RekordSzerk(megnev,datumm,tipus,allapot,azonos);     
            cbtipus.SelectedItem = null;
            btnujrek.Enabled = true;
            listView1.Items[kivsor].Selected = false;
            txtmegn.Text = "";
            datum.Value = DateTime.Now;
            cballap.Text = "";
            gblezart.Visible = false;
            gbtorolt.Visible = false;
            gbszerk.Visible = true;
            gbszerk.Enabled = true;
            btnujrek.Enabled = false;
            btntorol.Enabled = true;
            lzr2.Enabled = true;
            cballap.SelectedItem = null;
            cbtipus.SelectedItem = null;
            btnmod.Enabled = true;
            listView1.Items[kivsor].Selected = true;
            listView1.Select();





        }
        private void Állapotkeresés()
        {
            listView1.Items.Clear();
            SqlCommand cm = new SqlCommand("SELECT * FROM View_1 ORDER BY ID ASC", cnn);
            SqlDataReader dr = cm.ExecuteReader();

            while (dr.Read())
            {
                if (Convert.ToString(dr["Állapot"]) == ((ComboItem)cballapotkeres.SelectedItem).Text)
                {
                    ListViewItem item = new ListViewItem(dr["ID"].ToString());
                    item.SubItems.Add(dr["Megnevezés"].ToString());
                    item.SubItems.Add(dr["Dátum"].ToString());
                    item.SubItems.Add(dr["Állapot"].ToString());
                    item.SubItems.Add(dr["Típus"].ToString());
                    item.SubItems.Add(dr["Azonosítószám"].ToString());
                    listView1.Items.Add(item); //Add this row to the ListView
                }
            }
            dr.Close();
        }
        private void Típuskeresés()
        {
            listView1.Items.Clear(); 
            SqlCommand cm = new SqlCommand("SELECT * FROM View_1 ORDER BY ID ASC", cnn);
            SqlDataReader dr = cm.ExecuteReader();

            while (dr.Read())
            {
                if (Convert.ToString(dr["Típus"]) == ((ComboItem)cbtipuskeres.SelectedItem).Text)
                {
                    ListViewItem item = new ListViewItem(dr["ID"].ToString());
                    item.SubItems.Add(dr["Megnevezés"].ToString());
                    item.SubItems.Add(dr["Dátum"].ToString());
                    item.SubItems.Add(dr["Állapot"].ToString());
                    item.SubItems.Add(dr["Típus"].ToString());
                    item.SubItems.Add(dr["Azonosítószám"].ToString());
                    listView1.Items.Add(item); //Add this row to the ListView
                }
            }
            dr.Close();
        }
       
        private void cballapotkeres_TextChanged(object sender, EventArgs e)
        {
            if (cballapotkeres.Text != "")
            {
                Állapotkeresés();
                btnmod.Enabled = false;
                btntorol.Enabled = false;
                lzr2.Enabled = false;
            }
        }

        private void cbtipuskeres_TextChanged(object sender, EventArgs e)
        {
            if (cbtipuskeres.Text != "")
            {
            Típuskeresés();
                btnmod.Enabled = false;
                btntorol.Enabled = false;
                lzr2.Enabled = false;

            }

        }


        private void megnyit_Click(object sender, EventArgs e)
        {
            cballap.SelectedItem = cballap.Items[0];
            int seged = Convert.ToInt32(((ComboItem)cballap.SelectedItem).Key);
            RekordSzerk(megnev, datumm, tipus, seged,azonos);
            gblezart.Visible = false;
            gbszerk.Visible = true;
            gbtorolt.Visible = false;
            listView1.Items[kivsor].Selected = true;
            listView1.Select();

           

        }

        private void lzr2_Click(object sender, EventArgs e)
        {
            cballap.SelectedItem = cballap.Items[1];
            int seged = Convert.ToInt32(((ComboItem)cballap.SelectedItem).Key);
            RekordSzerk(megnev, datumm, kivtip, seged,azonos);
            gblezart.Visible = true;
            gbszerk.Visible = false;
            gbtorolt.Visible = false;
            gblezart.Enabled = true;
            gbszerk.Enabled = true;
            gbtorolt.Enabled = true;
            listView1.Items[kivsor].Selected = true;
            listView1.Select();

        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)

        {
          
        }

        private void szurtor_Click(object sender, EventArgs e)
        {
            cballapotkeres.SelectedItem = null;
            cbtipuskeres.SelectedItem = null;
            ListaFrissítés();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            kivsor = 0;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                tid = int.Parse(listView1.Items[i].SubItems[0].Text);
                kivsor = i;
                if (listView1.Items[i].SubItems[3].Text.Contains("Törölt"))
                {
                    RekordTöröl(tid);
                }

            }

        }

        private void Form1_Click(object sender, EventArgs e)
        {
            gblezart.Visible = false;
            gbtorolt.Visible = false;
            gbszerk.Visible = true;
            gbszerk.Enabled = true;
            btnujrek.Enabled = true;
            btntorol.Enabled = false;
            lzr2.Enabled = false;

            txtmegn.Text = "";
            cballap.SelectedItem = null;
            cbtipus.SelectedItem = null;

            btnmod.Enabled = false;
          
            ListaFrissítés();
        }


        private class ComboItem
        {
            private string MyText = "";

            private long MyKey = -1;
            public ComboItem(string X = "", Int32 Y = -1)
            {
                MyText = X;
                MyKey = Y;
            }

            public string Text
            {
                get { return MyText; }
                set { MyText = value; }
            }

            public long Key
            {
                get { return MyKey; }
                set { MyKey = value; }
            }

            public override string ToString()
            {
                return MyText.ToString();
            }

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {      
            this.Hide();
            log.ShowDialog();
            this.Close();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {              
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void fileToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            fileToolStripMenuItem.ForeColor = Color.Black;
        }

        private void fileToolStripMenuItem_DropDownClosed(object sender, EventArgs e)
        {
            fileToolStripMenuItem.ForeColor = Color.White;
        }
    }

}
