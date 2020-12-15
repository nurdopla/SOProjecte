#include <stdio.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <string.h>
#include <mysql.h>
#include <pthread.h>


//nova versio)(
//Declaració variables 
int contador_serveis;
int numSockets;
int sockets[100]; //vector de sockets on anirem guardant el sock_conn de cada client
int i;

//Estructura necessària per l'accès excluent
pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER;

// FUNCIONS
typedef struct{
	char Usuari[20];
	int Socket;
}Usuari;//Declara l'estructura d'un usuari, basada en el seu nom i el socket de conexio

typedef struct{
	Usuari UsuarisConectats[100];
	int num;
}LlistaConectats;//Declara una llista de usuaris que estan conectats

typedef struct{
	Usuari UsuarisPartida[4];	// el primer de tots sempre sera el convidador.
	int numUsuaris;
	int ID;	
	char mode[10]; // 1v1 o 2v2
	char Equip1[40]; //nom1,nom2
	char Equip2[40];
}Partida;//Declara una partida, la cual pot estar formada per un maxim de 4 persones i indica l'identificador d'aquesta

typedef struct{
	Partida Partides[100];	
	int num;
}LlistaPartides;//Declara una llista de partides que s'han iniciat

LlistaConectats llista;
LlistaPartides llistaP;

//Aquesta funció afegeix un usuari a la llista de conectats, donat el seu nom, el socket de conexió i la referencia de la llista on es vol afegir
void AfegirUsuariConectat(LlistaConectats *llista, char nom[20], int socket){
	int i = 0;
	int trobat = 0;
	while (i<llista->num && trobat==0)
	{
		if (strcmp(llista->UsuarisConectats[i].Usuari,nom)==0)
		{
			trobat=1;
		}
		i = i+1;
	}
	if (trobat==0)
	{
		if (llista->num==-1){
			llista->num = 0;
		};
		llista->UsuarisConectats[llista->num].Socket = socket;
		strcpy(llista->UsuarisConectats[llista->num].Usuari,nom);
		llista->num = llista->num + 1;
	}
	return;
}

//Aquest usuari elimina un cert usuari que estaba conectat esborrant el seu nom i cambiant el seu socket a -10, un valor que no es posible per un usuari conectat
void EliminarUsuariConectat(LlistaConectats *llista, char nom[20]){
	int i=0;
	int trobat=0;
	while (i<llista->num && trobat==0)
	{
		if (strcmp(llista->UsuarisConectats[i].Usuari,nom)==0)
		{
			trobat = 1;
			strcpy(llista->UsuarisConectats[i].Usuari,"");
			llista->UsuarisConectats[i].Socket = -10;
		}
		else
			i = i+1;
	}
	return;
}

//Donat el nom de la persona que volem obtenir el socket, aquesta funció retorna el socket guardat a la llista d'usuaris conectats. Retorna -1 si no ha trobat la persona a la llista
int ObtenerSocket (LlistaConectats* llista, char nom[20]){
	int i = 0;
	int trobat = 0;
	while (i<llista->num && trobat ==0)
	{
		if (strcmp(llista->UsuarisConectats[i].Usuari,nom)==0)
		{
			trobat=1;
			return llista->UsuarisConectats[i].Socket;
		}
		i = i+1;
	}
	if (trobat==0)
	{
		return -1;
	}
}

//Donat un socket, busca el nom de l'usuari guardat a la llista de conectats amb aquest socket i l'escriu al char convidador entrat com a parametre
void ObtenirUsuaridesdeSocket(LlistaConectats *llista, int socket, char convidador[20])
{
	int i = 0;
	int trobat = 0;
	while (i<llista->num && trobat ==0)
	{
		if (llista->UsuarisConectats[i].Socket==socket)
		{
			trobat=1;
			strcpy(convidador,llista->UsuarisConectats[i].Usuari);
		}
		i = i+1;
	}
	return;
}

//Funció utilitzada per trobar el socket de la persona que ha convidat a una persona a una partida donat el seu socket
void ObtenirRespostaInvitacio(LlistaConectats* llista, int socket,char resposta[20]){
	int i = 0;
	int trobat = 0;
	while (i<llista->num && trobat ==0)
	{
		if (llista->UsuarisConectats[i].Socket==socket)
		{
			trobat=1;
			strcpy(resposta,llista->UsuarisConectats[i].Usuari);
		}
		i = i+1;
	}
	return;
}

