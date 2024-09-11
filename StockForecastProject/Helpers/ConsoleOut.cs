using System;
using System.Collections.Generic;

namespace StockForecastProject
{
    public static class ConsoleOut
    {
        public static void WriteInfo(string text) => WriteColoredLine(ConsoleColor.Blue, text);
        public static void WriteWarning(string text) => WriteColoredLine(ConsoleColor.DarkYellow, text);
        public static void WriteError(string text) => WriteColoredLine(ConsoleColor.Red, text);

        /// <summary>
        /// Belirtilen renkte ve satır sonu ekleyerek metin yazdırır.
        /// </summary>
        /// <param name="clr">Konsol metin rengi.</param>
        /// <param name="text">Yazdırılacak metin.</param>
        /// <param name="pad">Satır sonu eklemek için kullanılan yeni satır sayısı. Varsayılan değer 1'dir.</param>
        public static void WriteColoredLine(ConsoleColor clr, string text, int pad = 1) => WriteColored(clr, text + new string('\n', pad));

        /// <summary>
        /// Belirtilen renkte metni konsola yazdırır.
        /// </summary>
        /// <param name="clr">Konsol metin rengi.</param>
        /// <param name="text">Yazdırılacak metin.</param>
        public static void WriteColored(ConsoleColor clr, string text)
        {
            Console.ForegroundColor = clr;
            Console.Write($"{text}");
            Console.ResetColor();
        }
        /// <summary>
        /// Programın sonunda yazılacak metni konsola yazdırır.
        /// </summary>
        public static void ProgramEnd()
        {
            Console.WriteLine(Environment.NewLine + "Programı sonlandırmak için bir herhangi bir tuşa basın...");
            Console.ReadKey();
        }

        public static void ClearLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, Console.CursorTop);
        }
    }
    /// <summary>
    /// Konsola çok satırlı ve renkli metin yazdırmak için kullanılan sınıf.
    /// </summary>
    public class MultiLineColoredText
    {
        private readonly List<(ConsoleColor, string)> _lines = new List<(ConsoleColor, string)>();
        /// <summary>
        /// Belirtilen renk ve metni listeye ekler.
        /// </summary>
        /// <param name="clr">Konsolda kullanılacak metin rengi.</param>
        /// <param name="text">Listeye eklenecek metin.</param>
        public void AddLine(ConsoleColor clr, string text) => _lines.Add((clr, text));

        public void AddText(string text) => _lines.Add((ConsoleColor.Gray, text));
        public void AddInfo(string text) => _lines.Add((ConsoleColor.Blue, text));
        public void AddWarning(string text) => _lines.Add((ConsoleColor.DarkYellow, text));
        public void AddError(string text) => _lines.Add((ConsoleColor.Red, text));

        /// <summary>
        /// Listeye eklenen tüm satırları, belirlenen renklerle konsola yazdırır.
        /// </summary>
        public void WriteAllLines()
        {
            foreach (var (clr, text) in _lines)
            {
                Console.ForegroundColor = clr;
                Console.WriteLine(text);
            }
            Console.ResetColor(); // Yazdırma işlemi tamamlandıktan sonra rengi sıfırlar.
            _lines.Clear(); // Yazdırılan satırları temizler.
        }
    }
}
