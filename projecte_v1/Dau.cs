using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class Dau
    {
        private int numero;
        public void Llançar(Random r)
        {
            numero = r.Next(6) + 1;
        }
        public int ObtenirNumero()
        {
            return numero;
        }
        public void MostrarImatge(PictureBox pb)
        {
            if (numero == 1)
            {
                pb.Image = WindowsFormsApplication1.Properties.Resources.blau;
            }
            if (numero == 2)
            {
                pb.Image = WindowsFormsApplication1.Properties.Resources.groc;
            }
            if (numero == 3)
            {
                pb.Image = WindowsFormsApplication1.Properties.Resources.lila;
            }
            if (numero == 4)
            {
                pb.Image = WindowsFormsApplication1.Properties.Resources.taronja;
            }
            if (numero == 5)
            {
                pb.Image = WindowsFormsApplication1.Properties.Resources.verd;
            }
            if (numero == 1)
            {
                pb.Image = WindowsFormsApplication1.Properties.Resources.vermell;
            }

        }
    }
}