//Funcio que crea un char amb la llista dels usuaris conectats separats per , i el numero d'usuaris davant (ex 3,Jordi,Nuria,Joana)
void CadenaLlistaConectats (LlistaConectats* llista, char UsuarisConectats[512]){
	//funcio que crea un string amb la llista dels usuaris conectats separats per , i el numero d'usuaris davant (ex 3,Jordi,Nuria,Joana)
	int i = 0;
	int sumatori=0;
	char res1[50];
	res1[0] = '\0';
	while (i<llista->num)
	{
		if (llista->UsuarisConectats[i].Socket!=-10)
		{
			sprintf(res1,"%s,%s",res1,llista->UsuarisConectats[i].Usuari);
			sumatori = sumatori + 1;
		}
		i=i+1;
	}
	sprintf(UsuarisConectats,"%d%s",sumatori,res1);
	printf("%s\n",UsuarisConectats);
	return;
}

//Funció que comprova si l'usuari es troba a la base de dades guardada a shiva2. Si l'usuari existeix, es retorna 1. Si l'usuari no existeix, es retorna 0
int PertanyUsuari(MYSQL *conn, char nomusuari[20]) // =1 si l'usuari està a la llista
{
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	
	char consulta[80];
	
	strcpy (consulta,"SELECT JUGADOR.USERNAME FROM (JUGADOR) WHERE JUGADOR.USERNAME = '");
	strcat (consulta, nomusuari);
	strcat (consulta, "'");
	
	int err = mysql_query (conn, consulta);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	resultado = mysql_store_result (conn);
	
	row = mysql_fetch_row (resultado);
	
	if (row == NULL)
		return 0; // pq no s'han obtingut dades per tant no hi ha l'usuari
	
	else
		return 1;
	
	mysql_close (conn);
	exit(0);
}

//Comprova si la contrasenya indicada a l'hora d'accedir al sistema es correcte donat com a parametre el nom d'usuari i la contrasenya. Si es correcte, es retorna un 1. Si es incorrecte, es retorna un 0
int ComprovarContrasenya(MYSQL *conn, char respuesta[512],char contrasenya[20], char nomusuari[20]) // =1 si l'usuari que tenim té la contrasenya que s'escriu
{
	MYSQL_RES *resultado;
	MYSQL_ROW row;	
	char consulta[120];
	
	strcpy (consulta,"SELECT JUGADOR.USERNAME FROM (JUGADOR) WHERE JUGADOR.USERNAME = '");
	strcat (consulta, nomusuari);
	strcat (consulta, "' AND JUGADOR.PASWORD = '");
	strcat (consulta, contrasenya);
	strcat (consulta, "'");
	
		
	int err=mysql_query (conn, consulta);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	resultado = mysql_store_result (conn);
	
	row = mysql_fetch_row (resultado);
	
	if (row == NULL)
	{
		return 0; // pq no s'han obtingut dades per tant no hi ha l'usuari
		strcpy(respuesta, "2");
	}
	else{
		return 1;
		strcpy(respuesta, "1");
	}
	
	mysql_close (conn);
	exit(0);
}

//Funció que retorna el numero total d'usuaris guardats a la base de dades. Si no hi ha cap, retorna un 0
int NumeroTotalUsuari(MYSQL *conn, char respuesta[512]) // ens retornara el num total d'usuaris, utilit per assiganr ID
{
	MYSQL_RES *resultado; 
	MYSQL_ROW row;
	char consulta[80];
	
	int numTot = 0;
	strcpy (consulta,"SELECT * FROM (JUGADOR)");
	
	int err = mysql_query (conn, consulta);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	resultado = mysql_store_result (conn);
	
	row = mysql_fetch_row (resultado);
	
	if (row == NULL)
		return 0;
	else
	{
		while (row !=NULL) {
			numTot = numTot +1;
			row = mysql_fetch_row (resultado);
		}
		return numTot;
	}
	strcpy(respuesta, "numTot");
	mysql_close (conn);
	exit(0);
}

//Un cop comprovades la contrasenya i que l'usuari no existeix a la base de dades, es registra a aquesta. La funció copia a la resposta 1 si s'ha pogut registrar o 0 en el cas de que l'usuari ja existeixi
void RegistrarUsuari(MYSQL *conn, char respuesta[512], char nomusuari[20],char contrasenya[80], int ID) // =1 es que s'ha registrat correctament; = 0 es qu el'usuari ja existia
{
	char consulta_nova[80];
	consulta_nova[0] = '\0';
	MYSQL_RES *resultado; 
	sprintf(consulta_nova,"INSERT INTO JUGADOR VALUES(%d,'%s','%s')",ID,nomusuari,contrasenya);
	int err = mysql_query (conn, consulta_nova);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	strcpy(respuesta, "1");
	return;	
	mysql_close (conn);
	exit(0);
}

