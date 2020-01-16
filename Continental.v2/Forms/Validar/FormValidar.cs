using Continental.v2.Business;

using Repositories.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Impinj.OctaneSdk;
using static Impinj.OctaneSdk.ImpinjReader;
using Continental.v2.Classes;
using ReposotoriesCUPS.ViewModels;
using ReposotoriesCUPS.Data;
using Repositories.ViewModels;

namespace Continental.v2.Forms.Validar
{
    public partial class FormValidar : Form
    {
        #region Variables
        private static FormValidar _instance;
        string _embarque = string.Empty;
        int _anden = 0;
        ImpinjReader reader;
        List<Tag> Leido = new List<Tag>();
        int imageHeigtMin = 650;
        int imageWhidtMin = 650;
        int imageHeigtMax = 900;
        int imageWhidtMax = 900;
        int imageMax_x = 300;
        int imageMax_y = 50;

        int imageMin_x = 170;
        int imageMin_y = 70;

        string EmbarqueTerminado = "Embarque terminado correctamente";
        string EmbarqueNoTerminado = "Algo salio mal";
        string EmbarqueIncompleto = "No es posible terminar el embarque, el embarque esta incompleto";
        #endregion

        #region Constructor
        public FormValidar(string embarque, int anden)
        {
            _embarque = embarque;
            _anden = anden;
            InitializeComponent();
            reader = new ImpinjReader();
            iniciar();
            LlenarDgv();
            IniciarReader(_embarque, _anden);
        }
        #endregion

        public void Validar(string numPart)
        {
            // Pregunta si esxiste sin leer con el embarque especifico
            int uno = BusinessOrders.MarcarLeido(_embarque, numPart);
            if (uno == 1)
            {
                PalletValido();
                if (!BusinessOrders.EmbarqueVivo2(_embarque))
                    TerminarEmbarque();
            }
            else
            {
                PalletNoValido();
            }
        }

        private void IniciarReader(string embarque, int anden)// string embarque
        {
            ReaderEModel reader = new ReaderEModel();
            try
            {
                reader = GetAnden(anden);
                Reader(reader.IpAddress);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // btnStart.Enabled = true;
            }

        }
        private ReaderEModel GetAnden(int _andenId)
        {
            ReaderEModel anden = new ReaderEModel();
            using (var unitOfWork = new UnitOfWork(new ApplicationDbContext()))
            {
                anden = unitOfWork.ReaderR.ReadsItems(x => x.ReaderID == _andenId).FirstOrDefault();
            }
            return anden;
        }
        public void Reader(string ipAnden)
        {
            try
            {
                string ipReader = ipAnden;

                if (!reader.IsConnected)
                {
                    reader.Connect(ipReader);

                    Settings settings = reader.QueryDefaultSettings();
                    settings.Report.IncludeAntennaPortNumber = true;
                    settings.Session = 2;
                    settings.SearchMode = SearchMode.SingleTarget;
                    settings.Report.IncludeLastSeenTime = true;

                    for (ushort a = 1; a <= 4; a++)
                    {
                        settings.Antennas.GetAntenna(a).TxPowerInDbm = Convert.ToDouble(17);// numericUpDown1.Value
                        settings.Antennas.GetAntenna(a).RxSensitivityInDbm = -70;
                    }
                    reader.ApplySettings(settings);
                    reader.Start();
                }
                reader.TagsReported += new TagsReportedHandler((sReader, report) =>
                {
                    if (BusinessOrders.EmbarqueVivo2(_embarque))
                        ReadTag(report.Tags[0]);
                    else
                        // Mensaje de advertencia por que no se pude agregar otro pallet a un embarque terminado
                        PalletNoValido();

                });
            }
            catch (Exception ex)
            {

                MessageBox.Show("     No fue posible conectar con el reader.    ");
                this.Dispose();
            }
          
        }

        public void ReadTag(Tag tags)
        {
            var numParte = EpcConvertHexAsc.HexToAscii(tags.Epc.ToString());
            if (numParte != null || numParte != string.Empty || numParte != "")
                Validar(numParte);
        }


