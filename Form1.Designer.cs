namespace CoolItDown
{
    partial class CoolItDownForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            HeaderPanel = new Panel();
            label1 = new Label();
            FooterPanel = new Panel();
            restartButton = new Button();
            GamePanel = new Panel();
            HeaderPanel.SuspendLayout();
            FooterPanel.SuspendLayout();
            SuspendLayout();
            // 
            // HeaderPanel
            // 
            HeaderPanel.BackColor = Color.FromArgb(42, 42, 64);
            HeaderPanel.Controls.Add(label1);
            HeaderPanel.Dock = DockStyle.Top;
            HeaderPanel.Location = new Point(0, 0);
            HeaderPanel.Name = "HeaderPanel";
            HeaderPanel.Padding = new Padding(10);
            HeaderPanel.Size = new Size(884, 80);
            HeaderPanel.TabIndex = 0;
            HeaderPanel.Paint += HeaderPanel_Paint;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Left;
            label1.Font = new Font("Segoe UI", 18F);
            label1.ForeColor = Color.Coral;
            label1.Location = new Point(10, 10);
            label1.Name = "label1";
            label1.Size = new Size(193, 32);
            label1.TabIndex = 0;
            label1.Text = "🔥 Cool It Down";
            // 
            // FooterPanel
            // 
            FooterPanel.BackColor = Color.FromArgb(42, 42, 64);
            FooterPanel.Controls.Add(restartButton);
            FooterPanel.Dock = DockStyle.Bottom;
            FooterPanel.ForeColor = SystemColors.ActiveCaptionText;
            FooterPanel.Location = new Point(0, 591);
            FooterPanel.Name = "FooterPanel";
            FooterPanel.Padding = new Padding(10);
            FooterPanel.Size = new Size(884, 70);
            FooterPanel.TabIndex = 1;
            // 
            // restartButton
            // 
            restartButton.BackColor = Color.Coral;
            restartButton.Cursor = Cursors.Hand;
            restartButton.FlatAppearance.BorderSize = 0;
            restartButton.FlatStyle = FlatStyle.Flat;
            restartButton.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            restartButton.ForeColor = Color.White;
            restartButton.Location = new Point(381, 13);
            restartButton.Name = "restartButton";
            restartButton.Size = new Size(120, 40);
            restartButton.TabIndex = 0;
            restartButton.Text = "Restart";
            restartButton.UseVisualStyleBackColor = false;
            // 
            // GamePanel
            // 
            GamePanel.Dock = DockStyle.Fill;
            GamePanel.Location = new Point(0, 80);
            GamePanel.Name = "GamePanel";
            GamePanel.Padding = new Padding(20);
            GamePanel.Size = new Size(884, 511);
            GamePanel.TabIndex = 2;
            // 
            // CoolItDownForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 47);
            ClientSize = new Size(884, 661);
            Controls.Add(GamePanel);
            Controls.Add(FooterPanel);
            Controls.Add(HeaderPanel);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "CoolItDownForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Cool It Down";
            HeaderPanel.ResumeLayout(false);
            HeaderPanel.PerformLayout();
            FooterPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel HeaderPanel;
        private Label label1;
        private Panel FooterPanel;
        private Button restartButton;
        private Panel GamePanel;
    }
}