//Aquesta funció busca els guanyadors d'una partida donat el ID d'aquesta com a referencia, i guarda els noms dels guanyadors a la resposta pasada com a parametre
void NomGuanyadorsPartida (MYSQL *conn, char respuesta[512], char* nombre){ //Nuria
	
	MYSQL_RES *resultado; 
	MYSQL_ROW row;
	char consulta[300];
	int err;
	
	strcpy (consulta,"SELECT JUGADOR.USERNAME FROM (JUGADOR,PARTIDA,PARTICIPACION) WHERE PARTIDA.ID = '");
	strcat (consulta, nombre);
	strcat (consulta,"'AND PARTICIPACION.ID_P = PARTIDA.ID AND PARTICIPACION.NOMEQUIP = PARTIDA.EQUIPGUANYADOR AND PARTICIPACION.ID_J = JUGADOR.ID");
	
	err=mysql_query (conn, consulta);
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	resultado = mysql_store_result (conn);
	
	row = mysql_fetch_row (resultado);
	printf("Estoy dentro de la funcion\n");
	if (row == NULL)
	{
		strcpy(respuesta,"-1");
		printf ("No se han obtenido datos en la consulta\n");
	}
	else{
		strcpy(respuesta,row[0]);
		row = mysql_fetch_row (resultado);
		while (row !=NULL) 
		{
			
			sprintf (respuesta,"%s,%s", respuesta, row[0]);
			row = mysql_fetch_row (resultado);
		}
	}
	return;
	mysql_close (conn);
	exit(0);
}

//Donat el nom d'una persona, pasat com a parametre, es busca a la base de dades la partida on aquest jugador ha guanyat amb més punts. El ID de la partida es guarda al parametre respuesta. Si no es tenen partides guanyades pel jugador, el ID de la partida sera -1
void PartidaMaximsPunts (MYSQL* conn, char respuesta[512], char* nombre){ //Jordi
	
	MYSQL_RES *resultado; 
	MYSQL_ROW row;
	int err;
	char consulta[300];
	
	strcpy (consulta,"SELECT DISTINCT PARTIDA.ID FROM (JUGADOR,PARTICIPACION,PARTIDA) WHERE PARTICIPACION.PUNTS IN ( SELECT DISTINCT MAX(PARTICIPACION.PUNTS) FROM (JUGADOR,PARTICIPACION) WHERE JUGADOR.USERNAME = '"); 
	strcat (consulta, nombre);
	strcat (consulta,"' AND JUGADOR.ID = PARTICIPACION.ID_J) AND PARTICIPACION.ID_P = PARTIDA.ID;");
	// hacemos la consulta 
	err=mysql_query (conn, consulta); 
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//recogemos el resultado de la consulta
	resultado = mysql_store_result (conn); 
	row = mysql_fetch_row (resultado);
	if (row == NULL)
	{
		strcpy(respuesta,"-1");
		printf ("No se han obtenido datos en la consulta\n");
	}
	else
	{
		// El resultado debe ser una matriz con una sola fila
		// y una columna que contiene el ID de la partida
		strcpy(respuesta, row[0]);
		printf ("ID de la partida que ha ganado con mas puntos: %s\n", row[0]);
	}
	return;
	mysql_close (conn);
	exit(0);
}

//Retorna al parametre resposta les persones que no han guanyat cap partida
void PersonaQueNoHaGuanyat(MYSQL *conn, char respuesta[512]){ //Joana
	
	MYSQL_RES *resultado; 
	MYSQL_ROW row;
	int err;
	
	err=mysql_query (conn, "SELECT JUGADOR.USERNAME FROM (JUGADOR,PARTIDA,PARTICIPACION) WHERE PARTICIPACION.NOMEQUIP NOT IN (SELECT PARTIDA.EQUIPGUANYADOR FROM PARTIDA) AND PARTICIPACION.ID_J=JUGADOR.ID;");
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	resultado = mysql_store_result (conn);
	
	row = mysql_fetch_row (resultado);
	
	if (row == NULL){
		printf ("No se han obtenido datos en la consulta\n");
	}
	else{
		strcpy(respuesta,row[0]);
		row = mysql_fetch_row (resultado);
		while (row !=NULL) 
		{
			
			sprintf (respuesta,"%s,%s", respuesta, row[0]);
			row = mysql_fetch_row (resultado);
		}
	}
	return;
	mysql_close (conn);
	exit(0);
}

