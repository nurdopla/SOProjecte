namespace WindowsFormsApplication1
{
    partial class form_inicial
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BotoEntrar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.NomUsuari = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.RegistrarUsuari = new System.Windows.Forms.Button();
            this.Desconnectar = new System.Windows.Forms.Button();
            this.Contrasenya = new System.Windows.Forms.TextBox();
            this.DonamNomGuanyadorsPartidaButton = new System.Windows.Forms.RadioButton();
            this.ID_Partida_TextBox = new System.Windows.Forms.TextBox();
            this.DonamPartidaMaximPuntsJugadorButton = new System.Windows.Forms.RadioButton();
            this.Nom_Jugador_TextBox = new System.Windows.Forms.TextBox();
            this.DonamPersonesQueNoHanGuanyatButton = new System.Windows.Forms.RadioButton();
            this.EnviarPeticionButton = new System.Windows.Forms.Button();
            this.count_lbl = new System.Windows.Forms.Label();
            this.LlistaConectats = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.Botó_Convidar = new System.Windows.Forms.Button();
            this.TitolPersonesConvidadesLabel = new System.Windows.Forms.Label();
            this.PersonesConvidadesLabel = new System.Windows.Forms.Label();
            this.ID_text_label = new System.Windows.Forms.Label();
            this.ID_label = new System.Windows.Forms.Label();
            this.Començar_button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.LlistaConectats)).BeginInit();
            this.SuspendLayout();
            // 
            // BotoEntrar
            // 
            this.BotoEntrar.Location = new System.Drawing.Point(72, 113);
            this.BotoEntrar.Name = "BotoEntrar";
            this.BotoEntrar.Size = new System.Drawing.Size(119, 42);
            this.BotoEntrar.TabIndex = 0;
            this.BotoEntrar.Text = "Entrar";
            this.BotoEntrar.UseVisualStyleBackColor = true;
            this.BotoEntrar.Click += new System.EventHandler(this.BotoEntrar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "NOM USUARI";
            // 
            // NomUsuari
            // 
            this.NomUsuari.Location = new System.Drawing.Point(127, 32);
            this.NomUsuari.Name = "NomUsuari";
            this.NomUsuari.Size = new System.Drawing.Size(172, 22);
            this.NomUsuari.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "CONTRASENYA";
            // 
            // RegistrarUsuari
            // 
            this.RegistrarUsuari.Location = new System.Drawing.Point(72, 169);
            this.RegistrarUsuari.Name = "RegistrarUsuari";
            this.RegistrarUsuari.Size = new System.Drawing.Size(119, 42);
            this.RegistrarUsuari.TabIndex = 5;
            this.RegistrarUsuari.Text = "Registrar-se";
            this.RegistrarUsuari.UseVisualStyleBackColor = true;
            this.RegistrarUsuari.Click += new System.EventHandler(this.RegistrarUsuari_Click);
            // 
            // Desconnectar
            // 
            this.Desconnectar.Location = new System.Drawing.Point(525, 373);
            this.Desconnectar.Name = "Desconnectar";
            this.Desconnectar.Size = new System.Drawing.Size(194, 42);
            this.Desconnectar.TabIndex = 6;
            this.Desconnectar.Text = "DESCONNECTAR-SE";
            this.Desconnectar.UseVisualStyleBackColor = true;
            this.Desconnectar.Click += new System.EventHandler(this.Desconnectar_Click);
            // 
            // Contrasenya
            // 
            this.Contrasenya.Location = new System.Drawing.Point(127, 73);
            this.Contrasenya.Name = "Contrasenya";
            this.Contrasenya.Size = new System.Drawing.Size(172, 22);
            this.Contrasenya.TabIndex = 7;
            // 
            // DonamNomGuanyadorsPartidaButton
            // 
            this.DonamNomGuanyadorsPartidaButton.AutoSize = true;
            this.DonamNomGuanyadorsPartidaButton.Location = new System.Drawing.Point(22, 252);
            this.DonamNomGuanyadorsPartidaButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DonamNomGuanyadorsPartidaButton.Name = "DonamNomGuanyadorsPartidaButton";
            this.DonamNomGuanyadorsPartidaButton.Size = new System.Drawing.Size(337, 21);
            this.DonamNomGuanyadorsPartidaButton.TabIndex = 8;
            this.DonamNomGuanyadorsPartidaButton.TabStop = true;
            this.DonamNomGuanyadorsPartidaButton.Text = "Dona\'m els noms dels guanyadors de la partida: ";
            this.DonamNomGuanyadorsPartidaButton.UseVisualStyleBackColor = true;
            // 
            // ID_Partida_TextBox
            // 
            this.ID_Partida_TextBox.Location = new System.Drawing.Point(348, 250);
            this.ID_Partida_TextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ID_Partida_TextBox.Name = "ID_Partida_TextBox";
            this.ID_Partida_TextBox.Size = new System.Drawing.Size(89, 22);
            this.ID_Partida_TextBox.TabIndex = 9;
            // 
            // DonamPartidaMaximPuntsJugadorButton
            // 
            this.DonamPartidaMaximPuntsJugadorButton.AutoSize = true;
            this.DonamPartidaMaximPuntsJugadorButton.Location = new System.Drawing.Point(22, 276);
            this.DonamPartidaMaximPuntsJugadorButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DonamPartidaMaximPuntsJugadorButton.Name = "DonamPartidaMaximPuntsJugadorButton";
            this.DonamPartidaMaximPuntsJugadorButton.Size = new System.Drawing.Size(510, 21);
            this.DonamPartidaMaximPuntsJugadorButton.TabIndex = 10;
            this.DonamPartidaMaximPuntsJugadorButton.TabStop = true;
            this.DonamPartidaMaximPuntsJugadorButton.Text = "Dona\'m la ID de la partida on el jugador amb el maxim de punts del jugador: ";
            this.DonamPartidaMaximPuntsJugadorButton.UseVisualStyleBackColor = true;
            // 
            // Nom_Jugador_TextBox
            // 
            this.Nom_Jugador_TextBox.Location = new System.Drawing.Point(525, 274);
            this.Nom_Jugador_TextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Nom_Jugador_TextBox.Name = "Nom_Jugador_TextBox";
            this.Nom_Jugador_TextBox.Size = new System.Drawing.Size(108, 22);
            this.Nom_Jugador_TextBox.TabIndex = 11;
            // 
            // DonamPersonesQueNoHanGuanyatButton
            // 
            this.DonamPersonesQueNoHanGuanyatButton.AutoSize = true;
            this.DonamPersonesQueNoHanGuanyatButton.Location = new System.Drawing.Point(22, 300);
            this.DonamPersonesQueNoHanGuanyatButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DonamPersonesQueNoHanGuanyatButton.Name = "DonamPersonesQueNoHanGuanyatButton";
            this.DonamPersonesQueNoHanGuanyatButton.Size = new System.Drawing.Size(432, 21);
            this.DonamPersonesQueNoHanGuanyatButton.TabIndex = 12;
            this.DonamPersonesQueNoHanGuanyatButton.TabStop = true;
            this.DonamPersonesQueNoHanGuanyatButton.Text = "Dona\'m els noms dels jugadors que no han guanyat cap partida";
            this.DonamPersonesQueNoHanGuanyatButton.UseVisualStyleBackColor = true;
            // 
            // EnviarPeticionButton
            // 
            this.EnviarPeticionButton.Location = new System.Drawing.Point(72, 373);
            this.EnviarPeticionButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.EnviarPeticionButton.Name = "EnviarPeticionButton";
            this.EnviarPeticionButton.Size = new System.Drawing.Size(119, 40);
            this.EnviarPeticionButton.TabIndex = 13;
            this.EnviarPeticionButton.Text = "Enviar";
            this.EnviarPeticionButton.UseVisualStyleBackColor = true;
            this.EnviarPeticionButton.Click += new System.EventHandler(this.EnviarPeticionButton_Click);
            // 
            // count_lbl
            // 
            this.count_lbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.count_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.count_lbl.Location = new System.Drawing.Point(271, 377);
            this.count_lbl.Name = "count_lbl";
            this.count_lbl.Size = new System.Drawing.Size(194, 39);
            this.count_lbl.TabIndex = 17;
            // 
            // LlistaConectats
            // 
            this.LlistaConectats.AllowUserToAddRows = false;
            this.LlistaConectats.AllowUserToDeleteRows = false;
            this.LlistaConectats.AllowUserToResizeColumns = false;
            this.LlistaConectats.AllowUserToResizeRows = false;
            this.LlistaConectats.BackgroundColor = System.Drawing.SystemColors.Control;
            this.LlistaConectats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.LlistaConectats.ColumnHeadersVisible = false;
            this.LlistaConectats.Location = new System.Drawing.Point(587, 52);
            this.LlistaConectats.Name = "LlistaConectats";
            this.LlistaConectats.ReadOnly = true;
            this.LlistaConectats.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.LlistaConectats.RowTemplate.Height = 24;
            this.LlistaConectats.Size = new System.Drawing.Size(152, 175);
            this.LlistaConectats.TabIndex = 19;
            this.LlistaConectats.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.LlistaConectats_CellClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(584, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(160, 17);
            this.label3.TabIndex = 20;
            this.label3.Text = "Llista Usuaris Conectats";
            // 
            // Botó_Convidar
            // 
            this.Botó_Convidar.Location = new System.Drawing.Point(443, 81);
            this.Botó_Convidar.Name = "Botó_Convidar";
            this.Botó_Convidar.Size = new System.Drawing.Size(111, 41);
            this.Botó_Convidar.TabIndex = 21;
            this.Botó_Convidar.Text = "Convidar";
            this.Botó_Convidar.UseVisualStyleBackColor = true;
            this.Botó_Convidar.Click += new System.EventHandler(this.Botó_Convidar_Click);
            // 
            // TitolPersonesConvidadesLabel
            // 
            this.TitolPersonesConvidadesLabel.AutoSize = true;
            this.TitolPersonesConvidadesLabel.Location = new System.Drawing.Point(424, 139);
            this.TitolPersonesConvidadesLabel.Name = "TitolPersonesConvidadesLabel";
            this.TitolPersonesConvidadesLabel.Size = new System.Drawing.Size(148, 17);
            this.TitolPersonesConvidadesLabel.TabIndex = 22;
            this.TitolPersonesConvidadesLabel.Text = "Persones convidades:";
            // 
            // PersonesConvidadesLabel
            // 
            this.PersonesConvidadesLabel.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.PersonesConvidadesLabel.Location = new System.Drawing.Point(414, 155);
            this.PersonesConvidadesLabel.Name = "PersonesConvidadesLabel";
            this.PersonesConvidadesLabel.Size = new System.Drawing.Size(167, 26);
            this.PersonesConvidadesLabel.TabIndex = 23;
            // 
            // ID_text_label
            // 
            this.ID_text_label.AutoSize = true;
            this.ID_text_label.Location = new System.Drawing.Point(500, 194);
            this.ID_text_label.Name = "ID_text_label";
            this.ID_text_label.Size = new System.Drawing.Size(25, 17);
            this.ID_text_label.TabIndex = 24;
            this.ID_text_label.Text = "ID:";
            // 
            // ID_label
            // 
            this.ID_label.AutoSize = true;
            this.ID_label.Location = new System.Drawing.Point(532, 194);
            this.ID_label.Name = "ID_label";
            this.ID_label.Size = new System.Drawing.Size(0, 17);
            this.ID_label.TabIndex = 25;
            // 
            // Començar_button
            // 
            this.Començar_button.Location = new System.Drawing.Point(395, 186);
            this.Començar_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Començar_button.Name = "Començar_button";
            this.Començar_button.Size = new System.Drawing.Size(100, 33);
            this.Començar_button.TabIndex = 26;
            this.Començar_button.Text = "Començar";
            this.Començar_button.UseVisualStyleBackColor = true;
            this.Començar_button.Click += new System.EventHandler(this.Començar_button_Click);
            // 
            // form_inicial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 470);
            this.Controls.Add(this.Començar_button);
            this.Controls.Add(this.ID_label);
            this.Controls.Add(this.ID_text_label);
            this.Controls.Add(this.PersonesConvidadesLabel);
            this.Controls.Add(this.TitolPersonesConvidadesLabel);
            this.Controls.Add(this.Botó_Convidar);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LlistaConectats);
            this.Controls.Add(this.count_lbl);
            this.Controls.Add(this.EnviarPeticionButton);
            this.Controls.Add(this.DonamPersonesQueNoHanGuanyatButton);
            this.Controls.Add(this.Nom_Jugador_TextBox);
            this.Controls.Add(this.DonamPartidaMaximPuntsJugadorButton);
            this.Controls.Add(this.ID_Partida_TextBox);
            this.Controls.Add(this.DonamNomGuanyadorsPartidaButton);
            this.Controls.Add(this.Contrasenya);
            this.Controls.Add(this.Desconnectar);
            this.Controls.Add(this.RegistrarUsuari);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.NomUsuari);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BotoEntrar);
            this.Name = "form_inicial";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.form_inicial_Load);
            ((System.ComponentModel.ISupportInitialize)(this.LlistaConectats)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BotoEntrar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox NomUsuari;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button RegistrarUsuari;
        private System.Windows.Forms.Button Desconnectar;
        private System.Windows.Forms.TextBox Contrasenya;
        private System.Windows.Forms.RadioButton DonamNomGuanyadorsPartidaButton;
        private System.Windows.Forms.TextBox ID_Partida_TextBox;
        private System.Windows.Forms.RadioButton DonamPartidaMaximPuntsJugadorButton;
        private System.Windows.Forms.TextBox Nom_Jugador_TextBox;
        private System.Windows.Forms.RadioButton DonamPersonesQueNoHanGuanyatButton;
        private System.Windows.Forms.Button EnviarPeticionButton;
        private System.Windows.Forms.Label count_lbl;
        private System.Windows.Forms.DataGridView LlistaConectats;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Botó_Convidar;
        private System.Windows.Forms.Label TitolPersonesConvidadesLabel;
        private System.Windows.Forms.Label PersonesConvidadesLabel;
        private System.Windows.Forms.Label ID_text_label;
        private System.Windows.Forms.Label ID_label;
        private System.Windows.Forms.Button Començar_button;
    }
}

