using System;
using System.Linq;
using System.Windows;

namespace com.example
{
    public class Program
    {
        // [STAThread] je pre WPF absolútne nutné, inak grafika spadne
        [STAThread]
        public static void Main(string[] args)
        {
            
            
            
            // Spustenie CLI verzie
            CliHra cliHra = new CliHra();
            cliHra.Spusti();
            
        }
    }
}