        #region Botones ELIMINAR
        private void btnIniciar_Click(object sender, EventArgs e)
        {
            iniciar();

        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            cancelar();
        }
        private void btnTerminado_Click(object sender, EventArgs e)
        {
            terminado();
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            valido();
        }
        private void btnValidar_Click(object sender, EventArgs e)
        {

            // Validar();
        }
        #endregion

        #region Metodos Delegados
        public void TerminarEmbarque()
        {
            btnTodo.Invoke(new MethodInvoker(delegate
            {
                terminado();
            }));
        }
        public void PalletValido()
        {
            btnTodo.Invoke(new MethodInvoker(delegate
            {
                valido();
                selected();
            }));
        }

        public void PalletNoValido()
        {
            btnTodo.Invoke(new MethodInvoker(delegate
            {
                cancelar();
            }));
        }

        public void DeselccionarFila()
        {
            btnTodo.Invoke(new MethodInvoker(delegate
            {
                selected();
            }));

        }

        public void selected()
        {
            DgvEmbarque.ClearSelection();
        }
        #endregion

        #region Imagenes de validación
        public void valido()
        {
            pBoxOk.Visible = true;
            pBoxIniciar.Visible = false;
            pBoxCancel.Visible = false;
            pBoxTerminado.Visible = false;
            LlenarDgv();
        }
        /// <summary>
        /// Metodo que muestra la imagen para iniciar la validación 
        /// </summary>
        public void iniciar()
        {
            bool vivo = BusinessOrders.EmbarqueVivo(_embarque).Count > 0 ? true : false;
            if (vivo)
            {
                pBoxIniciar.Visible = true;
                pBoxOk.Visible = false;
                pBoxCancel.Visible = false;
                pBoxTerminado.Visible = false;
            }
            else
                terminado();

        }
        public void cancelar()
        {
            pBoxCancel.Visible = true;
            pBoxOk.Visible = false;
            pBoxIniciar.Visible = false;
            pBoxTerminado.Visible = false;
        }
        public void terminado()
        {
            pBoxTerminado.Visible = true;
            pBoxCancel.Visible = false;
            pBoxOk.Visible = false;
            pBoxIniciar.Visible = false;
        }

        #endregion

        #region GetInstance
        public static FormValidar GetInstance(string _embarque, int _anden)
        {
            if (_instance == null || _instance.IsDisposed)
                _instance = new FormValidar(_embarque, _anden);
            return _instance;
        }
        #endregion


        /// <summary>
        /// boton que abre el form con las lista de os pallet cargados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTarimasCargadas_Click(object sender, EventArgs e)
        {
            DgvEmbarque.ClearSelection();
        }

     
        #region DataGrid
        /// <summary>
        /// Metodo para los nombre de las columnas de la tabla
        /// </summary>
        /// <returns></returns>
        private DataTable LlenarTabla()
        {
            DataTable dt = new DataTable("EmbarqueList");
            dt.Columns.Add("Número de Parte");
            dt.Columns.Add("Traza");
            dt.Columns.Add("Número de Pallet");
            dt.Columns.Add("Lista", typeof(bool));

            return dt;
        }
        private void LlenarDgv()
        {
            OrderVModel order = new OrderVModel();
            DgvEmbarque.DataSource = null;
            var dt = LlenarTabla();
            order = BusinessOrders.GetOrdenCompleta(_embarque);
            foreach (var item in order.ListOrderDetail)
            {
                dt.Rows.Add(item.continentalpartnumber, item.traza, item.total_pallets, item.Leido);
            }

            DgvEmbarque.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            //DgvEmbarque.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            DgvEmbarque.DataSource = dt;

        }

        private void DgvEmbarque_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            foreach (DataGridViewRow row in DgvEmbarque.Rows)
            {

                var escaneado = bool.Parse(row.Cells[3].Value.ToString());
                if (escaneado)
                    row.DefaultCellStyle.BackColor = Color.Green;
                else
                    row.DefaultCellStyle.BackColor = Color.Yellow;
            }

        }
        #endregion

        #region Propiedades dinamicas
        private void PictureBoxCancel(int imageHeigt, int imageWhidt, int x, int y)
        {
            pBoxCancel.Height = imageHeigt;
            pBoxCancel.Width = imageWhidt;
            pBoxCancel.Location = new Point(x, y);
        }

