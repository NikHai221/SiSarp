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
    public partial class MainWindow : Window
    {
        private Plocha plocha = new Plocha();
        private Canvas koren = null!;
        private int[]? vybranaPozicia;

        // DEFINÍCIA FARIEB PRIAMO V TRIEDE (nahrádza starú triedu Nastavenia)
        // Použité sú príjemné klasické šachové odtiene (krémová a hnedá)
        private readonly Brush svetlaFarba = new SolidColorBrush(Color.FromRgb(240, 217, 181)); 
        private readonly Brush tmavaFarba = new SolidColorBrush(Color.FromRgb(181, 136, 99));

        public MainWindow()
        {
            Start();
        }

        // KONŠTRUKTOR PRE PRECHOD Z CLI KEDYKOĽVEK POČAS HRY
        public MainWindow(Plocha existujucaPlocha)
        {
            this.plocha = existujucaPlocha; 
            Start();
        }

        private void Start()
        {
            this.koren = new Canvas();
            this.koren.Background = Brushes.Black;
            this.Content = this.koren;

            this.Title = "Šach";
            this.Width = 420; 
            this.Height = 440;

            this.plocha.setCallbackVysledku(sprava => {
                Application.Current.Dispatcher.Invoke(() => {
                    MessageBox.Show(sprava, "Koniec hry", MessageBoxButton.OK, MessageBoxImage.Information);
                });
            });

            this.vytvorSachovnicu();
            this.pridajObrazok();

            this.koren.MouseLeftButtonDown += OnMouseClicked;
        }

        private void OnMouseClicked(object sender, MouseButtonEventArgs e)
        {
            int[] pozicia = this.getPoziciuMysi(e);

            if (pozicia[0] < 0 || pozicia[0] > 7 || pozicia[1] < 0 || pozicia[1] > 7)
            {
                return;
            }

            if (this.vybranaPozicia == null)
            {
                int riadok = pozicia[1];
                int stlpec = pozicia[0];

                if (this.plocha.getFigurka(riadok, stlpec) != null)
                {
                    this.vybranaPozicia = pozicia;
                }
            }
            else
            {
                int startX = this.vybranaPozicia[1];
                int startY = this.vybranaPozicia[0];
                int endX = pozicia[1];
                int endY = pozicia[0];
                
                this.plocha.setFigurka(startX, startY, endX, endY);

                this.vybranaPozicia = null;
                this.restartujPlochu();
            }
        }

        private void vytvorSachovnicu()
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
                    // OTOČENIE Y SÚRADNICE: Logický riadok 0 bude nakreslený dole
                    Canvas.SetTop(policko, (7 - riadok) * velkostPolicka);

                    // POUŽITIE LOKÁLNYCH PREMENNÝCH PRE FARBU
                    if ((riadok + stlpec) % 2 != 0) 
                    {
                        policko.Fill = this.svetlaFarba;
                    }
                    else
                    {
                        policko.Fill = this.tmavaFarba;
                    }

                    this.koren.Children.Add(policko);
                }
            }
        }

        private void pridajObrazok()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Figurka figurka = this.plocha.getFigurka(i, j);

                    if (figurka == null)
                    {
                        continue;
                    }

                    ObrazFigurky infoOObrazku = new ObrazFigurky(figurka);
                    string nazovSuboru = infoOObrazku.getObrazok();

                    try
                    {
                        // OPRAVENÁ CESTA NA gui/Obrazky
                        string uriCesta = $"pack://application:,,,/gui/Obrazky/{nazovSuboru}";
                        BitmapImage bitmapa = new BitmapImage(new Uri(uriCesta, UriKind.Absolute));

                        Image pohladObrazu = new Image();
                        pohladObrazu.Source = bitmapa;
                        pohladObrazu.Width = 50;
                        pohladObrazu.Height = 50;

                        Canvas.SetLeft(pohladObrazu, j * 50);
                        // OTOČENIE Y SÚRADNICE PRE FIGÚRKY
                        Canvas.SetTop(pohladObrazu, (7 - i) * 50);

                        this.koren.Children.Add(pohladObrazu);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Chyba pri načítaní figúrky: {nazovSuboru}\nDetaily: {ex.Message}");
                    }
                }
            }
        }

        private int[] getPoziciuMysi(MouseButtonEventArgs eventArgs)
        {
            Point poziciaMysi = eventArgs.GetPosition(this.koren);
            int x = (int)(poziciaMysi.X / 50); 
            // OTOČENIE KLIKNUTIA MYŠOU: Ak klikneme úplne dole, pre logiku to bude riadok 0
            int y = 7 - (int)(poziciaMysi.Y / 50); 
            return new int[] { x, y }; 
        }

        private void restartujPlochu()
        {
            this.koren.Children.Clear(); 
            this.vytvorSachovnicu(); 
            this.pridajObrazok(); 
        }
    }
}