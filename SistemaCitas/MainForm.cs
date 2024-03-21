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
    public partial class MainForm : Form
    {

        SqlConnection connect = new SqlConnection(@"Data Source=ELIAN-D-;Initial Catalog=Sistema_Citas;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");


        public MainForm()
        {
            InitializeComponent();
            panel3.Visible = false;

            mostrarDatosCitas();
        }

        public void mostrarDatosCitas() 
        {
            CitaDatos cd = new CitaDatos();
            List<CitaDatos> listData = cd.ListarCitas();

            dataGridView1.DataSource= listData;
        }

        public void limpiarCampos() 
        {

            Nombre.Text = "";
            Apellido.Text = "";
            Cedula.Text = "";
            Detalles.Text = "";
            Fecha.Value = DateTime.Now;


        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult check = MessageBox.Show("¿Esta seguro de que quiere cerrar sesión?",
                "Mensaje de Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (check == DialogResult.Yes) 
            {
                Login login = new Login();
                login.Show();
                this.Hide();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel3.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
        }

        private void Nuevo_Click(object sender, EventArgs e)
        {
            if(Nombre.Text == ""
                || Apellido.Text == ""
                || Cedula.Text == ""
                || Detalles.Text == ""
                || Fecha.Text == ""
                ) 
            {
                MessageBox.Show("Favor llenar todos los campos", "Mensaje de Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else 
            {
                if (connect.State == ConnectionState.Closed) 
                {

                    try 
                    { 
                        connect.Open();

                        string checkId = "SELECT COUNT(*) FROM Citas WHERE Id_Cita = @citaId";

                        using (SqlCommand checkCita = new SqlCommand(checkId, connect))
                        {

                            checkCita.Parameters.AddWithValue("@citaId", 1);
                            int count = (int)checkCita.ExecuteScalar();

                            string insertData = "INSERT INTO Citas " +
                                "(Nombre, Apellido, Cedula, Detalle_Cita, Fecha, Id_User) " +
                                "VALUES(@nombre, @apellido, @cedula, @detalle, @fecha, 1)";

                            using(SqlCommand cmd = new SqlCommand(insertData, connect)) 
                            {
                                cmd.Parameters.AddWithValue("@nombre", Nombre.Text.Trim());
                                cmd.Parameters.AddWithValue("@apellido", Apellido.Text.Trim());
                                cmd.Parameters.AddWithValue("@cedula", Cedula.Text.Trim());
                                cmd.Parameters.AddWithValue("@detalle", Detalles.Text.Trim());
                                cmd.Parameters.AddWithValue("@fecha", Fecha.Value);

                                cmd.ExecuteNonQuery();

                                mostrarDatosCitas();

                                MessageBox.Show("Cita registrada satisfactoriamente", "Mensaje de Información",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                                limpiarCampos();

                            }

                        }

                    } 
                    catch (Exception ex)
                    {

                        MessageBox.Show("Error:" + ex, "Mensaje de Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    finally
                    {
                        connect.Close();
                    }

                }

                dataGridView1.Update();
                dataGridView1.Refresh();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex != -1) 
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                int citaId = Convert.ToInt32(row.Cells[0].Value);

                Nombre.Text = row.Cells[1].Value.ToString();
                Apellido.Text = row.Cells[2].Value.ToString();
                Cedula.Text = row.Cells[3].Value.ToString();
                Detalles.Text = row.Cells[4].Value.ToString();
                Fecha.Text = row.Cells[5].Value.ToString();

            }
        }

        private void Modificar_Click(object sender, EventArgs e)
        {

            if (Nombre.Text == ""
                || Apellido.Text == ""
                || Cedula.Text == ""
                || Detalles.Text == ""
                || Fecha.Text == ""
                )
            {
                MessageBox.Show("Favor llenar todos los campos", "Mensaje de Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                DialogResult check = MessageBox.Show("Estas seguro de editar la cita de: " +
                    Nombre.Text.Trim() + " " + Apellido.Text.Trim() + "?", "Mensaje de Confirmación", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);


                if (check == DialogResult.Yes)
                {
                    try
                    {

                        connect.Open();



                        string updateData = "UPDATE Citas SET Nombre = @nombre, " +
                            "Apellido = @apellido, Cedula = @cedula, Detalle_Cita = @detalle," +
                            " Fecha = @fecha, Id_User = 1" +
                            " WHERE Cedula = @cedula";

                        using(SqlCommand cmd = new SqlCommand(updateData, connect)) 
                        {

                            cmd.Parameters.AddWithValue("@nombre", Nombre.Text.Trim());
                            cmd.Parameters.AddWithValue("@apellido", Apellido.Text.Trim());
                            cmd.Parameters.AddWithValue("@cedula", Cedula.Text.Trim());
                            cmd.Parameters.AddWithValue("@detalle", Detalles.Text.Trim());
                            cmd.Parameters.AddWithValue("@fecha", Fecha.Value);


                            cmd.ExecuteNonQuery();

                            mostrarDatosCitas();

                            MessageBox.Show("Cita actualizada satisfactoriamente", "Mensaje de Información",
                                     MessageBoxButtons.OK, MessageBoxIcon.Information);

                            limpiarCampos();
                        }

                }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error:" + ex, "Mensaje de Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connect.Close();
                    }
                }
            }
        }

        private void Limpiar_Click(object sender, EventArgs e)
        {
            limpiarCampos();
        }

        private void Eliminar_Click(object sender, EventArgs e)
        {
            if (Nombre.Text == ""
                || Apellido.Text == ""
                || Cedula.Text == ""
                || Detalles.Text == ""
                || Fecha.Text == ""
                )
            {
                MessageBox.Show("Favor llenar todos los campos", "Mensaje de Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                DialogResult check = MessageBox.Show("Estas seguro de eliminar la cita de: " +
                    Nombre.Text.Trim() + " " + Apellido.Text.Trim() + "?", "Mensaje de Confirmación", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);


                if (check == DialogResult.Yes)
                {
                    try
                    {

                        connect.Open();



                        string updateData = "DELETE FROM Citas WHERE Cedula = @cedula";

                        using (SqlCommand cmd = new SqlCommand(updateData, connect))
                        {

                            cmd.Parameters.AddWithValue("@cedula", Cedula.Text.Trim());

                            cmd.ExecuteNonQuery();

                            mostrarDatosCitas();

                            MessageBox.Show("Cita eliminada satisfactoriamente", "Mensaje de Información",
                                     MessageBoxButtons.OK, MessageBoxIcon.Information);

                            limpiarCampos();
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error:" + ex, "Mensaje de Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connect.Close();
                    }
                }
            }
        }
    }
}
