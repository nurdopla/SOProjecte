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
    public partial class Formar_equips : Form
    {
        public Formar_equips()
        {
            InitializeComponent();
        }
        string EQUIP = "";

        public void SetNoms(string llistanoms)
        {
            string[] vector_noms = llistanoms.Split('/'); // de l'altre form rebem un string de la forma nom1/nom2/nom3 , els separem
            // posem als labels els noms dels diferents jugadors per a que el convidador pugui escollir amb qui vol anar a la partida
            Double lengthResult = vector_noms.Length;
            
            if (lengthResult == 3)
            {
                nom1.Text = vector_noms[0];
                nom2.Text = vector_noms[1];
                nom3.Text = vector_noms[2];
            }

        }
        public string GetEquip()
        {
            return EQUIP;
        }

        private void boto_ok_Click(object sender, EventArgs e)
        {
            int selecionats = 0;
            int num_jugador = 0;

            if (nom1.Checked)
            {
                selecionats = selecionats + 1;
                num_jugador = 1;
            }
            else if (nom2.Checked)
            {
                selecionats = selecionats + 1;
                num_jugador = 2;
            }
            else if (nom3.Checked)
            {
                selecionats = selecionats + 1;
                num_jugador = 3;
            }
            if (selecionats == 1)
            {
                if (num_jugador == 1)
                {
                    EQUIP = nom1.Text + "/" + nom2.Text + "/" + nom3.Text;
                    Close();
                }
                else if (num_jugador == 2)
                {
                    EQUIP = nom2.Text + "/" + nom1.Text + "/" + nom3.Text;
                    Close();
                }
                else if (num_jugador == 3)
                {
                    EQUIP = nom3.Text + "/" + nom2.Text + "/" + nom1.Text;
                    Close();
                }

            }
            
        }


    }
}
