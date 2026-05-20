using System;
using System.Text;
using com.example.Logika;

namespace com.example
{
    public class CliHra
    {
        private Plocha plocha;
        private bool hraBezi;

        public CliHra()
        {
            this.plocha = new Plocha();
            this.hraBezi = true;
            this.plocha.setCallbackVysledku(sprava => {
                Console.WriteLine("\n=================================");
                Console.WriteLine($" KONIEC HRY: {sprava}");
                Console.WriteLine("=================================\n");
                this.hraBezi = false;
            });
        }

        public void Spusti()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("Vitajte v CLI verzii Šachu!");
            Console.WriteLine("Pre nápovedu kedykoľvek napíšte 'help'.\n");

            while (this.hraBezi)
            {
                VykresliPlochu();
                Console.Write("Zadaj svoj ťah (napr. a2 a4) alebo argument (--help): ");
                string? vstup = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(vstup)) continue;
                
                string vycistenyVstup = vstup.Trim().ToLower();
                string[] prikazy = vycistenyVstup.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string hlavnyPrikaz = prikazy[0];

                if (hlavnyPrikaz == "exit") break;

                if (hlavnyPrikaz == "help" || hlavnyPrikaz == "--help")
                {
                    VypisHelp();
                    continue;
                }

                if (hlavnyPrikaz == "bgcolor" || hlavnyPrikaz == "--bgcolor")
                {
                    if (prikazy.Length > 1)
                    {
                        NastavFarbuPozadia(prikazy[1]);
                    }
                    else
                    {
                        Console.WriteLine("\n[!] Použitie: bgcolor <farba> (napr. bgcolor DarkBlue)");
                    }
                    continue;
                }

                if (hlavnyPrikaz == "gamerules" || hlavnyPrikaz == "--gamerules")
                {
                    VypisPravidla();
                    continue;
                }