        private void PictureBoxIniciar(int imageHeigt, int imageWhidt, int x, int y)
        {
            pBoxIniciar.Height = imageHeigt;
            pBoxIniciar.Width = imageWhidt;
            this.pBoxIniciar.Location = new Point(x, y);
        }

        private void PictureBoxOk(int imageHeigt, int imageWhidt, int x, int y)
        {
            pBoxOk.Height = imageHeigt;
            pBoxOk.Width = imageWhidt;
            this.pBoxOk.Location = new Point(x, y);
        }

        private void PictureBoxTerminado(int imageHeigt, int imageWhidt, int x, int y)
        {
            pBoxTerminado.Height = imageHeigt;
            pBoxTerminado.Width = imageWhidt;
            this.pBoxTerminado.Location = new Point(x, y);
        }
        #endregion
        #region Botones Max Min Sal Res

        private void Maximizar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            Maximizar.Visible = false;
            Restaurar.Visible = true;

            PictureBoxCancel(imageHeigtMax, imageWhidtMax, imageMax_x, imageMax_y);
            PictureBoxIniciar(imageHeigtMax, imageWhidtMax, imageMax_x, imageMax_y);
            PictureBoxOk(imageHeigtMax, imageWhidtMax, imageMax_x, imageMax_y);
            PictureBoxTerminado(imageHeigtMax, imageWhidtMax, imageMax_x, imageMax_y);
            //DgvEmbarque.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            //this.panelImagen.Location = new Point(100, 0);


            DgvEmbarque.Visible = true;
        }

        private void Salir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Restaurar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            Restaurar.Visible = false;
            Maximizar.Visible = true;
            Maximizar.Visible = true;
            Restaurar.Visible = false;
            DgvEmbarque.Visible = false;
            PictureBoxCancel(imageHeigtMin, imageWhidtMin, imageMin_x, imageMin_y);
            PictureBoxIniciar(imageHeigtMin, imageWhidtMin, imageMin_x, imageMin_y);
            PictureBoxOk(imageHeigtMin, imageWhidtMin, imageMin_x, imageMin_y);
            PictureBoxTerminado(imageHeigtMin, imageWhidtMin, imageMin_x, imageMin_y);
            //DgvEmbarque.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            //var uno = panelImagen.Size;
            //panelImagen.Location = new Point(100, 0);
            //var uno = panelImagen.Location;
            //panelImagen.Size = new System.Drawing.Size(253, 148);
        }

        private void Minimizar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;

        }


        #endregion

        private void FormValidar_Load(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                Maximizar.Visible = false;
                Restaurar.Visible = true;
                PictureBoxCancel(imageHeigtMax, imageWhidtMax, imageMax_x, imageMax_y);
                PictureBoxIniciar(imageHeigtMax, imageWhidtMax, imageMax_x, imageMax_y);
                PictureBoxOk(imageHeigtMax, imageWhidtMax, imageMax_x, imageMax_y);
                PictureBoxTerminado(imageHeigtMax, imageWhidtMax, imageMax_x, imageMax_y);
            }
            DeselccionarFila();
            txbEmbarque.Text = _embarque;
        }

        private void btnTermEmb_Click(object sender, EventArgs e)
        {
            string Mensaje = string.Empty;
            // TODO: Validar si se puede terminar el embarque _embarque
            if (!BusinessOrders.EmbarqueVivo2(_embarque))
            {
                Mensaje = BusinessOrders.TerminarEmbarque(_embarque) == 1 ? EmbarqueTerminado : EmbarqueNoTerminado;
                FormAsignar fa = FormAsignar.GetInstance();
                if (!fa.IsDisposed)
                {
                    if (reader.IsConnected)
                    {
                        reader.Stop();
                        reader.Disconnect();
                    }
                    this.Dispose();
                    fa.Show();
                    fa.BringToFront();

                }
            }
            else
                Mensaje = EmbarqueIncompleto;
            MessageBox.Show(Mensaje);

           

        }
    }
    #region Modelos para el tag
    [Serializable]
    public class ReadTag
    {
        public string continentalpartnumber { get; set; }
        public bool Reading { get; set; }
    }

    [Serializable]
    public class ReadTag2
    {
        public string PartNumber { get; set; }
        public string Quantity { get; set; }
    }
    #endregion

}
