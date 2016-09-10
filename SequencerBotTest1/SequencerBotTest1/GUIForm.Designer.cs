namespace SequencerBotTest1
{
    partial class GUIForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GUIForm));
      this.OutputLabel = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // OutputLabel
      // 
      this.OutputLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.OutputLabel.Font = new System.Drawing.Font("OCR A Extended", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.OutputLabel.ForeColor = System.Drawing.Color.White;
      this.OutputLabel.Location = new System.Drawing.Point(-4, -1);
      this.OutputLabel.Name = "OutputLabel";
      this.OutputLabel.Size = new System.Drawing.Size(526, 159);
      this.OutputLabel.TabIndex = 2;
      this.OutputLabel.Text = "Debug";
      // 
      // GUIForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.Black;
      this.ClientSize = new System.Drawing.Size(522, 157);
      this.Controls.Add(this.OutputLabel);
      this.ForeColor = System.Drawing.Color.Black;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "GUIForm";
      this.Text = "Sequencer Bot";
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GUIForm_FormClosed);
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label OutputLabel;
    }
}