                if (hlavnyPrikaz == "gui" || hlavnyPrikaz == "--gui")
                {
                    Console.WriteLine("\n[i] Otváram grafické rozhranie (WPF)...");

                    if (System.Windows.Application.Current == null)
                    {
                        new System.Windows.Application { ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown };
                    }

                    var okno = new MainWindow(this.plocha);

                    this.plocha.setCallbackVysledku(sprava => {
                        this.hraBezi = false;
                        Console.WriteLine($"\n=================================");
                        Console.WriteLine($" KONIEC HRY: {sprava}");
                        Console.WriteLine("=================================\n");
                        
                        var app = System.Windows.Application.Current;
                        if (app != null)
                        {
                            app.Dispatcher.Invoke(() => {
                                System.Windows.MessageBox.Show(sprava, "Koniec hry", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                            });
                        }
                    });

                    okno.ShowDialog();

                    if (!this.hraBezi) break;

                    this.plocha.setCallbackVysledku(sprava => {
                        Console.WriteLine("\n=================================");
                        Console.WriteLine($" KONIEC HRY: {sprava}");
                        Console.WriteLine("=================================\n");
                        this.hraBezi = false;
                    });

                    Console.WriteLine("\n[i] Návrat do CLI režimu. Šachovnica aktualizovaná.");
                    continue;
                }

                string[] casti = vstup.Split(new[] { ' ', '-', ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (casti.Length == 2 && 
                    SkusRozparsovatPoziciu(casti[0], out int r1, out int s1) && 
                    SkusRozparsovatPoziciu(casti[1], out int r2, out int s2))
                {
                    this.plocha.setFigurka(r1, s1, r2, s2);
                }
                else
                {
                    Console.WriteLine("\n[!] Neplatný formát. Použite 'e2 e4' alebo 'help'.");
                }
            }
        }

        private void NastavFarbuPozadia(string nazovFarby)
        {
            if (Enum.TryParse(nazovFarby, true, out ConsoleColor farba))
            {
                Console.BackgroundColor = farba;
                Console.Clear(); 
                Console.WriteLine($"\n[i] Farba pozadia úspešne zmenená na: {farba}");
            }
            else
            {
                Console.WriteLine($"\n[!] Neznáma farba: '{nazovFarby}'. Skúste napr. Blue, DarkRed, Black.");
            }
        }

        private void VypisPravidla()
        {
            Console.WriteLine("\n=== AKO FUNGUJE HRA ===");
            Console.WriteLine("Cieľom hry je dať mat súperovmu kráľovi.");
            Console.WriteLine("Hráči sa striedajú v ťahoch, biely začína ako prvý.");
            Console.WriteLine("\nZadávanie ťahov v CLI:");
            Console.WriteLine("Ťahy sa zadávajú pomocou súradníc šachovnice (napríklad 'e2 e4').");
            Console.WriteLine("Prvá časť ('e2') je pozícia figúrky, ktorou chcete pohnúť.");
            Console.WriteLine("Druhá časť ('e4') je cieľové políčko, kam chcete figúrku presunúť.");
            Console.WriteLine("\nŠpeciálne funkcie:");
            Console.WriteLine("Hru môžete kedykoľvek presunúť do grafického okna príkazom 'gui'.");
            Console.WriteLine("=======================\n");
            Console.WriteLine("Stlačte Enter pre pokračovanie...");
            Console.ReadLine();
        }

        private void VypisHelp()
        {
            Console.WriteLine("\n=== ŠACH - NÁPOVEDA ===");
            Console.WriteLine("Štandardné ťahy:");
            Console.WriteLine("  Zadávajte v tvare: e2 e4\n");
            
            Console.WriteLine("Príkazy počas hry:");
            Console.WriteLine("  help              Zobrazí túto nápovedu.");
            Console.WriteLine("  bgcolor <farba>   Zmení farbu pozadia (napr. 'bgcolor Blue').");
            Console.WriteLine("     FARBY NA VYBER: Black, White, Gray, Blue, Green, Cyan, Red, Yellow, Magenta, DarkBlue, DarkGreen, DarkCyan, DarkRed, DarkYellow, DarkMagenta.");
            Console.WriteLine("  gamerules         Vysvetlí pravidlá a princíp fungovania hry.");
            Console.WriteLine("  gui               Prepne aktuálne rozohranú hru do grafického režimu (WPF).");
            Console.WriteLine("  exit              Ukončí prebiehajúcu hru a vypne program.");
            Console.WriteLine("=======================\n");
            Console.WriteLine("Stlačte Enter pre pokračovanie... ");
            Console.ReadLine();
        }

        private bool SkusRozparsovatPoziciu(string cast, out int riadok, out int stlpec)
        {
            riadok = -1;
            stlpec = -1;
            if (cast.Length != 2) return false;
            char slp = char.ToLower(cast[0]); 
            char rdk = cast[1];
            if (slp >= 'a' && slp <= 'h' && rdk >= '1' && rdk <= '8')
            {
                stlpec = slp - 'a'; 
                riadok = rdk - '1'; 
                return true;
            }
            return false;
        }

        private void VykresliPlochu()
        {
            Console.WriteLine("\n    a b c d e f g h");
            Console.WriteLine("  -------------------");
            
            for (int i = 7; i >= 0; i--)
            {
                Console.Write((i + 1) + " | "); 
                
                for (int j = 0; j < 8; j++)
                {
                    Figurka f = this.plocha.getFigurka(i, j);
                    if (f == null)
                    {
                        if ((i + j) % 2 == 0)
                        {
                            Console.Write("□ "); 
                        }
                        else
                        {
                            Console.Write("■ "); 
                        }
                    }
                    else
                    {
                        char znak = ZiskajZnakFigurky(f);
                        Console.Write(znak + " ");
                    }
                }
                Console.WriteLine("| " + (i + 1));
            }
            Console.WriteLine("  -------------------");
            Console.WriteLine("    a b c d e f g h\n");
        }

        private char ZiskajZnakFigurky(Figurka f)
        {
            if (f.getStrana() == Strana.BIELA)
            {
                switch (f.getTyp())
                {
                    case TypFigurky.KRAL: return '♚';
                    case TypFigurky.DAMA: return '♛';
                    case TypFigurky.VEZA: return '♜';
                    case TypFigurky.STRELEC: return '♝';
                    case TypFigurky.JAZDEC: return '♞';
                    case TypFigurky.PESIAK: return '♟';
                }
            }
            else 
            {
                switch (f.getTyp())
                {
                    case TypFigurky.KRAL: return '♔';
                    case TypFigurky.DAMA: return '♕';
                    case TypFigurky.VEZA: return '♖';
                    case TypFigurky.STRELEC: return '♗';
                    case TypFigurky.JAZDEC: return '♘';
                    case TypFigurky.PESIAK: return '♙';
                }
            }
            return '?';
        }
    }
}