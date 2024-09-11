namespace StockForecasting
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Subscribe to the MessageReceived event to get the log messages
            MessageOutput.MessageReceived += (sender, e) =>
            {
                switch (e.Type)
                {
                    case LogLevel.Info:
                        MessageBox.Show(e.Message, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case LogLevel.Error:
                        MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case LogLevel.Warning:
                        MessageBox.Show(e.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                }
            };

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new FrmStock());
        }
    }
}