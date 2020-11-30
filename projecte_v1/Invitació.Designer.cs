namespace WindowsFormsApplication1
{
    partial class Invitació
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
            this.label1 = new System.Windows.Forms.Label();
            this.Acceptar = new System.Windows.Forms.Button();
            this.Rebutjar = new System.Windows.Forms.Button();
            this.Missatge = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(72, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 17);
            this.label1.TabIndex = 0;
            // 
            // Acceptar
            // 
            this.Acceptar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Acceptar.Location = new System.Drawing.Point(141, 109);
            this.Acceptar.Name = "Acceptar";
            this.Acceptar.Size = new System.Drawing.Size(118, 60);
            this.Acceptar.TabIndex = 1;
            this.Acceptar.Text = "Acceptar invitació";
            this.Acceptar.UseVisualStyleBackColor = true;
            this.Acceptar.Click += new System.EventHandler(this.Acceptar_Click);
            // 
            // Rebutjar
            // 
            this.Rebutjar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Rebutjar.Location = new System.Drawing.Point(377, 109);
            this.Rebutjar.Name = "Rebutjar";
            this.Rebutjar.Size = new System.Drawing.Size(120, 60);
            this.Rebutjar.TabIndex = 2;
            this.Rebutjar.Text = "Rebutjar invitació";
            this.Rebutjar.UseVisualStyleBackColor = true;
            this.Rebutjar.Click += new System.EventHandler(this.Rebutjar_Click);
            // 
            // Missatge
            // 
            this.Missatge.AutoSize = true;
            this.Missatge.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Missatge.Location = new System.Drawing.Point(31, 55);
            this.Missatge.Name = "Missatge";
            this.Missatge.Size = new System.Drawing.Size(23, 25);
            this.Missatge.TabIndex = 3;
            this.Missatge.Text = "2";
            // 
            // Invitació
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(715, 210);
            this.Controls.Add(this.Missatge);
            this.Controls.Add(this.Rebutjar);
            this.Controls.Add(this.Acceptar);
            this.Controls.Add(this.label1);
            this.Location = new System.Drawing.Point(100, 100);
            this.Name = "Invitació";
            this.Text = "Invitació";
            this.Load += new System.EventHandler(this.Invitació_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Acceptar;
        private System.Windows.Forms.Button Rebutjar;
        private System.Windows.Forms.Label Missatge;
    }
}