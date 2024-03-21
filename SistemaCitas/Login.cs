using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace SistemaCitas
{
    public partial class Login : Form
    {

        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\DiseñandoDWH\SistemaCitas\SistemaCitas\SitemaCitas.mdf;Integrated Security=True;Connect Timeout=30");

        public Login()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            login_password.PasswordChar = login_showPass.Checked ? '\0' : '*';
        }

        private void login_btn_Click(object sender, EventArgs e)
        {
            if (login_username.Text == "")
            {
                MessageBox.Show("Por favor ingresar el nombre de usuario"
                    , "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }else if ( login_password.Text == "") 
            {
                MessageBox.Show("Por favor ingresar la contraseña"
                    , "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (login_username.Text == "" || login_password.Text == "") 
            {
                MessageBox.Show("Por favor ingresar todas las credenciales"
                , "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (connect.State == ConnectionState.Closed)
                {

                    try
                    {
                        connect.Open();

                        string selectData = "SELECT * FROM Users WHERE User_Name = @username " +
                            "AND Password = @password";

                        using (SqlCommand cmd = new SqlCommand(selectData, connect))
                        {
                            cmd.Parameters.AddWithValue("@username", login_username.Text.Trim());
                            cmd.Parameters.AddWithValue("@password", login_password.Text.Trim());

                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            DataTable table = new DataTable();
                            adapter.Fill(table);

                            if (table.Rows.Count >= 1)
                            {
                                MessageBox.Show("Ha iniciado sesión satisfactoriamente", "Mensaje de Información"
                                    , MessageBoxButtons.OK, MessageBoxIcon.Information);

                                MainForm mform = new MainForm();
                                mform.Show();
                                this.Hide();

                            }
                            else
                            {
                                MessageBox.Show("Nombre de usuario u Contraseña incorrecto", "Mensaje de Error"
                                   , MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error" + ex
                            , "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally 
                    {
                        connect.Close();
                    }
                }

                login_username.Text = "";
                login_password.Text = "";
            }
        }
    }
}
