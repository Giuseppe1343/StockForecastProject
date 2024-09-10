using System;
using System.Threading;

namespace Stok_Tahmin_Modeli.Helpers
{
    /// <summary>
    /// Konsolda bir yükleme animasyonu gerçekleştiren sınıf.
    /// </summary>
    public class LoadingAnimation
    {
        readonly char[] chars = { '\\', '|', '/', '-' };
        bool _isRunning = false;

        CancellationTokenSource _cts;
        Thread _load;

        /// <summary>
        /// Yükleme animasyonunun çalışıp çalışmadığını belirten bir özelliktir.
        /// </summary>
        /// <returns>Animasyon çalışıyorsa true, değilse false döner.</returns>
        public bool IsRunning => _isRunning;
        /// <summary>
        /// Yükleme animasyonunu başlatmak için kullanılır.
        /// </summary>
        public LoadingAnimation() { }

        /// <summary>
        /// Yükleme animasyonunu başlatır.
        /// </summary>
        public void Start()
        {
            if (_isRunning)
                return;
            _cts = new CancellationTokenSource();
            var token = _cts.Token;
            _load = new Thread(() => Loading(token));
            _load.Start();
        }
        /// <summary>
        /// Yükleme animasyonunu durdurur.
        /// </summary>
        public void Stop()
        {
            if (!_isRunning)
                return;
            _cts.Cancel();
            _load.Join();
        }
        /// <summary>
        /// Yükleme animasyonunun işleyişini kontrol eder. Belirtilen iptal token'ı kullanılarak animasyon durdurulabilir.
        /// </summary>
        /// <param name="ct">Animasyonu iptal etmek için kullanılan CancellationToken.</param>
        private void Loading(CancellationToken ct)
        {
            _isRunning = true;
            Console.CursorVisible = false;

            var clear = Console.GetCursorPosition(); // Save the current cursor position for clearing

            Console.Write("Loading ");
            var pos = Console.GetCursorPosition(); // Save the position of the word "Loading" to create animation next to it
            int count = 0;

            while (!ct.IsCancellationRequested)
            {
                Console.SetCursorPosition(pos.Left, pos.Top);
                Console.Write(chars[count++ % 4]);
                Thread.Sleep(100);
            }

            Console.SetCursorPosition(clear.Left,clear.Top); // Clearing

            Console.CursorVisible = true;
            _isRunning = false;
        }
    }
}
