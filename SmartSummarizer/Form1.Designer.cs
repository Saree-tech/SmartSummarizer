namespace SmartSummarizer
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtInput = new System.Windows.Forms.TextBox();
            this.btnSummarize = new System.Windows.Forms.Button();
            this.rtbOutput = new System.Windows.Forms.RichTextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblInputTitle = new System.Windows.Forms.Label();
            this.lblOutputTitle = new System.Windows.Forms.Label();
            this.grpInput = new System.Windows.Forms.GroupBox();
            this.grpOutput = new System.Windows.Forms.GroupBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnTestApi = new System.Windows.Forms.Button();
            this.btnListModels = new System.Windows.Forms.Button();  // Added
            this.grpInput.SuspendLayout();
            this.grpOutput.SuspendLayout();
            this.SuspendLayout();

            // 
            // grpInput
            // 
            this.grpInput.Controls.Add(this.lblInputTitle);
            this.grpInput.Controls.Add(this.txtInput);
            this.grpInput.Location = new System.Drawing.Point(12, 12);
            this.grpInput.Name = "grpInput";
            this.grpInput.Size = new System.Drawing.Size(760, 228);
            this.grpInput.TabIndex = 0;
            this.grpInput.TabStop = false;

            // 
            // lblInputTitle
            // 
            this.lblInputTitle.AutoSize = true;
            this.lblInputTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblInputTitle.Location = new System.Drawing.Point(6, 16);
            this.lblInputTitle.Name = "lblInputTitle";
            this.lblInputTitle.Size = new System.Drawing.Size(120, 19);
            this.lblInputTitle.TabIndex = 0;
            this.lblInputTitle.Text = "Text to Summarize:";

            // 
            // txtInput
            // 
            this.txtInput.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtInput.Location = new System.Drawing.Point(6, 38);
            this.txtInput.Multiline = true;
            this.txtInput.Name = "txtInput";
            this.txtInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInput.Size = new System.Drawing.Size(748, 180);
            this.txtInput.TabIndex = 1;
            this.txtInput.TextChanged += new System.EventHandler(this.txtInput_TextChanged);

            // 
            // btnSummarize
            // 
            this.btnSummarize.BackColor = System.Drawing.Color.SteelBlue;
            this.btnSummarize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSummarize.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnSummarize.ForeColor = System.Drawing.Color.White;
            this.btnSummarize.Location = new System.Drawing.Point(150, 246);
            this.btnSummarize.Name = "btnSummarize";
            this.btnSummarize.Size = new System.Drawing.Size(150, 45);
            this.btnSummarize.TabIndex = 2;
            this.btnSummarize.Text = "Summarize (Ctrl+S)";
            this.btnSummarize.UseVisualStyleBackColor = false;
            this.btnSummarize.Click += new System.EventHandler(this.btnSummarize_Click);

            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.LightGray;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnClear.Location = new System.Drawing.Point(306, 246);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(90, 45);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);

            // 
            // btnTestApi
            // 
            this.btnTestApi.BackColor = System.Drawing.Color.Orange;
            this.btnTestApi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTestApi.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnTestApi.Location = new System.Drawing.Point(402, 246);
            this.btnTestApi.Name = "btnTestApi";
            this.btnTestApi.Size = new System.Drawing.Size(100, 45);
            this.btnTestApi.TabIndex = 4;
            this.btnTestApi.Text = "Test API";
            this.btnTestApi.UseVisualStyleBackColor = false;
            this.btnTestApi.Click += new System.EventHandler(this.btnTestApi_Click);

            // 
            // btnListModels
            // 
            this.btnListModels.BackColor = System.Drawing.Color.Purple;
            this.btnListModels.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnListModels.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnListModels.ForeColor = System.Drawing.Color.White;
            this.btnListModels.Location = new System.Drawing.Point(508, 246);
            this.btnListModels.Name = "btnListModels";
            this.btnListModels.Size = new System.Drawing.Size(100, 45);
            this.btnListModels.TabIndex = 5;
            this.btnListModels.Text = "List Models";
            this.btnListModels.UseVisualStyleBackColor = false;
            this.btnListModels.Click += new System.EventHandler(this.btnListModels_Click);

            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 297);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(760, 10);
            this.progressBar.TabIndex = 6;
            this.progressBar.Visible = false;

            // 
            // grpOutput
            // 
            this.grpOutput.Controls.Add(this.lblOutputTitle);
            this.grpOutput.Controls.Add(this.rtbOutput);
            this.grpOutput.Location = new System.Drawing.Point(12, 313);
            this.grpOutput.Name = "grpOutput";
            this.grpOutput.Size = new System.Drawing.Size(760, 228);
            this.grpOutput.TabIndex = 7;
            this.grpOutput.TabStop = false;

            // 
            // lblOutputTitle
            // 
            this.lblOutputTitle.AutoSize = true;
            this.lblOutputTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblOutputTitle.Location = new System.Drawing.Point(6, 16);
            this.lblOutputTitle.Name = "lblOutputTitle";
            this.lblOutputTitle.Size = new System.Drawing.Size(74, 19);
            this.lblOutputTitle.TabIndex = 0;
            this.lblOutputTitle.Text = "Summary:";

            // 
            // rtbOutput
            // 
            this.rtbOutput.BackColor = System.Drawing.Color.White;
            this.rtbOutput.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.rtbOutput.Location = new System.Drawing.Point(6, 38);
            this.rtbOutput.Name = "rtbOutput";
            this.rtbOutput.ReadOnly = true;
            this.rtbOutput.Size = new System.Drawing.Size(748, 180);
            this.rtbOutput.TabIndex = 1;
            this.rtbOutput.Text = "";

            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStatus.Location = new System.Drawing.Point(12, 544);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(149, 15);
            this.lblStatus.TabIndex = 8;
            this.lblStatus.Text = "Ready to summarize text.";

            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(784, 568);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.grpOutput);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnListModels);
            this.Controls.Add(this.btnTestApi);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSummarize);
            this.Controls.Add(this.grpInput);
            this.Name = "Form1";
            this.Text = "SmartSummarizer - AI Text Summarizer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.grpInput.ResumeLayout(false);
            this.grpInput.PerformLayout();
            this.grpOutput.ResumeLayout(false);
            this.grpOutput.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}