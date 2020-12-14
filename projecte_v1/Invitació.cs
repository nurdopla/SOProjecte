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
    public partial class Invitació : Form
    {
        public string _Missatge;
        public string ID;
        public string resposta;
        public Invitació()
        {
            InitializeComponent();
        }

        private void Invitació_Load(object sender, EventArgs e)
        {
            Missatge.Text = _Missatge;
            ID_Label.Text = ID;
        }

       private void Acceptar_Click(object sender, EventArgs e)
        {
            resposta = "SI";
            Close();
        }

        private void Rebutjar_Click(object sender, EventArgs e)
        {
            resposta = "NO";
            Close();
        }
    }
}
