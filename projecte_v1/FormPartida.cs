using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class FormPartida : Form
    {
        public FormPartida()
        {
            InitializeComponent();
            this.BackgroundImage = WindowsFormsApplication1.Properties.Resources.fons_joc;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }
        Dau dau = new Dau();
        Random r = new Random();
        string[] equip;

        private void boto_iniciar_Click(object sender, EventArgs e)
        {
            dau.Llançar(r);
            dau.MostrarImatge(fotoDau);
        }
        public void GetEquip(string PersonesConvidades)
        {
            equip[0] = "hola";
        }
    }
}
