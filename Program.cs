using System;
using System.Linq;
using System.Windows;

namespace com.example
{
    public class Program
    {
        
        [STAThread]
        public static void Main(string[] args)
        {
            CliHra cliHra = new CliHra();
            cliHra.Spusti();
        }
    }
}