//Creada una llista de partides, s'afegeix una persona a aquesta llista identificant la partida per l'ID enviat com a parametre. Si no existeix la partida buscada, es retornara un -1. Si s'ha realitzat el procés correctament, es retorna 0
int AfegirUsuariPartida(LlistaPartides *llista, char nom[20], int socket, int ID){
	int i = 0;
	int trobat = 0;
	
	
	while (i<llista->num && trobat==0)
	{
		if (llista->Partides[i].ID == ID)
		{
			trobat=1;			
		}
		else{
			i = i+1;
		}
		
	}
	if (trobat==1){ //ja existeix la partida
		int j = 0;		
		int trobat2 = 0;
		printf("%d\n",llista->Partides[i].numUsuaris);
		while ((j<llista->Partides[i].numUsuaris) && (trobat2==0))
		{
			printf("%d\n",llista->Partides[i].numUsuaris);
			if (strcmp(llista->Partides[i].UsuarisPartida[j].Usuari,nom)==0)
			{
				trobat2=1;
				printf("El usuari %s ja estava en aquesta partida\n",nom);
			}
			j = j+1;
		}
		printf("%d\n",llista->Partides[i].numUsuaris);
		if (trobat2==0)
		{
			
			llista->Partides[i].UsuarisPartida[llista->Partides[i].numUsuaris].Socket = socket;
			
			sprintf(llista->Partides[i].UsuarisPartida[llista->Partides[i].numUsuaris].Usuari,"%s",nom);
		
			printf("usuari %s afegit\n",llista->Partides[i].UsuarisPartida[llista->Partides[i].numUsuaris].Usuari);
			int num = llista->Partides[i].numUsuaris;
			printf("num abans: %d ",num);
			num++;
			llista->Partides[i].numUsuaris = num;
			printf("num usuaris: %d\n",llista->Partides[i].numUsuaris);
		}
		return 0;
	}
	else{ //no existeix la partida, crearem una
		return -1;
	}	

}

//Crea i afegeix un equip a una partida donat els noms dels dos integrants d'aquests
void AfegirEquipPartida(LlistaPartides *llistaP, char usuari1[20], char usuari2[20], int ID, int numEquip){
	
	int i = 0;
	int trobat = 0;
	
	while (i<llistaP->num && trobat==0)
	{
		if (llistaP->Partides[i].ID == ID)
		{
			trobat=1;			
		}
		else{
			i = i+1;
		}
		
	}
	if (trobat == 1){
		
		if (numEquip == 1){			
			sprintf(llistaP->Partides[i].Equip1,"%s,%s",usuari1,usuari2);			
		}
		else if (numEquip == 2){
			sprintf(llistaP->Partides[i].Equip1,"%s,%s",usuari1,usuari2);
		}
		else{
			printf("No has introduit numero de equip\n");
		}
		
	}
	else{
		printf("La partida no existeix\n");
	}
}

//Busca els diferents usuaris d'una partida i afegeix els seus noms a les distintes chars enviades com a parametres
void BuscarUsuarisPartida(LlistaPartides *llistaP, int ID, char nom1[20], char nom2[20], char nom3[20], char nom4[20]){
	//Retorna els noms dels usuaris d'una Partida, si la partida es de 1v1 nom3 i nom4 seran NO
	
	int i = 0;
	int trobat = 0;
	
	while (i<llistaP->num && trobat==0)
	{
		if (llistaP->Partides[i].ID == ID)
		{
			trobat=1;			
		}
		else{
			i = i+1;
		}
		
	}
	
	if (trobat == 1){
		
		if (strcmp(llistaP->Partides[i].mode,"1v1") == 0){
			
			sprintf(nom1,"%s",llistaP->Partides[i].UsuarisPartida[0].Usuari);
			sprintf(nom2,"%s",llistaP->Partides[i].UsuarisPartida[1].Usuari);
			
			strcpy(nom3,"NO");
			strcpy(nom4,"NO");
			
		}
		else{
			
			sprintf(nom1,"%s",llistaP->Partides[i].UsuarisPartida[0].Usuari);
			sprintf(nom2,"%s",llistaP->Partides[i].UsuarisPartida[1].Usuari);
			sprintf(nom3,"%s",llistaP->Partides[i].UsuarisPartida[2].Usuari);
			sprintf(nom4,"%s",llistaP->Partides[i].UsuarisPartida[3].Usuari);
			
		}		
		
	}
	else{
		printf("No existeix la partida\n");
	}
	
}

//Afegexix el mode d'una partida indicant el ID d'aquesta
void AfegirModePartida(LlistaPartides *llistaP, int ID, char mode[10])
{
	
	int i = 0;
	int trobat = 0;
	
	while (i<llistaP->num && trobat==0)
	{
		if (llistaP->Partides[i].ID == ID)
		{
			trobat=1;			
		}
		else{
			i = i+1;
		}
		
	}
	
	if (trobat ==1){
		
		strcpy(llistaP->Partides[i].mode,mode);
		
	}
	else{
		printf("No s'ha pogut canviar el mode\n");
	}
	
}

