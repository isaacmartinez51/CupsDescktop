using Continental.v2.Business;
using Continental.v2.Classes.Exceptions;
using Continental.v2.MessageBoxForms;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Repositories.ViewModels;
using ReposotoriesCUPS.Data;
using ReposotoriesCUPS.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Continental.v2.Forms.Validar
{
    public partial class FormAsignar : Form
    {
        private static FormAsignar _instance;
        string _embarque;
        string embarqueTerminado = "Embarque terminado!";
        public FormAsignar()
        {
            InitializeComponent();

            txbEmbarque.Height = 50;
        }

        #region Get API Embarques Information
        /// <summary>
        /// Metodo que se comunica con la API de embarques 
        /// </summary>
        /// <param name="numeroEmbarque"></param>
        /// <returns></returns>
        private ShipmentVModel Shipment(string numeroEmbarque)
        {

            try
            {
                int embarque = int.Parse(numeroEmbarque);
                var url = "https://continental.xlo.cloud/embarques/aviso/" + embarque;
                var webrequest = (HttpWebRequest)WebRequest.Create(url);
                using (var response = webrequest.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var json = reader.ReadToEnd();
                    ShipmentVModel result = JsonConvert.DeserializeObject<ShipmentVModel>(json);
                    if  (result.detalle == null)
                        throw new Exception($"No existe el embarque: '{embarque}'");
                    else if (result.detalle.Count <= 0)
                        throw new Exception($"No fue posible encontrar datos sobre el embarque:'{embarque}'");
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new DataValidationException("Embarques: ", ex.Message);
            }
        }
        #endregion

        #region BtnsNormales
        private void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                OrderVModel order = new OrderVModel();
                int palletBox = 0;
                if (BusinessOrders.Terminado(txbEmbarque.Text))
                {
                    //MensajeError(embarqueTerminado);
                    MessageBox.Show(embarqueTerminado);
                    //TODO: Limpiar el txt donde se escribe el embarque
                }
                else if (BusinessOrders.ExisteAsignada(txbEmbarque.Text))
                {
                    _embarque = txbEmbarque.Text;
                    order = BusinessOrders.GetOrder(_embarque);

                    FormValidar fv = FormValidar.GetInstance(_embarque, (int)order.ReaderID);
                    if (!fv.IsDisposed)
                    {
                        this.Hide();
                        fv.Show();
                        fv.BringToFront();
                    }
                    else {
                        cboxAndenes.Visible = false;
                        btnAsignar.Visible = false;
                    }

                }
                else if (BusinessOrders.ExisteNoAsignada(txbEmbarque.Text))
                {

                    //TODO: Si esta asignado abrir la siguiente pantalla que es la de validar


                    // Obtengo los datos del embarque
                    var uno = Shipment(txbEmbarque.Text);

                    if (uno != null)
                    {
                        // Preguntas si esta cancelado
                        if (int.Parse(uno.cancelado) == 0)
                        {
                            dataGridView1.DataSource = null;
                            var dt = LlenarTabla();
                            order = BusinessOrders.GetOrdenCompleta(txbEmbarque.Text);
                            foreach (var item in order.ListOrderDetail)
                            {
                                dt.Rows.Add(item.continentalpartnumber, item.traza, item.total_pallets, item.Leido);
                            }
                            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                            dataGridView1.DataSource = dt;
                            cboxAndenes.Visible = dt != null ? true : false;
                            btnAsignar.Visible = dt != null ? true : false;
                        }
                    }
                    else
                        MessageBox.Show("");
                }
                else
                {
                    // Obtengo los datos del embarque
                    ShipmentVModel embarque = Shipment(txbEmbarque.Text);

                    if (embarque.detalle.Count > 0)
                    {
                        // Preguntas si esta cancelado
                        if (int.Parse(embarque.cancelado) == 0)
                        {
                            // ir a traza y obtener el numero de pallets
                            int index = 0;
                            foreach (OrderDetailVModel item in embarque.detalle)
                            {
                                if (item.continentalpartnumber.ToLower() == "varios")
                                {
                                    item.total_pallets = 1;
                                }
                                else
                                {
                                    #region Connection Traza- Se obtiene el total de piezas por tarima
                                    //string oracleConn = "Data Source= tqdb002x.tq.mx.conti.de:1521/tqtrazapdb.tq.mx.conti.de; User Id=consulta; Password= solover";
                                    //string query = $"SELECT aunitsperbox * aboxperpallet FROM ETGDL.products WHERE MLFB = '{item.continentalpartnumber}' ";
                                    //using (OracleConnection connection = new OracleConnection(oracleConn))
                                    //{
                                    //    OracleCommand command = new OracleCommand(query, connection);
                                    //    connection.Open();
                                    //    OracleDataReader reader = command.ExecuteReader();

                                    //    if (reader.Read())
                                    //    {
                                    //        palletBox = reader.GetInt32(0);
                                    //    }

                                    //    reader.Close();
                                    //}

                                    #endregion

                                    #region Dummyes para no conectar con traza
                                    palletBox = 300;
                                    #endregion

                                    if (int.Parse(item.cantidad) <= palletBox)
                                    {
                                        item.total_pallets = 1;
                                    }
                                    else
                                    {
                                        //int pallets = int.Parse(item.cantidad) / piezasPorTarima;
                                        //if (pallets == 0)
                                        //    txbEmbarque.Text = string.Empty;
                                        //item.total_pallets = pallets;
                                        int pallets = int.Parse(item.cantidad) / palletBox;
                                        item.total_pallets = pallets;
                                    }

                                }
                                index++;
                            }
                            var orderT = BusinessOrders.CreateNuevaOrden(embarque);
                            order = orderT.Result;
                            // Crear el embarque

                            dataGridView1.DataSource = null;
                            var dt = LlenarTabla();
                            order = BusinessOrders.GetOrdenCompleta(txbEmbarque.Text);
                            foreach (var item in order.ListOrderDetail)
                            {
                                dt.Rows.Add(item.continentalpartnumber, item.traza, item.total_pallets, item.Leido);
                            }
                            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                            dataGridView1.DataSource = dt;
                            cboxAndenes.Visible = dt != null ? true : false;
                            btnAsignar.Visible = dt != null ? true : false;
                        }
                        else
                            MessageBox.Show("El embarque no contiene tarimas para embarcar");
                    }

                }

            }
            catch (DataValidationException dex)
            {

                MessageBox.Show(dex.ErrorMessage, dex.PropertyName);
            }
        }
        private void btnAsignar_Click(object sender, EventArgs e)
        {
            ReaderVModel anden = (ReaderVModel)cboxAndenes.SelectedItem;
            // TODO-1: Validar que regresa cuando no da un error
            var uno = BusinessOrders.IniciarEmbarque(txbEmbarque.Text, anden.ReaderID);
            if (uno == 1)
            {
                // TODO-3: Abrir el form que validará las tarimas
                MessageBox.Show("Se inició el embarque");
                _embarque = txbEmbarque.Text;

                FormValidar fv = FormValidar.GetInstance(_embarque, anden.ReaderID);
                if (!fv.IsDisposed)
                {
                    this.Hide();
                    fv.Show();
                    fv.BringToFront();
                }
                else
                {
                    cboxAndenes.Visible = false;
                    btnAsignar.Visible = false;
                }
            }
            else
                MessageBox.Show("No fué posible iniciar el embarque");
        }

        #endregion

        private DataTable LlenarTabla()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Número de parte");
            dt.Columns.Add("Traza");
            dt.Columns.Add("Tarima número");
            dt.Columns.Add("Cargado", typeof(bool));
            return dt;
        }
        private void LlenarComboAnden()
        {
            List<ReaderVModel> readers = new List<ReaderVModel>();
            using (var unitOfWork = new UnitOfWork(new ApplicationDbContext()))
            {
                readers = unitOfWork.ReaderR.GetQueryReader().ToList();
                cboxAndenes.Enabled = readers.Count > 0 ? true : false;
            }
            //Vaciar comboBox
            cboxAndenes.DataSource = null;
            //Asignar la propiedad DataSource
            cboxAndenes.DataSource = readers;
            //Indicar qué propiedad se verá en la lista
            cboxAndenes.DisplayMember = "Name";
            //Indicar qué valor tendrá cada ítem
            cboxAndenes.ValueMember = "ReaderID";
        }


        #region Botones Max Min Res Cer
        private void Salir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Maximizar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            Maximizar.Visible = false;
            Restaurar.Visible = true;
        }

        private void Restaurar_Click(object sender, EventArgs e)
        {
            StartPosition = FormStartPosition.CenterParent;
            WindowState = FormWindowState.Normal;
            Restaurar.Visible = false;
            Maximizar.Visible = true;
            
        }

        private void Minimizar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;

        }
        #endregion

        #region GetInstance
        public static FormAsignar GetInstance()
        {
            if (_instance == null || _instance.IsDisposed)
                _instance = new FormAsignar();
            return _instance;

        }

        private void cboxAndenes_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReaderVModel anden = (ReaderVModel)cboxAndenes.SelectedItem;
            if (anden.ReaderID > 1)
                btnAsignar.Enabled = true;
            else
                btnAsignar.Enabled = false;
        }
        #endregion

        private void FormAsignar_Load(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                Maximizar.Visible = false;
                Restaurar.Visible = true;
            }
            // se llena el combobox con los andenes
            LlenarComboAnden();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void MensajeError(string msj)
        {
            DialogResult resultado = new DialogResult();
            Form mensaje = new FormMessageBoxError(msj);

            resultado = mensaje.ShowDialog();
            if (resultado == DialogResult.OK)
            {
                txbEmbarque.Text = string.Empty;
                txbEmbarque.Focus();
            }
        }
    }
}
