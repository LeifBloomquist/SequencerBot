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
      this.OutputLabel = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // OutputLabel
      // 
      this.OutputLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.OutputLabel.Location = new System.Drawing.Point(12, 9);
      this.OutputLabel.Name = "OutputLabel";
      this.OutputLabel.Size = new System.Drawing.Size(498, 129);
      this.OutputLabel.TabIndex = 2;
      this.OutputLabel.Text = "Debug";
      // 
      // GUIForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(522, 157);
      this.Controls.Add(this.OutputLabel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.Name = "GUIForm";
      this.Text = "Sequencer Bot";
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GUIForm_FormClosed);
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label OutputLabel;
    }
}

