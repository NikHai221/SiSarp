using System;
using System.Windows;

namespace com.example
{
    public class Program
    {
        // [STAThread] je pre WPF absolútne nutné, inak grafika spadne
        [STAThread]
        public static void Main(string[] args)
        {
            // Inicializácia a spustenie hlavného cyklu WPF aplikácie
            Application app = new Application();
            
            // Otvorenie úvodného okna (StartMenu)
            app.Run(new StartMenu());
        }
    }
}