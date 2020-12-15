namespace WindowsFormsApplication1
{
    partial class Formar_equips
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
            this.nom1 = new System.Windows.Forms.RadioButton();
            this.nom2 = new System.Windows.Forms.RadioButton();
            this.nom3 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.boto_ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // nom1
            // 
            this.nom1.AutoSize = true;
            this.nom1.Location = new System.Drawing.Point(77, 81);
            this.nom1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nom1.Name = "nom1";
            this.nom1.Size = new System.Drawing.Size(69, 21);
            this.nom1.TabIndex = 9;
            this.nom1.TabStop = true;
            this.nom1.Text = "NOM1";
            this.nom1.UseVisualStyleBackColor = true;
            
            // 
            // nom2
            // 
            this.nom2.AutoSize = true;
            this.nom2.Location = new System.Drawing.Point(140, 121);
            this.nom2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nom2.Name = "nom2";
            this.nom2.Size = new System.Drawing.Size(69, 21);
            this.nom2.TabIndex = 10;
            this.nom2.TabStop = true;
            this.nom2.Text = "NOM2";
            this.nom2.UseVisualStyleBackColor = true;
            // 
            // nom3
            // 
            this.nom3.AutoSize = true;
            this.nom3.Location = new System.Drawing.Point(209, 81);
            this.nom3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nom3.Name = "nom3";
            this.nom3.Size = new System.Drawing.Size(69, 21);
            this.nom3.TabIndex = 11;
            this.nom3.TabStop = true;
            this.nom3.Text = "NOM3";
            this.nom3.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(327, 17);
            this.label1.TabIndex = 25;
            this.label1.Text = "SELECCIONA UN USUARI PER A FORMAR EQUIP";
            // 
            // boto_ok
            // 
            this.boto_ok.Location = new System.Drawing.Point(137, 167);
            this.boto_ok.Name = "boto_ok";
            this.boto_ok.Size = new System.Drawing.Size(72, 25);
            this.boto_ok.TabIndex = 26;
            this.boto_ok.Text = "OK";
            this.boto_ok.UseVisualStyleBackColor = true;
            this.boto_ok.Click += new System.EventHandler(this.boto_ok_Click);
            // 
            // Formar_equips
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 217);
            this.Controls.Add(this.boto_ok);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nom3);
            this.Controls.Add(this.nom2);
            this.Controls.Add(this.nom1);
            this.Name = "Formar_equips";
            this.Text = "Formar_equips";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton nom1;
        private System.Windows.Forms.RadioButton nom2;
        private System.Windows.Forms.RadioButton nom3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button boto_ok;

    }
}