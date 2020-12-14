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
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class form_inicial : Form
    {
        Socket server;
        int PORT = 50054;
        string IP = "147.83.117.22";
        string[] Usuaris;
        int HeEntrat = 0; // utilitzare aquesta variable per evitar l'error d'intentar entrar 2 cops.
        //int ComençarPartida_index = 0; // utilitzarem aquesta variable per tal de que el convidador només comenci un sol cop la partida encara que convidi a més d'un jugador
        int ID_Partida = 0;
        int contador_IDs = 0;
        string NomUsuariHost; // es fara servir per guardar el nom d'usuari de la persona que es conecta, per si de cas vol començar ella una partida

        int NumJugadorsEquip = 0; // aquesta variable es per saber si el jugador ha creat una partida de 2 o 4 persones. 
        int NumSeleccionats = 0;

        string NomJugador; //per guardar el nom del jugador de la peticio "Donam la partida del jugador amb els maxims punts"
        Thread atendre; // objecte de la classe thread

        
        delegate void DelegadoParaEscribir(string missatge);     
   
        List<FormPartida> FormsPartida = new List<FormPartida>();

        public form_inicial()
        {
            InitializeComponent();
            //CheckForIllegalCrossThreadCalls = false; // Necessari per a poder accedir als elements del form desde un thread diferent
            LlistaConectats.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
        public void PosaDataGriedView(string missatge) // aixo s'ha de fer per qualsevol cosa que modifiqui alguna cosa del client, que no sigui un message box. 
        {
            Usuaris = missatge.Split(',');
            if (Usuaris.Length > 1)
            {
                //  LlistaConectats.RowCount = Int32.Parse(Usuaris[0]);
                LlistaConectats.Rows.Clear();
                LlistaConectats.ColumnHeadersVisible = false;
                LlistaConectats.RowHeadersVisible = false;
                LlistaConectats.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                int i = 1;
                while (i < Usuaris.Length)
                {
                    if (Usuaris[i] != NomUsuari.Text)
                    {
                        LlistaConectats.Rows.Add(Usuaris[i]);
                    }
                    i = i + 1;
                }
            }
            else
            {
                LlistaConectats.Rows.Clear();
            }
        }
        private void AtendreServidor()
        {
            while (true) //bucle infinit
            {
                //Rebem missatge del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                // veiem quin codi de resposta hem rebut
                string[] parts = Encoding.ASCII.GetString(msg2).Split('/');

                int codi = Convert.ToInt32(parts[0]);
                string missatge = parts[1].Split('\0')[0]; // Guardem la resposta

                switch (codi)
                {
                    
                    case 1: // Log in
                        EntrarResposta(missatge);
                        break;
                    case 2: // Registrar
                        RegistrarResposta(missatge);
                        break;
                    case 3: // Nom guanyadors partida
                        NomGuanyadorsPartidaResposta(missatge);
                        break;
                    case 4: // Partida maxim punts
                        PartidaMaximPuntsJugadorResposta(missatge, NomJugador);
                        break;
                    case 5: // Persones que no ha guanyat
                        PersonesQueNoHaGuanyatResposta(missatge);
                        break;
                    case 6: // Numero de serveis
                        NumeroServeisResposta(missatge);
                        break;
                    case 7: // Notificacio llista de conectats
                        ObtenirConectatsResposta(missatge);
                        break;
                       // MessageBox.Show(missatge);
                    case 8: // Notificació on et conviden a jugar una partida
                        MostrarPropostaPartida(missatge); 
                        break;
                    case 9: // Resposta a la petició de convidar a una partida                        
                        string Convidat = ConvidatsNomResposta(missatge);
                        ID_Partida = Convert.ToInt32(ConvidatsIDResposta(missatge));
                        if (Convidat != "NO")
                        {
                            DelegadoParaEscribir delegadoConvidats = new DelegadoParaEscribir(EscriureNomsConvidats);
                            PersonesConvidadesLabel.Invoke(delegadoConvidats,new object[] {Convidat});
                            DelegadoParaEscribir delegadoID = new DelegadoParaEscribir(EscriureIDPartida);
                            ID_label.Invoke(delegadoID, new object[] { Convert.ToString(ID_Partida) });
                        }
                        //PropostaPartidaResposta(missatge);
                        break;
                    case 10:
                        string nomsequip = EquipsResposta(missatge);

                        ThreadStart ts = delegate { ObrirPantallaPartida(nomsequip); };
                        Thread T = new Thread(ts);
                        T.Start();
                        
                        break;

                    case 11:
                        FormsPartida[0].EscriuXat(missatge);
                        break;
                }
            }
        }

        public void Tancar_connexio()
        {
            if (HeEntrat != 0)
            {
                //Mensaje de desconexión
                string mensaje = "0/" + NomUsuari.Text + "/";
                HeEntrat = 0;

                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
                // Nos desconectamos
                atendre.Abort(); // tanquem el thread
                this.BackColor = Color.Gray;
                server.Shutdown(SocketShutdown.Both);
                server.Close();
            }

        }    


        public void ObtenirConectatsResposta(string missatge)
        {
            try
            {
                DelegadoParaEscribir delegado = new DelegadoParaEscribir(PosaDataGriedView);
                LlistaConectats.Invoke(delegado, new object[] {missatge});
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("El missatge es incorrecte");
            }
            LlistaConectats.ClearSelection();
        }
        public void EntrarPeticio()
        {
            
            if (HeEntrat != 1)
            {
                HeEntrat = 1;
                //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
                //al que deseamos conectarnos
                IPAddress direc = IPAddress.Parse(IP);
                IPEndPoint ipep = new IPEndPoint(direc, PORT);


                //Creamos el socket 
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    server.Connect(ipep);//Intentamos conectar el socket


                }
                catch (SocketException ex)
                {
                    //Si hay excepcion imprimimos error y salimos del programa con return 
                    MessageBox.Show("No he podido conectar con el servidor");
                    return;
                }

                NomUsuariHost = NomUsuari.Text;
                string missatge = "1/" + NomUsuari.Text + "/" + Contrasenya.Text; // Volem connectar el usuari => missatge 1/NomUsuari/Contrasenya
                // Enviamos al servidor el nombre tecleado ==> això es per enviar la consulta
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(missatge);
                server.Send(msg);

                
            }
            else
                MessageBox.Show("Ja has entrat");


        }

        public void EntrarResposta(string missatge)
        {
            // Funció que processa la resposta a Entrar
            
            
            if (Convert.ToInt32(missatge) == 1)
            {
                MessageBox.Show("Existeix l'usuari i coincideix la contrasenya");
                //this.BackColor = Color.Lime;                

            }
            else if (Convert.ToInt32(missatge) == 2)
            {
                MessageBox.Show("Existeix l'usuari però no coincideix la contrasenya");
                Tancar_connexio();
            }
            else if (Convert.ToInt32(missatge) == 3)
            {
                MessageBox.Show("No existeix l'usuari");
                //Mensaje de desconexión
                Tancar_connexio();
            }
            LlistaConectats.ClearSelection();
        }

        public void RegistrarPeticio()
        {
            if (HeEntrat != 1)
            {
                HeEntrat = 1;
                //Creamos un IPEndPoint con el ip del servidor y puerto del servidor 
                //al que deseamos conectarnos
                IPAddress direc = IPAddress.Parse(IP);
                IPEndPoint ipep = new IPEndPoint(direc, PORT);


                //Creamos el socket 
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    server.Connect(ipep);//Intentamos conectar el socket


                }
                catch (SocketException ex)
                {
                    //Si hay excepcion imprimimos error y salimos del programa con return 
                    MessageBox.Show("No he podido conectar con el servidor");
                    return;
                }

                string missatge = "2/" + NomUsuari.Text + "/" + Contrasenya.Text; // Volem connectar el usuari => missatge 1/NomUsuari/Contrasenya
                MessageBox.Show(missatge);
                // Enviamos al servidor el nombre tecleado ==> això es per enviar la consulta
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(missatge);
                server.Send(msg);

                
            }
        }

        
        public void RegistrarResposta(string missatge)
        {
            // Funcio que processa la resposta a la peticio de registrarse

            
            if (Convert.ToInt32(missatge) == 1)
            {
                MessageBox.Show("Usuari Registrat i connectat.");
                this.BackColor = Color.Lime;                
            }
            else if (Convert.ToInt32(missatge) == 2)
            {
                MessageBox.Show("Ja existeix aquest usuari.");
                Tancar_connexio();
            }
            LlistaConectats.ClearSelection();
        }

        public void MostrarPropostaPartida(string missatge) // el missatge que rebem aquí és de la forma convidador,ID
        {
            string[] miss = missatge.Split(',');
            int ID_Actual = Convert.ToInt32(miss[1]);
            Invitació invitació = new Invitació();
            invitació._Missatge = "El jugador amb nom d'usuari " + miss[0] + " et convida a jugar una partida";
            invitació.ID = miss[1];
            invitació.ShowDialog();
            string resposta = invitació.resposta;
            string Peticio = "9/" + Convert.ToString(ID_Actual) + "/" + resposta;
            MessageBox.Show("Enviem peticio: " + Peticio);
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(Peticio);
            server.Send(msg);

        }

        

        public string FormarEquips(string NomsLabel)
        {
            //recollir els noms   
            MessageBox.Show(NomsLabel);
            string[] NomsConvidats = NomsLabel.Split(',');
            if (NumJugadorsEquip == 4)
            {                
                NumSeleccionats = NomsConvidats.Length + 1;
                string NomsMissatge = NomsConvidats[0] + "/" + NomsConvidats[1] + "/" + NomsConvidats[2];
                //obrir el form per formar equips
                Formar_equips FormarEquips = new Formar_equips();
                FormarEquips.SetNoms(NomsMissatge);
                FormarEquips.ShowDialog();
                string LlistaCompanys = FormarEquips.GetEquip();
                string Noms = NomUsuariHost + "/" + LlistaCompanys;
                MessageBox.Show("Missatge noms equips: " + Noms);
                return Noms;
                
            }
            else if (NumJugadorsEquip == 2) // estem al cas de un contra un => nomes hem de retornar el nom del convidador i el del convidat
            {
                
                string Noms = NomUsuariHost + "/" + NomsConvidats[0];
                return Noms;
            }
            else
            {
                MessageBox.Show("Has de selaccionar 1 o 3 companys.");
                return "no";
            }
        }

        public void ObrirPantallaPartida(string equip)
        {
            if (ID_Partida != 0)
            {
                int cont = FormsPartida.Count;
                FormPartida partida = new FormPartida(cont, server);
                partida.GetEquip(equip);
                partida.GetJugador(NomUsuariHost);
                partida.GetID(ID_Partida);
                FormsPartida.Add(partida);
                partida.ShowDialog();
                this.Hide(); 
            }

        }

        public string EquipsResposta(string missatge)
        {
            string equip;
            string[] miss = missatge.Split(','); // resposta del servidor 10/ID,convidador,company,nom1,nom2
            ID_Partida = Convert.ToInt32(miss[0]);
            if (miss.Length < 4)
            {
                equip = miss[1] + "," + miss[2];
            }
            else
            {
                equip = miss[1] + "," + miss[2] + "," + miss[3] + "," + miss[4];
            }
            return equip;
        }
        
        public void NumeroServeisResposta(string missatge)
        {
            if (HeEntrat != 0)
            {
                int num = Convert.ToInt32(missatge);
                count_lbl.Text = "Nº de serveis: " + num;
            }
            else
            {
                MessageBox.Show("Cal que et connectis primer.");
            }        
            
        }             


        public void NomGuanyadorsPartidaPeticio(string ID_Partida)
        {   // Demana al servidor el nom dels dos jugadors que van guanyar una partida
            // Protocol 3/ID_Partida

            string mensaje = "3/" + ID_Partida;
            // Envia al servidor la ID de la partida
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            
        }

        public void NomGuanyadorsPartidaResposta(string missatge)
        {
            
            if (missatge == "-1")
            {
                MessageBox.Show("No s'ha obtingut resultat d'aquesta partida.");

            }
            else
            {
                MessageBox.Show("Els jugadors que van guanyar la partida són: " + missatge);
            }
        }

        public void PartidaMaximPuntsJugadorPeticio(string NomJugador)
        {   // Demana al servidor el maxim de punts que ha fet un jugador a les seves partides
            // Protocol 4/NomJugador

            string mensaje = "4/" + NomJugador;
            //Envia al servidor el nom del jugador
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);                      
            
        }

        public void PartidaMaximPuntsJugadorResposta(string missatge,string NomJugador)
        {            
            if (missatge == "-1")
            {
                MessageBox.Show("No s'ha obtingut resultat d'aquest jugador.");
            }
            else
            {
                MessageBox.Show("La partida on el jugador " + NomJugador + " ha fet el màxim de punts és: " + missatge);
            }
        }

        public void PersonesQueNoHanGuanyatPeticio()
        {   // Demana al servidor el nom dels jugadors que no han guanyat cap partida
            // Protocol 5/

            string mensaje = "5/";
            // Nomes enviem el numero del protocol
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

        }

        public void PersonesQueNoHaGuanyatResposta(string missatge)
        {            
            if (missatge == "0")
                MessageBox.Show("No se han encontrado datos");
            else
                MessageBox.Show("Els jugadors que no han guanyat cap partida son: " + missatge);
        }

        public string ConvidatsNomResposta(string missatge) // aqui el missatge te la forma 9/ID,convidat,SI
        {            
            string[] parts = missatge.Split(',');
            if (parts[2] == "SI")
            {
                return parts[1];
            }
            else
            {
                return "NO";
            }
        }

        public string ConvidatsIDResposta(string missatge) // aqui el missatge te la forma 9/ID,convidat,SI
        {
            string[] parts = missatge.Split(',');
            return parts[0];
        }


        public void EscriureNomsConvidats(string convidat)
        {
            PersonesConvidadesLabel.Text = PersonesConvidadesLabel.Text + convidat + ",";

        }

        public void EscriureIDPartida(string ID)
        {
            ID_label.Text = ID;
        }

        private void Desconnectar_Click(object sender, EventArgs e)
        {
            Tancar_connexio();
            LlistaConectats.Rows.Clear();
            PersonesConvidadesLabel.Text = "";
            
        }

        private void EnviarPeticionButton_Click(object sender, EventArgs e)
        {

            if (DonamNomGuanyadorsPartidaButton.Checked)
            {
                if (ID_Partida_TextBox.Text == "")
                {
                    MessageBox.Show("No has escrit cap partida");
                }
                else
                {
                    //Enviar petició
                    NomGuanyadorsPartidaPeticio(ID_Partida_TextBox.Text);
                    
                }
            }

            else if (DonamPartidaMaximPuntsJugadorButton.Checked)
            {
                if (Nom_Jugador_TextBox.Text == "")
                {
                    MessageBox.Show("No has escrit cap nom");
                }
                else
                {
                    //Enviar petició
                    NomJugador = Nom_Jugador_TextBox.Text;
                    PartidaMaximPuntsJugadorPeticio(NomJugador);

                }
            }
            else if (DonamPersonesQueNoHanGuanyatButton.Checked)
            {
                //Enviar petició
                PersonesQueNoHanGuanyatPeticio();
            }
            else
            {
                MessageBox.Show("Selecciona una consulta");
            }
        }

        private void BotoEntrar_Click(object sender, EventArgs e)
        {

            EntrarPeticio();

            // Posar en marxa el thread que atendrà al servidor
            ThreadStart ts = delegate { AtendreServidor(); };
            atendre = new Thread(ts);
            atendre.Start();
        }

        private void RegistrarUsuari_Click(object sender, EventArgs e)
        {
            RegistrarPeticio();

            // Posar en marxa el thread que atendrà al servidor
            ThreadStart ts = delegate { AtendreServidor(); };
            atendre = new Thread(ts);
            atendre.Start(); 
        }

        private void form_inicial_Load(object sender, EventArgs e)
        {
            //LlistaConectats.RowCount = 10;
            LlistaConectats.ColumnCount = 1;
        }

        private void Botó_Convidar_Click(object sender, EventArgs e)
        {
            string[] Seleccionats = new string[100];
            int marcadas = LlistaConectats.SelectedRows.Count;
            int i = 1;
            int row = 0;
            int j = 0 ;
            while (i < Usuaris.Length-1) 
            {
                if (LlistaConectats.Rows[row].Cells[0].Style.BackColor == Color.Blue) 
                {
                    Seleccionats[j] = Usuaris[i];
                    j=j+1;
                    LlistaConectats.Rows[row].Cells[0].Style.BackColor = Color.White;
                    row = row + 1;
                }
                i = i + 1;
            }
            LlistaConectats.ClearSelection();
            
            if (Seleccionats[0] != null)
            {
               
                NumSeleccionats = NumSeleccionats + j;

                if ((NumSeleccionats > 3) || (NumSeleccionats == 2))
                {
                    MessageBox.Show("Només podeu ser 2 o 4 en una partida, torna a intentar-ho o comença la partida");
                    NumSeleccionats = NumSeleccionats - j;
                }
                else
                {
                    
                    MessageBox.Show("Estas creant una partida nova");
                    ID_Partida = contador_IDs + 1;
                    MessageBox.Show("Estas creant una partida nova amb ID = " + Convert.ToString(ID_Partida));
                    
                    //Enviem el missatge
                    int u = 0;
                    string mensaje = "8";
                    mensaje = mensaje + "/" + Convert.ToString(ID_Partida);
                    // Envia al servidor la ID de la partida
                    while (Seleccionats[u] != null)
                    {
                        mensaje = mensaje + "/" + Seleccionats[u];
                        MessageBox.Show(mensaje);
                        u = u + 1;
                        NumJugadorsEquip = NumSeleccionats + 1;
                    }
                    if (NumSeleccionats == 1) //com que estem al cas de un contra 1 hem de omplir els dos noms que queden amb un 0 => 8/ID_partida/nom/0/0
                    {
                        mensaje = mensaje + "/0/0";
                        NumJugadorsEquip = 2;
                    }
                    
                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                    server.Send(msg);

                }
                               
            }
            else
            {
                MessageBox.Show("No hi ha cap persona seleccionada per convidar");
            }
        }

        private void LlistaConectats_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (LlistaConectats.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor == Color.Blue)
            {
                LlistaConectats.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
            }
            else
            {
                LlistaConectats.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Blue;
            }
        }

        private void Començar_button_Click(object sender, EventArgs e)
        {
            string equip;            
            equip = FormarEquips(PersonesConvidadesLabel.Text);
            if (equip == "no")
            {
                MessageBox.Show("No es poden formar els equips");
            }
            else
            {
                string mensaje;

                if (NumJugadorsEquip == 4)
                {
                    //creem el missatge amb els equips per a poder començar la partida a tots els jugadors
                    mensaje = "10/" + Convert.ToString(ID_Partida) + "/2v2"+ "/" + equip;
                }
                else
                {
                    mensaje = "10/" + Convert.ToString(ID_Partida) + "/1v1" + "/" + equip;
                }
                MessageBox.Show(mensaje);
                
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);
                //ObrirPantallaPartida(equip);
            }
            

        }           
    }
}
