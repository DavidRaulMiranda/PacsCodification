
namespace PacsCodification
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_codifica = new System.Windows.Forms.Button();
            this.btnGeneraCodi = new System.Windows.Forms.Button();
            this.btnDecodifica = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_codifica
            // 
            this.btn_codifica.Location = new System.Drawing.Point(48, 116);
            this.btn_codifica.Name = "btn_codifica";
            this.btn_codifica.Size = new System.Drawing.Size(75, 23);
            this.btn_codifica.TabIndex = 0;
            this.btn_codifica.Text = "Codifica";
            this.btn_codifica.UseVisualStyleBackColor = true;
            this.btn_codifica.Click += new System.EventHandler(this.btn_codifica_Click);
            // 
            // btnGeneraCodi
            // 
            this.btnGeneraCodi.Location = new System.Drawing.Point(48, 65);
            this.btnGeneraCodi.Name = "btnGeneraCodi";
            this.btnGeneraCodi.Size = new System.Drawing.Size(75, 23);
            this.btnGeneraCodi.TabIndex = 1;
            this.btnGeneraCodi.Text = "GeneraCodi";
            this.btnGeneraCodi.UseVisualStyleBackColor = true;
            this.btnGeneraCodi.Click += new System.EventHandler(this.btnGeneraCodi_Click);
            // 
            // btnDecodifica
            // 
            this.btnDecodifica.Location = new System.Drawing.Point(562, 79);
            this.btnDecodifica.Name = "btnDecodifica";
            this.btnDecodifica.Size = new System.Drawing.Size(75, 23);
            this.btnDecodifica.TabIndex = 2;
            this.btnDecodifica.Text = "Decodifica";
            this.btnDecodifica.UseVisualStyleBackColor = true;
            this.btnDecodifica.Click += new System.EventHandler(this.btnDecodifica_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(48, 173);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Compara";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(712, 255);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnDecodifica);
            this.Controls.Add(this.btnGeneraCodi);
            this.Controls.Add(this.btn_codifica);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_codifica;
        private System.Windows.Forms.Button btnGeneraCodi;
        private System.Windows.Forms.Button btnDecodifica;
        private System.Windows.Forms.Button button1;
    }
}

