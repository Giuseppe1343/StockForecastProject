namespace StockForecasting
{
    partial class FrmStock
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
            grdStocks = new DataGridView();
            lblDescription = new Label();
            panel1 = new Panel();
            lblPreprocessIndicator = new Label();
            lblPredictionIndicator = new Label();
            lblDataIndicator = new Label();
            barPreprocessIndicator = new ProgressBar();
            barPredictionIndicator = new ProgressBar();
            barDataIndicator = new ProgressBar();
            ((System.ComponentModel.ISupportInitialize)grdStocks).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // grdStocks
            // 
            grdStocks.AllowUserToAddRows = false;
            grdStocks.AllowUserToDeleteRows = false;
            grdStocks.AllowUserToOrderColumns = true;
            grdStocks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grdStocks.Dock = DockStyle.Fill;
            grdStocks.Location = new Point(0, 40);
            grdStocks.Name = "grdStocks";
            grdStocks.ReadOnly = true;
            grdStocks.Size = new Size(784, 435);
            grdStocks.TabIndex = 1;
            grdStocks.CellMouseDoubleClick += grdStocks_CellMouseDoubleClickAsync;
            // 
            // lblDescription
            // 
            lblDescription.Dock = DockStyle.Top;
            lblDescription.Location = new Point(0, 0);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(784, 40);
            lblDescription.TabIndex = 2;
            lblDescription.Text = "Tahmin Sonuçları İçin Stoğu Listeden Çift Tıklayarak Açınız. ";
            lblDescription.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            panel1.Controls.Add(lblPreprocessIndicator);
            panel1.Controls.Add(lblPredictionIndicator);
            panel1.Controls.Add(lblDataIndicator);
            panel1.Controls.Add(barPreprocessIndicator);
            panel1.Controls.Add(barPredictionIndicator);
            panel1.Controls.Add(barDataIndicator);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 475);
            panel1.Name = "panel1";
            panel1.Size = new Size(784, 86);
            panel1.TabIndex = 3;
            // 
            // lblPreprocessIndicator
            // 
            lblPreprocessIndicator.Font = new Font("Segoe UI", 9F);
            lblPreprocessIndicator.Location = new Point(10, 32);
            lblPreprocessIndicator.Name = "lblPreprocessIndicator";
            lblPreprocessIndicator.Size = new Size(136, 23);
            lblPreprocessIndicator.TabIndex = 1;
            lblPreprocessIndicator.Text = "Toplam";
            lblPreprocessIndicator.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblPredictionIndicator
            // 
            lblPredictionIndicator.Font = new Font("Segoe UI", 9F);
            lblPredictionIndicator.Location = new Point(10, 59);
            lblPredictionIndicator.Name = "lblPredictionIndicator";
            lblPredictionIndicator.Size = new Size(136, 23);
            lblPredictionIndicator.TabIndex = 1;
            lblPredictionIndicator.Text = "Toplam";
            lblPredictionIndicator.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblDataIndicator
            // 
            lblDataIndicator.Font = new Font("Segoe UI", 9F);
            lblDataIndicator.Location = new Point(10, 5);
            lblDataIndicator.Name = "lblDataIndicator";
            lblDataIndicator.Size = new Size(136, 23);
            lblDataIndicator.TabIndex = 1;
            lblDataIndicator.Text = "Toplam";
            lblDataIndicator.TextAlign = ContentAlignment.MiddleRight;
            // 
            // barPreprocessIndicator
            // 
            barPreprocessIndicator.Location = new Point(152, 32);
            barPreprocessIndicator.Name = "barPreprocessIndicator";
            barPreprocessIndicator.Size = new Size(627, 23);
            barPreprocessIndicator.Step = 1;
            barPreprocessIndicator.TabIndex = 0;
            // 
            // barPredictionIndicator
            // 
            barPredictionIndicator.Location = new Point(152, 59);
            barPredictionIndicator.Name = "barPredictionIndicator";
            barPredictionIndicator.Size = new Size(627, 23);
            barPredictionIndicator.Step = 1;
            barPredictionIndicator.TabIndex = 0;
            // 
            // barDataIndicator
            // 
            barDataIndicator.Location = new Point(152, 5);
            barDataIndicator.Name = "barDataIndicator";
            barDataIndicator.Size = new Size(627, 23);
            barDataIndicator.Step = 1;
            barDataIndicator.TabIndex = 0;
            // 
            // FrmStock
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 561);
            Controls.Add(grdStocks);
            Controls.Add(panel1);
            Controls.Add(lblDescription);
            Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 162);
            Name = "FrmStock";
            Text = "Stock Forecasting";
            ((System.ComponentModel.ISupportInitialize)grdStocks).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private DataGridView grdStocks;
        private Label lblDescription;
        private Panel panel1;
        private Label lblDataIndicator;
        private ProgressBar barDataIndicator;
        private ProgressBar barPredictionIndicator;
        private Label lblPredictionIndicator;
        private Label lblPreprocessIndicator;
        private ProgressBar barPreprocessIndicator;
    }
}
