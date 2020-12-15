using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace WindowsFormsApplication1
{
    public partial class FormPartida : Form
    {
        int numForm;
        string Jugador; // nom del jugador que esta jugant en aquest formulari
        int ID; // id de la partida
        Socket server;
        
        public FormPartida(int numForm, Socket server)
        {
            InitializeComponent();
            this.numForm = numForm;
            this.server = server;
            this.BackgroundImage = WindowsFormsApplication1.Properties.Resources.fons_joc;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }
        Dau dau = new Dau();
        Random r = new Random();
        string[] equip = new string[4];

        int NumJugadors = 0;
        
        private void boto_iniciar_Click(object sender, EventArgs e)
        {
            dau.Llançar(r);
            dau.MostrarImatge(fotoDau);
        }
        delegate void DelegadoParaEscribir(string[] missatge);
        delegate void DelegadoParaEscribirXat(string missatge);

        public void PosaComponentsEquips(string[] vector) // aixo s'ha de fer per qualsevol cosa que modifiqui alguna cosa del client, que no sigui un message box. 
        {
            if (NumJugadors == 2)
            {
                nomsequip1.Text = vector[0];
                nomsequip2.Text = vector[1];
            }
            else if (NumJugadors == 4)
            {
                nomsequip1.Text = vector[0] + " i " + vector[1];
                nomsequip2.Text = vector[2] + " i " + vector[3];
            }
            
        }

        public void EscriuXat1Delegado(string missatge)
        {
            xat1.Text = missatge;
        }
        public void EscriuXat2Delegado(string missatge)
        {
            xat2.Text = missatge;
        }
        public void EscriuXat3Delegado(string missatge)
        {
            xat3.Text = missatge;
        }
        public void EscriuXat4Delegado(string missatge)
        {
            xat4.Text = missatge;
        }
        public void EscriuXat5Delegado(string missatge)
        {
            xat5.Text = missatge;
        }
        public void EscriuXat6Delegado(string missatge)
        {
            xat6.Text = missatge;
        }
        public void EscriuXat(string missatge) // rep un missatge de l'estil ID_Nom_frase
        {
            string[] miss = missatge.Split('_');           
            
            string frase = miss[1] + ": " + miss[2];

            DelegadoParaEscribirXat Dxat6 = new DelegadoParaEscribirXat(EscriuXat6Delegado);
            xat6.Invoke(Dxat6, new object[] { xat5.Text });

            DelegadoParaEscribirXat Dxat5 = new DelegadoParaEscribirXat(EscriuXat5Delegado);
            xat5.Invoke(Dxat5, new object[] { xat4.Text });

            DelegadoParaEscribirXat Dxat4 = new DelegadoParaEscribirXat(EscriuXat4Delegado);
            xat4.Invoke(Dxat4, new object[] { xat3.Text });

            DelegadoParaEscribirXat Dxat3 = new DelegadoParaEscribirXat(EscriuXat3Delegado);
            xat3.Invoke(Dxat3, new object[] { xat2.Text });

            DelegadoParaEscribirXat Dxat2 = new DelegadoParaEscribirXat(EscriuXat2Delegado);
            xat2.Invoke(Dxat2, new object[] { xat1.Text });

            DelegadoParaEscribirXat Dxat1 = new DelegadoParaEscribirXat(EscriuXat1Delegado);
            xat1.Invoke(Dxat1, new object[] { frase });
        }

        public void EnviaXat() // funcio per enviar al servidor el missatge del xat
        {
            string missatge;
            missatge = xat_textBox.Text;
            missatge = "11/" + Convert.ToString(ID) + "/" + missatge;
            //Envia al servidor
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(missatge);
            server.Send(msg);
        }

        public void GetEquip(string NomsDelEquip) // utilitzem aquesta funció pe robtenir el nom dels dos components del equip.
        {
            string[] noms = NomsDelEquip.Split(',');
            int i = 0;
            NumJugadors = noms.Length;
            while (i < NumJugadors)
            {
                equip[i] = noms[i];
                i = i + 1;
            }
        }

        
        public void GetJugador(string nomJugador)
        {
            this.Jugador = nomJugador;
        }

        public void GetID(int IDPartida)
        {
            this.ID = IDPartida;
        }

        private void FormPartida_Load(object sender, EventArgs e)
        {
            DelegadoParaEscribir delegado = new DelegadoParaEscribir(PosaComponentsEquips);
            nomsequip1.Invoke(delegado, new object[] {equip});
        }

        private void SendXatButton_Click(object sender, EventArgs e)
        {
            EnviaXat();
            xat_textBox.Clear();
        }       
    }
}