//Crea una partida amb un ID ja seleccionat. La primera persona afegida a la partida es el convidador, la persona que ha decidit crear la partida i convidar a altres jugadors.
void CrearPartida(LlistaPartides *llista, char nom[20], int socket, int ID){
	
	llista->Partides[llista->num].ID = ID;
	llista->Partides[llista->num].UsuarisPartida[0].Socket = socket;
	printf("Funcio crear partida, socket convidador: %d\n",llista->Partides[llista->num].UsuarisPartida[0].Socket);
	/*strcpy(llista->Partides[llista->num].UsuarisPartida[0].Usuari,nom);*/
	sprintf(llista->Partides[llista->num].UsuarisPartida[0].Usuari,"%s",nom);
	llista->Partides[llista->num].numUsuaris = 1;
	printf("Funcio crear partida, nom convidador: %s num usuaris %d\n",nom,llista->Partides[llista->num].numUsuaris);
	printf("Hem creat una nova partida, amb convidador: %s\n", llista->Partides[llista->num].UsuarisPartida[0].Usuari);
	llista->num = llista->num + 1;
	
}

//Donada la llista de partides i el ID de la partida on buscar, la funció modifica el parametre convidador amb el nom del usuari que ha creat aquesta partida, el qual es troba a la posició 0 d'usuaris de la partida
void MirarConvidadorPartida(LlistaPartides *llistaP, int ID, char convidador[20]){
	
	int i = 0;
	int trobat = 0;
	
	while (i<llistaP->num && trobat==0)
	{
		if (llistaP->Partides[i].ID == ID)
		{
			trobat=1;			
		}
		else{
			i = i+1;
		}		
	}
	if (trobat=1){
		strcpy(convidador,llistaP->Partides[i].UsuarisPartida[0].Usuari);
	}
	else{
		printf("No existeix la partida\n");
	}
}

// Funció per enviar la notificació amb la llista de usuaris conectats dins d'un char, la qual es enviada cada cop que una persona es registra o es desconecta
void EnviarLlistaConectatsNotificacio(int numSockets, int sockets[100], char UsuarisConectats[512]){
	// Funció per enviar la notificació amb la llista de usuaris conectats dins d'una string
	// Aqui preparamos una notificacion
	char notificacio[512];
	printf("Usuaris conectats: %s\n",UsuarisConectats);
	sprintf(notificacio,"7/%s",UsuarisConectats);// afegir el 7/ pk el client sapiga que esta rebent
	printf("Notificacio llista conectats: %s\n",notificacio);
	// usamos el vector de sockets para enviar la notificacion a todos los clientes conectados
	int j;
	for (j=0;j<numSockets;j++){//numConectados nos indica quantos sockets hay, es decir quantos conectados (en nuestro caso la i del vector de sockets)
		write(sockets[j],notificacio,strlen(notificacio));//enviamos a través de cada socket la notificacion
		
	}
	
}

