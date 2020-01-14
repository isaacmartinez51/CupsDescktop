using Continental.v2.Business;
using Continental.v2.Forms.Validar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Continental.v2
{
    public partial class FormAsi : Form
    {
        public FormAsi()
        {
            InitializeComponent();
            this.btnObtenerEmbarque.Enter += btnObtenerEmbarque1;

            //this.btnObtenerEmbarque.MouseHover += new System.EventHandler(this.btnObtenerEmbarque_MouseHover);
        }

        private void btnObtenerEmbarque1(object sender, EventArgs e)
        {

            var uno = e;
            var dos = sender;
            //this.btnObtenerEmbarque.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
            this.btnObtenerEmbarque.Image = Properties.Resources.tarima_gris_post_24px;
        }







        #region Botones Max Min Res Cer
        private void Salir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Maximizar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            lblEmbarque.Location = new Point(63, 170);
            txbEmbarque.Location = new Point(12, 210);
            btnObtenerEmbarque.Location= new Point(12, 240);
            cboxAndenes.Location = new Point(12, 290);
            btnAsignar.Location = new Point(12,330);
            var uno = btnObtenerEmbarque.Location;
            var dos = btnAsignar.Location;
            var tres = cboxAndenes.Location;
            Maximizar.Visible = false;
            Restaurar.Visible = true;
        }

        private void Restaurar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            Restaurar.Visible = false;
            Maximizar.Visible = true;
            lblEmbarque.Location = new Point(63, 170);
            txbEmbarque.Location = new Point(12, 210);
            btnObtenerEmbarque.Location = new Point(12, 240);
            cboxAndenes.Location = new Point(12, 290);
            btnAsignar.Location = new Point(12, 330);
            lblEmbarque.Location = new Point(12, 210);
        }

        private void Minimizar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
            
        }
        #endregion

        #region Botones Menu izquierdo

        private void btnTarimasCargadas_Click(object sender, EventArgs e)
        {
            FormListaEmbarque fr = new FormListaEmbarque();
            fr.Show();
        }

        private void btnTerminar_Click(object sender, EventArgs e)
        {
            //int uno = BusinessOrders.TerminarEmbarque(_embarque);
            //if (uno == 1)
            //{
            //    MessageBox.Show("Embarque terminado correctamente");
            //}
            //else
            //    MessageBox.Show("No fué posible terminar el embarque");
        }
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
       

        #endregion

        #region Imagenes de validación
        public void valido()
        {
          
        }
        public void iniciar()
        {
         
            ////_embarque
            //bool vivo = BusinessOrders.EmbarqueVivo("").Count > 0 ? true : false;

            //if (vivo)
            //{
            //    pBoxIniciar.Visible = true;
            //    pBoxOk.Visible = false;
            //    pBoxCancel.Visible = false;
            //    pBoxTerminado.Visible = false;
            //}
            //else
            //{
            //    terminado();
            //}

        }
        public void cancelar()
        {
       
        }
        public void terminado()
        {
          
        }



        #endregion
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void panelUp_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        //private void btnObtenerEmbarque_MouseHover(object sender, EventArgs e)
        //{

            
        //    //this.btnObtenerEmbarque.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
        //    //this.btnObtenerEmbarque.Image = Properties.Resources.tarima_gris_post_24px;
        //}

        private void btnObtenerEmbarque_MouseLeave(object sender, EventArgs e)
        {
            this.btnObtenerEmbarque.Image = Properties.Resources.tarima_blanca_post_24px;
        }

        private void label1_MouseHover(object sender, EventArgs e)
        {
            //label1.Font.Bold = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 15.75F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.label1.Image = global::Continental.v2.Properties.Resources.tarima_gris_post_24px;
            //this.label1.ForeColor = System.Drawing.Color.Black;
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            this.label1.Font = new Font("Century Gothic", 15.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.label1.Image = global::Continental.v2.Properties.Resources.tarima_blanca_post_24px;
            //this.label1.ForeColor = Color.White;
        }

        private void btnAsignar_Click(object sender, EventArgs e)
        {
            
            panelBtnAsignar.Visible = true;
            panelLblEmbarque.Visible = false;
            panelObtenerEmbarque.Visible = false;
            panelCbox.Visible = false;
        }

      

        private void btnObtenerEmbarque_Click(object sender, EventArgs e)
        {
            panelBtnAsignar.Visible = false;
            panelLblEmbarque.Visible = false;
            panelObtenerEmbarque.Visible = true;
            panelCbox.Visible = false;
        }

        private void txbEmbarque_Click(object sender, EventArgs e)
        {
            panelBtnAsignar.Visible = false;
            panelLblEmbarque.Visible = true;
            panelObtenerEmbarque.Visible = false;
            panelCbox.Visible = false;
        }

        private void cboxAndenes_Click(object sender, EventArgs e)
        {
            panelBtnAsignar.Visible = false;
            panelLblEmbarque.Visible = false;
            panelObtenerEmbarque.Visible = false;
            panelCbox.Visible = true;
        }


        private void label1_Click(object sender, EventArgs e)
        {
            panelLblEmbarque.Visible = true;
            panelObtenerEmbarque.Visible = false;
        }

      
    }
}
