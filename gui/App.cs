using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using com.example.Logika;
using com.example.Obrazky;

namespace com.example
{
    // Vo WPF trieda zvyčajne dedí od Window namiesto JavaFX Application
    public partial class MainWindow : Window
    {
        private Plocha plocha = new Plocha(); // Objekt šachovnice, ktorý uchováva aktuálny stav hry
        private Canvas koren = null!; // Koreňový uzol grafického rozhrania (ekvivalent JavaFX Group/Pane)
        private int[]? vybranaPozicia;

        public MainWindow()
        {
            // Štandardná inicializácia WPF
            // Ak nemáš XAML súbor, všetko vytvoríme v kóde tak ako v JavaFX:
            Start();
        }

        /// <summary>
        /// Metóda na spustenie aplikácie a nastavenie scény.
        /// </summary>
        private void Start()
        {
            this.koren = new Canvas();
            this.koren.Background = Brushes.Black; // Čierne pozadie
            this.Content = this.koren;

            this.Title = "Šach";
            this.Width = 420; // Pridané rozmery okna (8x50 + okraje)
            this.Height = 440;

            // Nastavenie callbacku pre zobrazenie výsledku
            this.plocha.setCallbackVysledku(sprava => {
                // UI update musí byť na hlavnom vlákne
                Application.Current.Dispatcher.Invoke(() => {
                    MessageBox.Show(sprava, "Koniec hry", MessageBoxButton.OK, MessageBoxImage.Information);
                });
            });

            this.vytvorSachovnicu(); // Vytvorenie grafickej šachovnice
            this.pridajObrazok(); // Pridanie figúrok na šachovnicu

            // Nastavenie obsluhy udalosti kliknutia myšou na plátno
            this.koren.MouseLeftButtonDown += OnMouseClicked;
        }

        /// <summary>
        /// Obsluha kliknutia myšou na Canvas.
        /// </summary>
        private void OnMouseClicked(object sender, MouseButtonEventArgs e)
        {
            int[] pozicia = this.getPoziciuMysi(e); // Získa x a y

            // Zabezpečenie: Ak by používateľ klikol úplne mimo plochy
            if (pozicia[0] < 0 || pozicia[0] > 7 || pozicia[1] < 0 || pozicia[1] > 7)
            {
                return;
            }

            if (this.vybranaPozicia == null)
            {
                // SME PRI PRVOM KLIKNUTÍ
                // Uložíme pozíciu IBA vtedy, ak na tom políčku reálne nejaká figúrka stojí.
                // Tvoja logika používa obrátené indexy (riadok = [1], stĺpec = [0])
                int riadok = pozicia[1];
                int stlpec = pozicia[0];

                if (this.plocha.getFigurka(riadok, stlpec) != null)
                {
                    this.vybranaPozicia = pozicia; // OK, chytili sme figúrku
                }
            }
            else
            {
                // SME PRI DRUHOM KLIKNUTÍ (vyberáme, kam má figúrka ísť)
                int startX = this.vybranaPozicia[1];
                int startY = this.vybranaPozicia[0];
                int endX = pozicia[1];
                int endY = pozicia[0];
                
                this.plocha.setFigurka(startX, startY, endX, endY); // Aktualizácia šachovnice

                // Po akomkoľvek pokuse o presun (či už bol úspešný alebo nie) pustíme figúrku z ruky
                this.vybranaPozicia = null;
                this.restartujPlochu(); // Obnovenie grafiky šachovnice
            }
        }

        /// <summary>
        /// Metóda na vytvorenie šachovnice.
        /// </summary>
        private void vytvorSachovnicu()
        {
            int velkostPolicka = 50; // Veľkosť jedného políčka na šachovnici

            for (int riadok = 0; riadok < 8; riadok++)
            {
                for (int stlpec = 0; stlpec < 8; stlpec++)
                {
                    Rectangle policko = new Rectangle();
                    policko.Width = velkostPolicka;
                    policko.Height = velkostPolicka;
                    
                    // Poziciovanie vo WPF Canvas
                    Canvas.SetLeft(policko, stlpec * velkostPolicka);
                    Canvas.SetTop(policko, riadok * velkostPolicka);

                    // Striedanie farieb políčok
                    // (Predpokladám, že Nastavenia.getSvetlaFarba() vracia SolidColorBrush)
                    if ((riadok + stlpec) % 2 == 0)
                    {
                        policko.Fill = Nastavenia.getSvetlaFarba();
                    }
                    else
                    {
                        policko.Fill = Nastavenia.getTmavaFarba();
                    }

                    this.koren.Children.Add(policko); // Pridanie políčka na Canvas
                }
            }
        }

        /// <summary>
        /// Metóda na pridanie figúrok na šachovnicu.
        /// </summary>
        private void pridajObrazok()
{
    for (int i = 0; i < 8; i++)
    {
        for (int j = 0; j < 8; j++)
        {
            // 1. Získanie figúrky z aktuálnej pozície na šachovnici
            Figurka figurka = this.plocha.getFigurka(i, j);

            // 2. Ak na políčku nie je figúrka, preskočíme ho
            if (figurka == null)
            {
                continue;
            }

            // 3. Získame názov súboru pomocou pomocnej triedy
            ObrazFigurky infoOObrazku = new ObrazFigurky(figurka);
            string nazovSuboru = infoOObrazku.getObrazok();

            try
            {
                // 4. Vytvorenie cesty k obrázku (Pack URI pre WPF)
                // Predpokladá sa, že obrázky sú v zložke "Obrazky" a majú Build Action nastavený na "Resource"
                string uriCesta = $"pack://application:,,,/Obrazky/{nazovSuboru}";
                
                BitmapImage bitmapa = new BitmapImage(new Uri(uriCesta, UriKind.Absolute));

                // 5. Vytvorenie grafického prvku Image
                Image pohladObrazu = new Image();
                pohladObrazu.Source = bitmapa;
                pohladObrazu.Width = 50;
                pohladObrazu.Height = 50;

                // 6. Nastavenie pozície na plátne (j je stĺpec/X, i je riadok/Y)
                Canvas.SetLeft(pohladObrazu, j * 50);
                Canvas.SetTop(pohladObrazu, i * 50);

                // 7. Pridanie obrázka do koreňového uzla
                this.koren.Children.Add(pohladObrazu);
            }
            catch (Exception ex)
            {
                // Debugging: Ak sa konkrétny obrázok nepodarí načítať, dozviete sa ktorý a prečo
                MessageBox.Show($"Chyba pri načítaní figúrky: {nazovSuboru}\nDetaily: {ex.Message}");
            }
        }
    }
}

        /// <summary>
        /// Metóda na získanie pozície myši na šachovnici.
        /// </summary>
        private int[] getPoziciuMysi(MouseButtonEventArgs eventArgs)
        {
            Point poziciaMysi = eventArgs.GetPosition(this.koren);
            int x = (int)(poziciaMysi.X / 50); // Vypočíta stĺpec podľa X súradnice
            int y = (int)(poziciaMysi.Y / 50); // Vypočíta riadok podľa Y súradnice
            return new int[] { x, y }; // Vráti pozíciu ako pole [riadok, stĺpec]
        }

        /// <summary>
        /// Metóda na obnovenie grafického zobrazenia šachovnice.
        /// </summary>
        private void restartujPlochu()
        {
            this.koren.Children.Clear(); // Vymaže všetky objekty z plátna
            this.vytvorSachovnicu(); // Opätovné vytvorenie šachovnice
            this.pridajObrazok(); // Opätovné pridanie figúrok
        }
    }
}