//Aquest es el thread del servidor dedicat a atendre les peticions de cada client al mateix temps, permetent que tots aquests poguin tenir una connexió amb aquest
void *AtenderCliente(void *socket){
	
	MYSQL *conn;
	int err;
	
	//InicialitzarConexio(conn);
	
	//Creamos una conexion al servidor MYSQL 
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	//inicializar la conexion
	conn = mysql_real_connect (conn, "shiva2.upc.es","root", "mysql", "T2_BBDDJuego",0, NULL, 0);
	if (conn==NULL) {
		printf ("Error al inicializar la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	//En aquest punt es començen a rebre peticions al servidor
	
	int sock_conn; 
	int *s;
	s=(int *) socket;
	sock_conn = *s;
	
	char peticion[512];

	int ret;
	
	char nomusuari[20];
	char contrasenya[20];
	char ID_Partida[20];
	char NomJugador[20];
	
	//recibir i ejecutar peticion
	
	int terminar =0; //para terminar la conexion con el boton desconexion
	
	while (terminar ==0)//Terminar será diferent de 0 quan l'usuari es desconecti, on terminar pasará a ser 1
		{
		
		char respuesta[512];
		respuesta[0] = '\0';
		char UsuarisConectats[512];
		UsuarisConectats[0]='\0';
		
		// Ahora recibimos la petici?n
		ret=read(sock_conn, peticion, sizeof(peticion));
		printf ("Recibido\n");
		
		
		// Afegim la marca de final de linea per a que aquesta no afegeixi cap lletra durant el buffer
		peticion[ret]='\0';
		
		printf ("Peticion: %s\n",peticion);
		
		
		// vamos a ver que quieren
		char *p = strtok( peticion, "/");
		int codigo =  atoi (p);
		// Ya tenemos el c?digo de la petici?n
		
		printf ("CODI %d\n",codigo);
		if (codigo == 1)//Identifica que l'usuari vol entrar amb un usuari ja registrat
		{
			printf("He entrar al 1\n");
			p = strtok( NULL, "/");
			strcpy(nomusuari,p);
			p = strtok( NULL, "/");
			strcpy (contrasenya, p);
			//Ja tenim el nom del usuari, hem de mirar si coinicdeix amb un de la base de dades ==> fem una funcio per a aixó
			if (PertanyUsuari(conn, nomusuari) == 1){
				if (ComprovarContrasenya(conn, respuesta,contrasenya, nomusuari) == 1){
					strcpy(respuesta, "1/1");
					write (sock_conn,respuesta, strlen(respuesta));
					AfegirUsuariConectat(&llista,nomusuari,sock_conn);
					CadenaLlistaConectats(&llista,UsuarisConectats);
					EnviarLlistaConectatsNotificacio(numSockets,sockets,UsuarisConectats);
				}
				else{
					strcpy(respuesta, "1/2");
					write (sock_conn,respuesta, strlen(respuesta));
				}
				
			}
			
			else{
				strcpy(respuesta, "1/3");
				write (sock_conn,respuesta, strlen(respuesta));
			}
			
		}
		else if (codigo == 2){//Indica que un usuari es vol registrar per accedir al sistema
			int ID;
			p = strtok( NULL, "/");
			strcpy(nomusuari,p);
			p = strtok( NULL, "/");
			strcpy (contrasenya, p);
			if (PertanyUsuari(conn, nomusuari) == 1){
				strcpy(respuesta, "2/2");
				write (sock_conn,respuesta, strlen(respuesta));
			}
			else{
				ID = NumeroTotalUsuari(conn, respuesta) ;
				ID = ID + 1;
				printf("El ID es %d\n",ID);
				printf("Nomusuari: %s\n",nomusuari);
				RegistrarUsuari(conn, respuesta, nomusuari, contrasenya, ID);
				printf("%s\n",respuesta);
				strcpy(respuesta, "2/1");
				printf("%s\n",respuesta);
				write (sock_conn,respuesta, strlen(respuesta));
				AfegirUsuariConectat(&llista,nomusuari,sock_conn);
				CadenaLlistaConectats(&llista,UsuarisConectats);
				EnviarLlistaConectatsNotificacio(numSockets,sockets,UsuarisConectats);
			}
			
		}
		else if (codigo == 3)//Indica que es vol saber el nom dels guanyadors d'una partida
		{
			p = strtok( NULL, "/");
			strcpy (ID_Partida, p);
			NomGuanyadorsPartida (conn, respuesta, ID_Partida);
			sprintf(respuesta,"3/%s",respuesta);
			printf("Resposta codi 3: %s\n",respuesta);
			write (sock_conn,respuesta, strlen(respuesta));
		}
		
		else if (codigo == 4)//Indica que un cert usuari vol saber els punts maxims que ha realitzat una persona a una partida
		{
			p = strtok( NULL, "/");
			strcpy (NomJugador, p);
			PartidaMaximsPunts(conn, respuesta, NomJugador);
			sprintf(respuesta,"4/%s",respuesta);
			printf("Resposta codi 4: %s",respuesta);
			write (sock_conn,respuesta, strlen(respuesta));
		}
		else if (codigo == 5)//Indica que l'usuari vol saber quines persones no han guanyat cap partida
		{
			PersonaQueNoHaGuanyat(conn, respuesta);
			sprintf(respuesta,"5/%s",respuesta);
			printf("Resposta codi 5: %s",respuesta);
			write (sock_conn,respuesta, strlen(respuesta));				
		}
		else if (codigo == 8)//Utilitzat per afegir un usuari a una partida quan aquest el convidador
		{
			
			char convidador[20];
			ObtenirUsuaridesdeSocket(&llista,sock_conn,convidador);			
			sprintf(respuesta,"8/%s",convidador);
			
			//printf("El mensaje enviado a la persona que se invita es: %s\n",respuesta);
			p = strtok( NULL, "/");
			int ID_Partida = atoi(p);
			sprintf(respuesta,"%s,%d",respuesta,ID_Partida);
			CrearPartida(&llistaP,convidador,sock_conn,ID_Partida);
			p = strtok( NULL, "/");
			while (p != NULL)
			{
				strcpy (nomusuari, p);								
				int socketconvidat= ObtenerSocket(&llista,nomusuari);
				int afegit = AfegirUsuariPartida(&llistaP,nomusuari,socketconvidat,ID_Partida);
				printf("Hem afegit l'usuari %s a la partida\n",nomusuari);
				if (socketconvidat!=-1)
				{
					write(socketconvidat,respuesta,strlen(respuesta));
				}
				p = strtok( NULL, "/");				
			}
		}
		
		else if (codigo == 9)//Indica que el convidat ha respós SI o NO a la petició de jugar una partida i aquesta resposta s'envia a la persona convidadora per a que aquesta sigui conscient de qui participará a la partida
		{
			printf("Hem entrat al codi 9\n");
			p = strtok( NULL, "/");	
			int ID_Actual = atoi(p);
			printf("ID int : %d\n",ID_Actual);
			char ID_Actual2[10];
			strcpy(ID_Actual2,p);
			printf("ID string : %s\n",ID_Actual2);
			char Convidador[20];
			MirarConvidadorPartida(&llistaP,ID_Actual,Convidador);
			printf("Hem trobat convidador: %s\n",Convidador);
			int socket_conv = ObtenerSocket(&llista,Convidador);
			printf("Hem trobat el socket del convidador\n");
			//Preparem la resposta del estil 9/ID,convidat,SIoNO
			char Convidada[20];
			ObtenirUsuaridesdeSocket(&llista,sock_conn,Convidada);
			printf("Hem trobat la persona convidada: %s\n",Convidada);
			p = strtok( NULL, "/");	
			char SIoNO[10];
			strcpy(SIoNO,p);
			printf("La resposta de la persona es %s\n",SIoNO);
			sprintf(respuesta,"9/%s,%s,%s",ID_Actual2,Convidada,SIoNO);
			printf("Resposta Peticio 9: %s\n",respuesta);
			write(socket_conv,respuesta,strlen(respuesta));
			printf("Hem enviat la resposta\n");
		}
		
		else if (codigo == 10) // la peticio es de l'estil 10/IDPartida/mode/convidador/companyconvidador/nom1/nom2
		{
			
			char convidador1[20];
			char CompanyConvidador[20];
			char nom1[20];
			char nom2[20];
			int ID;
			char mode[10];
			int socket1;
			int socket2;
			int socket3;
			int socket4;
			char respuesta1[512];
			char respuesta2[512];
			
			//guardem les variables
			p = strtok( NULL, "/");
			ID = atoi(p);
			p = strtok(NULL,"/");
			strcpy(mode,p);	
			AfegirModePartida(&llistaP,ID,mode);
			p = strtok( NULL, "/");
			strcpy (convidador1, p);
			socket1 = ObtenerSocket(&llista,convidador1);
			p = strtok( NULL, "/");
			strcpy (CompanyConvidador, p);
			socket2 = ObtenerSocket(&llista,CompanyConvidador);
			//si es 2 contra 2, 4 jugadors
			
			if (strcmp(mode,"2v2")==0){
				p = strtok( NULL, "/");
				strcpy (nom1, p);	
				socket3 = ObtenerSocket(&llista,nom1);				
				p = strtok( NULL, "/");
				strcpy (nom2, p);
				socket4 = ObtenerSocket(&llista,nom2);
				
				AfegirEquipPartida(&llistaP,convidador1,CompanyConvidador,ID,1);
				AfegirEquipPartida(&llistaP,nom1,nom2,ID,2);
				
				// escriurem dos respostes diferents per a enviarles als dos equips diferents
				sprintf(respuesta1,"10/%d,%s,%s,%s,%s",ID,convidador1,CompanyConvidador,nom1,nom2);
				printf("resposta1 a peticio 10 : %s\n",respuesta1);
				
				sprintf(respuesta2,"10/%d,%s,%s,%s,%s",ID,nom1,nom2,convidador1,CompanyConvidador);
				printf("resposta2 a peticio 10 : %s\n",respuesta2);
				
				write(socket1,respuesta1,strlen(respuesta1));
				write(socket2,respuesta1,strlen(respuesta1));
				write(socket3,respuesta2,strlen(respuesta2));
				write(socket4,respuesta2,strlen(respuesta2));
			}
			else{ // 1 contra 1
				sprintf(respuesta,"10/%d,%s,%s",ID,convidador1,CompanyConvidador);
				printf("resposta a peticio 10 : %s\n",respuesta);
				
				write(socket1,respuesta,strlen(respuesta));
				write(socket2,respuesta,strlen(respuesta));
			}
		}
		
		else if (codigo == 11)
		{
			// rebem un missatge de l'estil 11/ID/missatge
			
			int ID;
			char Nom[20];
			char missatge[200];
			
			p = strtok(NULL,"/");
			ID = atoi(p);
			
			p = strtok(NULL,"/");
			strcpy(missatge,p);
			
			// Busquem els usuaris de la Partida
			char nom1[20];
			char nom2[20];
			char nom3[20];
			char nom4[20];
			
			BuscarUsuarisPartida(&llistaP,ID,nom1,nom2,nom3,nom4);
			printf("Els usuaris de la partida %d, son: %s %s %s %s\n",ID,nom1,nom2,nom3,nom4);
			
			// Preparem la resposta			
			
			ObtenirUsuaridesdeSocket(&llista,sock_conn,Nom);
			
			sprintf(respuesta, "11/%d_%s_%s", ID, Nom, missatge);
			printf("La respuesta es: %s\n",respuesta);
			
			//Obtenim els sockets de cada i enviem la resposta
			int socket1;
			int socket2;
			int socket3;
			int socket4;
			
			socket1 = ObtenerSocket(&llista,nom1);
			socket2 = ObtenerSocket(&llista,nom2);
			
			write(socket1,respuesta,strlen(respuesta));
			write(socket2,respuesta,strlen(respuesta));
			
			if (strcmp(nom3,"NO")!=0){ //4 jugadors
				
				socket3 = ObtenerSocket(&llista, nom3);
				socket4 = ObtenerSocket(&llista, nom4);
				
				write(socket3,respuesta,strlen(respuesta));
				write(socket4,respuesta,strlen(respuesta));
				
			}
			
		}
			
		else if (codigo == 0)//Indica que l'usuari es vol desconectar, i aquesta acció terminará amb el loop
		{
			printf("He entrar al 0\n");
			terminar =1;
			p = strtok( NULL, "/");
			strcpy (nomusuari, p);
			EliminarUsuariConectat(&llista,nomusuari);
			CadenaLlistaConectats(&llista,UsuarisConectats);
			EnviarLlistaConectatsNotificacio(numSockets,sockets,UsuarisConectats);
		}
		printf("No he fet cap numero\n");
		if (codigo!=0)
			printf("Esperando consulta\n");
		if ((codigo == 3) || (codigo == 4) || (codigo == 5))//SI es realitza qualsevol d'aquestes peticions, el contador de peticions afegeix una nova petició, i així es té un contador global de totes les peticions realitzades per tots els usuaris conectats
		{
			pthread_mutex_lock( &mutex);
			contador_serveis = contador_serveis +1;
			pthread_mutex_unlock( &mutex);
			//notificar a todos los clientes conectados los servicios realizados
			char char_contador_serveis[20];
			sprintf(char_contador_serveis,"6/%d",contador_serveis);
			int j;
			for(j=0;j<i;j++)
			{
				write(sockets[j],char_contador_serveis,strlen(char_contador_serveis));
			}
			
		}		
		
	}
	//Donem per finalitzat el servei amb aquest usuari i terminem el thread creat per a que aquest realitzés peticions
	close(sock_conn); 
	
}



int main(int argc, char *argv[])
{
	int PORT = 50054; //en tenim 3! 
	contador_serveis = 0;
	
	//InicialitzarSocket(int sock_listen, int PORT);
	llista.num=0;
	llistaP.num=0;
	//INICIEM ELS SOCKETS
	int sock_conn, sock_listen, ret;
	struct sockaddr_in serv_adr;
	
	// INICIALITZACIONS
	
	// Obrim el socket
	if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0)
		printf("Error creant socket");
	
	// Fem el bind al port
	memset(&serv_adr, 0, sizeof(serv_adr));// inicialitza a zero serv_addr
	serv_adr.sin_family = AF_INET;
	// asocia el socket a cualquiera de las IP de la m?quina. 
	
	//htonl formatea el numero que recibe al formato necesario
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY);
	
	// establecemos el puerto de escucha
	serv_adr.sin_port = htons(PORT);
	
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
		printf ("Error al bind");
	
	
	if (listen(sock_listen, 3) < 0)
		printf("Error en el Listen");
	//Socket en el que vamos a estar escuchando i La cola de peticiones pendientes no podra ser superior a 3
	
	
	pthread_t thread[100]; //vector on guardarem el identificador de thread de cada client
	//recibir conexion i peticiones
	// Bucle infinito
	for (;;){
		
		//RecibirConexion(sock_conn,sock_listen);
		printf("Escoltant\n");
		
		sock_conn = accept(sock_listen, NULL, NULL);		
		printf("He rebut conexió\n");
		
		sockets[numSockets] = sock_conn;
		
		pthread_create(&thread[numSockets], NULL, AtenderCliente, &sockets[numSockets]);
		numSockets = numSockets+1;
		i=i+1;
	}
}

