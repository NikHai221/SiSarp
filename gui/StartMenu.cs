using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace com.example
{
    /// <summary>
    /// Hlavné menu aplikácie so šachovnicovým pozadím.
    /// Dedené od Window.
    /// </summary>
    public class StartMenu : Window
    {
        private Canvas root;
        private StackPanel menuBox;

        public StartMenu()
        {
            Start();
        }

        private void Start()
        {
            this.Title = "Šach - Menu";
            this.Width = 420; // Prispôsobené veľkosti šachovnice + okraje okna
            this.Height = 440;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ResizeMode = ResizeMode.NoResize;

            this.root = new Canvas(); // Základný koreň (nahrádza Group z Javy)

            // Šachovnicové pozadie
            this.vytvorSachovnicuPozadie(root);

            // Tlačidlá
            Button playButton = new Button { Content = "Play", FontSize = 16, Padding = new Thickness(20, 10, 20, 10), Margin = new Thickness(0, 0, 0, 20) };
            Button settingsButton = new Button { Content = "Nastavenia", FontSize = 16, Padding = new Thickness(20, 10, 20, 10), Margin = new Thickness(0, 0, 0, 20) };
            Button exitButton = new Button { Content = "Ukončiť", FontSize = 16, Padding = new Thickness(20, 10, 20, 10) };

            playButton.Click += (s, e) => {
                try
                {
                    // Zavolanie hry (V predchádzajúcom kóde to bolo triedou App, 
                    // v C# verzii sme ju nazvali MainWindow)
                    MainWindow hra = new MainWindow();
                    hra.Show();
                    this.Close(); // Zatvorí menu
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };

            settingsButton.Click += (s, e) => {
                this.zobrazNastavenia(this);
                // Po zatvorení nastavení prekreslíme pozadie, aby sa aplikovali nové farby
                ObnovPozadie(); 
            };
            
            exitButton.Click += (s, e) => Application.Current.Shutdown();

            this.menuBox = new StackPanel();
            this.menuBox.Children.Add(playButton);
            this.menuBox.Children.Add(settingsButton);
            this.menuBox.Children.Add(exitButton);

            // Vycentrovanie menuBoxu na Canvas
            Canvas.SetLeft(menuBox, 130);
            Canvas.SetTop(menuBox, 100);

            this.root.Children.Add(menuBox);

            this.Content = root;
        }

        /// <summary>
        /// Vytvorí šachovnicové pozadie.
        /// </summary>
        private void vytvorSachovnicuPozadie(Canvas canvas)
        {
            int velkostPolicka = 50;
            for (int riadok = 0; riadok < 8; riadok++)
            {
                for (int stlpec = 0; stlpec < 8; stlpec++)
                {
                    Rectangle policko = new Rectangle();
                    policko.Width = velkostPolicka;
                    policko.Height = velkostPolicka;
                    
                    Canvas.SetLeft(policko, stlpec * velkostPolicka);
                    Canvas.SetTop(policko, riadok * velkostPolicka);

                    if ((riadok + stlpec) % 2 == 0)
                    {
                        policko.Fill = Nastavenia.getSvetlaFarba();
                    }
                    else
                    {
                        policko.Fill = Nastavenia.getTmavaFarba();
                    }

                    canvas.Children.Add(policko);
                }
            }
        }

        /// <summary>
        /// Zobrazí nastavenia šachovnice.
        /// </summary>
        private void zobrazNastavenia(Window stage)
        {
            Nastavenia.zobrazNastavenia(stage);
        }

        /// <summary>
        /// Obnoví grafické plátno pre zobrazenie nových farieb
        /// </summary>
        private void ObnovPozadie()
        {
            this.root.Children.Clear();
            this.vytvorSachovnicuPozadie(this.root);
            this.root.Children.Add(this.menuBox); // Znovu pridať tlačidlá navrch
        }
    }
}