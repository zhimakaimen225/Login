namespace Gd
{
    partial class PrintForm
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
            this.buttonPrintSetting = new System.Windows.Forms.Button();
            this.buttonPreview = new System.Windows.Forms.Button();
            this.buttonPrint = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonPrintSetting
            // 
            this.buttonPrintSetting.Location = new System.Drawing.Point(42, 212);
            this.buttonPrintSetting.Name = "buttonPrintSetting";
            this.buttonPrintSetting.Size = new System.Drawing.Size(62, 23);
            this.buttonPrintSetting.TabIndex = 0;
            this.buttonPrintSetting.Text = "打印设置";
            this.buttonPrintSetting.UseVisualStyleBackColor = true;
            this.buttonPrintSetting.Click += new System.EventHandler(this.buttonPrintSetting_Click);
            // 
            // buttonPreview
            // 
            this.buttonPreview.Location = new System.Drawing.Point(110, 212);
            this.buttonPreview.Name = "buttonPreview";
            this.buttonPreview.Size = new System.Drawing.Size(62, 23);
            this.buttonPreview.TabIndex = 1;
            this.buttonPreview.Text = "打印预览";
            this.buttonPreview.UseVisualStyleBackColor = true;
            this.buttonPreview.Click += new System.EventHandler(this.buttonPreview_Click);
            // 
            // buttonPrint
            // 
            this.buttonPrint.Location = new System.Drawing.Point(178, 212);
            this.buttonPrint.Name = "buttonPrint";
            this.buttonPrint.Size = new System.Drawing.Size(62, 23);
            this.buttonPrint.TabIndex = 2;
            this.buttonPrint.Text = "打印";
            this.buttonPrint.UseVisualStyleBackColor = true;
            // 
            // PrintForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.buttonPrint);
            this.Controls.Add(this.buttonPreview);
            this.Controls.Add(this.buttonPrintSetting);
            this.Name = "PrintForm";
            this.Text = "PrintForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonPrintSetting;
        private System.Windows.Forms.Button buttonPreview;
        private System.Windows.Forms.Button buttonPrint;
    }
}