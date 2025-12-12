using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace algo_projet_final
{
    internal class TerminalClass
    {
        // importation pour activer l'ANSI sur le terminal
        private const int STD_OUTPUT_HANDLE = -11;
        private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 4;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        public TerminalClass()
        {
            var handle = GetStdHandle(STD_OUTPUT_HANDLE);

            // On obtient le mode de la console
            uint mode;
            if (!GetConsoleMode(handle, out mode))
            {
                Console.Error.WriteLine("Failed to get console mode");
                return;
            }

            // On active le mode ANSI
            mode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;
            if (!SetConsoleMode(handle, mode))
            {
                Console.Error.WriteLine("Failed to set console mode");
                return;
            }
        }

        public void ClearTerminal()
        {
            Console.Clear();
        }

        public string SetTextColor(int r, int g, int b)
        {
            return $"\x1b[38;2;{r};{g};{b}m";
        }

        public string SetBackgroundColor(int r, int g, int b)
        {
            return $"\x1b[48;2;{r};{g};{b}m";
        }

        public string ResetEffect()
        {
            return "\x1b[0m";
        }

        public string SetBold()
        {
            return "\x1b[1m";
        }

        public string SetItalic()
        {
            return "\x1b[3m";
        }

        public string SetUnderline()
        {
            return "\x1b[4m";
        }
    }
}
