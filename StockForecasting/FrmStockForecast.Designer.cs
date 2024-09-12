namespace StockForecasting
{
    partial class FrmStockForecast
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
            lblDescription = new Label();
            chartControl1 = new Syncfusion.Windows.Forms.Chart.ChartControl();
            SuspendLayout();
            // 
            // lblDescription
            // 
            lblDescription.Dock = DockStyle.Top;
            lblDescription.Location = new Point(0, 0);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(800, 40);
            lblDescription.TabIndex = 4;
            lblDescription.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // chartControl1
            // 
            chartControl1.ChartArea.CursorLocation = new Point(0, 0);
            chartControl1.ChartArea.CursorReDraw = false;
            chartControl1.IsWindowLess = false;
            // 
            // 
            // 
            chartControl1.Legend.Location = new Point(667, 75);
            chartControl1.Localize = null;
            chartControl1.Location = new Point(12, 43);
            chartControl1.Name = "chartControl1";
            chartControl1.PrimaryXAxis.LogLabelsDisplayMode = Syncfusion.Windows.Forms.Chart.LogLabelsDisplayMode.Default;
            chartControl1.PrimaryXAxis.Margin = true;
            chartControl1.PrimaryYAxis.LogLabelsDisplayMode = Syncfusion.Windows.Forms.Chart.LogLabelsDisplayMode.Default;
            chartControl1.PrimaryYAxis.Margin = true;
            chartControl1.Size = new Size(776, 395);
            chartControl1.TabIndex = 7;
            chartControl1.Text = "chartControl1";
            // 
            // 
            // 
            chartControl1.Title.Name = "Default";
            chartControl1.Titles.Add(chartControl1.Title);
            chartControl1.VisualTheme = "";
            // 
            // FrmStockForecast
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(chartControl1);
            Controls.Add(lblDescription);
            Name = "FrmStockForecast";
            Text = "FrmStockForecast";
            ResumeLayout(false);
        }

        #endregion
        private Label lblDescription;
        private Syncfusion.Windows.Forms.Chart.ChartControl chartControl1;
    }
}