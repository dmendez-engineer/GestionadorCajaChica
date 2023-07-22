using GestionadorCajaChica.DB;
using Npgsql;
using Npgsql.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestionadorCajaChica
{
    public partial class Form1 : Form
    {
        public static NpgsqlConnection conn;
        public DataTable fechas = new DataTable();
        public DataTable ingresos = new DataTable();
        public String botonSeleccionado;
        public Form1()
        {
            InitializeComponent();
            CConexcion connecion = new CConexcion();

            conn = connecion.establecerConexcion();

            this.obtenerFechas();

        }
        public void obtenerFechas()
        {
            String query = "Select * from \"Dates\"";
            NpgsqlCommand conector = new NpgsqlCommand(query, conn);
            NpgsqlDataAdapter datos = new NpgsqlDataAdapter(conector);

            datos.Fill(this.fechas);
            int auxNumber = 10;
            foreach (DataRow dr in this.fechas.Rows)
            {
                //Location 6;13
                //Size 103;34
                DateTime date = DateTime.Parse(dr.ItemArray[1].ToString());
                auxNumber = auxNumber + 40;
                Button btn = new Button();

                btn.Size = new Size(103, 34);
                btn.Name = "btnDate" + date.Day;
                btn.Text = date.Date.ToString("dd/MM/yyyy");
                Button addButton = new Button();
                addButton = (Button)pDates.Controls[0];
                btn.Location = new Point(17, auxNumber);
                btn.MouseEnter += new EventHandler(btnDateMouseEnter);
                btn.MouseLeave += new EventHandler(btnDateMouseLeave);
                btn.Click += new EventHandler(btnGetInformation);
                btn.BackColor = Color.Transparent;
                btn.Cursor = Cursors.Hand;
                pDates.Controls.Add(btn);
            }
        }
        Panel p = new Panel();
        private void btnDateMouseEnter(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.BackColor = Color.DarkGray;
        }
        private void btnDateMouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.BackColor = Color.Transparent;
        }
        private void btnMouseEnter(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            pMenu.Controls.Add(p);
            p.BackColor = Color.FromArgb(90, 210, 2);
            p.Size = new Size(109, 5);
            p.Location = new Point(btn.Location.X, btn.Location.Y + 40);
        }
        private void btnMouseLeave(object sender, EventArgs e)
        {
            pMenu.Controls.Remove(p);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            pDates.Visible = !pDates.Visible;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            /* if (rtbData.Visible==true)
             {
         //        rtbData.Visible = !rtbData.Visible;
                 Button btn = sender as Button;
                 rtbData.Text = btn.Text;
             }
             else
             {
                 rtbData.Visible = true;
                 Button btn = sender as Button;
                 rtbData.Text = btn.Text;
             }*/

        }

        private void btnDate_Click(object sender, EventArgs e)
        {
            /* Button btn = sender as Button;
             if (rtbData.Visible==true)
             {
               //  rtbData.Visible = !rtbData.Visible;
                 rtbData.Text = btn.Text;
             }
             else
             {
                 rtbData.Visible = true;

                 rtbData.Text = btn.Text;
             }*/
        }

        private void btnGetInformation(object sender, EventArgs e)
        {
            dgv_Information.AllowUserToAddRows = false;

            if (dgv_Information.ColumnCount != 0)
            {
                int indice = dgv_Information.RowCount;

                for (int i = 0; i < indice; i++)
                {

                    dgv_Information.Rows.RemoveAt(0);
                }


            }


            Button btn = sender as Button;
            this.botonSeleccionado = btn.Text;

            String query = "Select * from \"ingresos\" where \"Fecha\"=" + "'" + btn.Text + "'";

            NpgsqlCommand conector = new NpgsqlCommand(query, conn);
            NpgsqlDataAdapter datos = new NpgsqlDataAdapter(conector);

            datos.Fill(ingresos);
            dgv_Information.DataSource = ingresos;



            dgv_Information.AllowUserToAddRows = true;
            Double totalEfectivo = 0;
            Double totalTransferencia = 0;
            Double totalTarjeta = 0;
            foreach (DataGridViewRow row in dgv_Information.Rows)
            {

                if (row.Cells[0].Value != null)
                {
                    var auxTipoPago = row.Cells[4].Value.ToString();
                    switch (row.Cells[4].Value.ToString())
                    {
                        case "EFECTIVO":
                            totalEfectivo += Double.Parse(row.Cells[7].Value.ToString());
                            break;
                        case "TARJETA":
                            totalTarjeta += Double.Parse(row.Cells[7].Value.ToString());
                            break;
                        case "TRANSFERENCIA":
                            totalTransferencia += Double.Parse(row.Cells[7].Value.ToString());
                            break;


                    }
                }

            }
            lblEfectivo.Text = totalEfectivo.ToString();
            lblTransferencia.Text = totalTransferencia.ToString();
            lblTarjeta.Text = totalTarjeta.ToString();
            Double total = totalEfectivo + totalTransferencia + totalTarjeta;
            lblTotal.Text = total.ToString();



        }
        private void btnAddDate_Click(object sender, EventArgs e)
        {



            Button btn = new Button();

            int index = pDates.Controls.Count - 1;
            btn.Text = DateTime.UtcNow.ToString("dd/MM/yyyy");
            btn.Name = "btnDate" + DateTime.Today.Day;
            btn.Size = new Size(103, 34);

            btn.Click += new System.EventHandler(btnGetInformation);

            Button lastButton = new Button();
            lastButton = (Button)pDates.Controls[index];

            Button button1AddDate = (Button)pDates.Controls[1];
            String query = "Insert into \"Dates\" (\"Fecha\") " +
                "values (" + "'" + DateTime.UtcNow.ToString("dd/MM/yyyy") + "'" + ")";

            NpgsqlCommand conector = new NpgsqlCommand(query, conn);
            conector.ExecuteNonQuery();
            // this.obtenerFechas();
            pDates.Controls.Add(btn);
            btn.Location = new Point(17, lastButton.Location.Y + 40);

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            dgv_Information.DataSource = null;
            dgv_Information.Rows.Clear();

            dgv_Information.Refresh();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
        private void calculoTotales(DataGridView dgv)
        {
            Double totalEfectivo = 0;
            Double totalTransferencia = 0;
            Double totalTarjeta = 0;
            foreach (DataGridViewRow row in dgv.Rows)
            {

                if (row.Cells[0].Value != null)
                {
                    switch (row.Cells[4].Value.ToString())
                    {
                        case "EFECTIVO":
                            totalEfectivo += Double.Parse(row.Cells[7].Value.ToString());
                            break;
                        case "TARJETA":
                            totalTarjeta += Double.Parse(row.Cells[7].Value.ToString());
                            break;
                        case "TRANSFERENCIA":
                            totalTransferencia += Double.Parse(row.Cells[7].Value.ToString());
                            break;


                    }
                }

            }
            lblEfectivo.Text = totalEfectivo.ToString();
            lblTransferencia.Text = totalTransferencia.ToString();
            lblTarjeta.Text = totalTarjeta.ToString();
            Double total = totalEfectivo + totalTransferencia + totalTarjeta;
            lblTotal.Text = total.ToString();
        }
        private void btnAgregarGasto_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fechaSeleccionada = DateTime.Parse(this.botonSeleccionado);

                if (fechaSeleccionada < DateTime.Now.Date)
                {
                    MessageBox.Show("Solo puede agregar un ingreso del día actual");
                    return;
                }
                else
                {
                    String fechaActual = DateTime.Now.ToString("dd/MM/yyyy");
                    String concepto = txtConcepto.Text;
                    String nEquipo = txtNEquipo.Text;
                    String tipoPago = cboTipoPago.SelectedItem.ToString();
                    Double ingreso = Double.Parse(txtIngreso.Text);
                    Double egreso = Double.Parse(txtEgreso.Text);
                    Double saldo = Double.Parse(txtSaldo.Text);
                    String observacion = rtbObservacion.Text;

                    String query = "Insert into \"ingresos\" (\"Fecha\",\"Concepto\",\"NEquipo\"," +
                        "\"TipoPago\",\"Ingreso\",\"Egreso\",\"Saldo\",\"ObservactionIngreso\") values" +
                        "(" + "'" + fechaActual + "'" + "," + concepto +
                        "," + "'" + nEquipo + "'" + "," + "'" + tipoPago.ToUpper() + "'" + "," +
                        ingreso + "," + egreso + "," +
                         saldo + ","
                        + "'" + observacion + "'" + ")";
                    NpgsqlCommand commando = new NpgsqlCommand(query, conn);
                    commando.ExecuteNonQuery();
                    this.obtenerInformacion(fechaActual);
                    this.calculoTotales(dgv_Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Debe seleccionar la fecha actual");
                return;
            }




        }

        private void obtenerInformacion(String date)
        {
            dgv_Information.AllowUserToAddRows = false;

            if (dgv_Information.ColumnCount != 0)
            {
                int indice = dgv_Information.RowCount;

                for (int i = 0; i < indice; i++)
                {

                    dgv_Information.Rows.RemoveAt(0);
                }


            }



            String query = "Select * from \"ingresos\" where \"Fecha\"=" + "'" + date + "'";

            NpgsqlCommand conector = new NpgsqlCommand(query, conn);
            NpgsqlDataAdapter datos = new NpgsqlDataAdapter(conector);

            datos.Fill(ingresos);
            dgv_Information.DataSource = ingresos;



            dgv_Information.AllowUserToAddRows = true;
        }
        private void txtIngreso_TextChanged(object sender, EventArgs e)
        {
            txtSaldo.Text = txtIngreso.Text;
        }

        private void txtEgreso_TextChanged(object sender, EventArgs e)
        {
            if (txtIngreso.Text.Length == 0)
            {
                MessageBox.Show("Debe digitar el ingreso de primero");
                txtEgreso.Text = "0";
                return;
            }
            int ingreso = Int32.Parse(txtIngreso.Text);

            if (txtEgreso.Text.Length != 0)
            {
                int egreso = Int32.Parse(txtEgreso.Text);
                int calculo = ingreso - egreso;
                txtSaldo.Text = calculo.ToString();

            }
            else
            {
                txtSaldo.Text = txtIngreso.Text;
            }

        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {


            String dateFormat = this.dtpDate.Value.ToString("dd/MM/yyyy");
            this.botonSeleccionado = dateFormat;
            this.obtenerInformacion(dateFormat);
            this.calculoTotales(dgv_Information);
        }
    }
}
