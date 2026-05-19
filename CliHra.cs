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

            // Nastavenie callbacku, ktorý sa zavolá pri konci hry
            this.plocha.setCallbackVysledku(sprava => {
                Console.WriteLine("\n=================================");
                Console.WriteLine($" KONIEC HRY: {sprava}");
                Console.WriteLine("=================================\n");
                this.hraBezi = false;
            });
        }

        public void Spusti()
        {
            // TOTO JE DÔLEŽITÉ: Zapne v termináli podporu pre UTF-8, inak uvidíš otázniky
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("Vitajte v CLI verzii Šachu!");
            Console.WriteLine("Zadávajte ťahy v štandardnom šachovom formáte (napríklad 'e2 e4').");
            Console.WriteLine("Kedykoľvek počas hry môžete napísať 'gui' pre prechod do grafického režimu.");
            Console.WriteLine("Pre ukončenie napíšte 'exit'.\n");

            while (this.hraBezi)
            {
                VykresliPlochu();
                Console.Write("Zadaj svoj ťah (napr. a2 a4 alebo 'gui'/'exit'): ");
                string vstup = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(vstup)) continue;
                
                string vycistenyVstup = vstup.Trim().ToLower();
                if (vycistenyVstup == "exit") break;

                // CHYTENIE PRÍKAZU PRE PRECHOD DO GUI POČAS HRY
                if (vycistenyVstup == "gui")
                {
                    Console.WriteLine("\n[i] Otváram grafické rozhranie (WPF)...");

                    // Inicializácia WPF Application prostredia na pozadí, ak ešte neexistuje
                    if (System.Windows.Application.Current == null)
                    {
                        new System.Windows.Application { ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown };
                    }

                    // Vytvorenie grafického okna s našou AKTUÁLNOU rozohranou plochou
                    var okno = new MainWindow(this.plocha);

                    // Prepíšeme callback tak, aby po výhre/prehre v GUI správne zareagovalo CLI aj WPF súčasne
                    this.plocha.setCallbackVysledku(sprava => {
                        this.hraBezi = false;
                        Console.WriteLine($"\n=================================");
                        Console.WriteLine($" KONIEC HRY: {sprava}");
                        Console.WriteLine("=================================\n");
                        
                        System.Windows.Application.Current.Dispatcher.Invoke(() => {
                            System.Windows.MessageBox.Show(sprava, "Koniec hry", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                        });
                    });

                    // Zobrazenie okna (zablokuje toto CLI vlákno, kým okno nezavrieš)
                    okno.ShowDialog();

                    // Po zatvorení okna skontrolujeme, či hra neskončila v GUI
                    if (!this.hraBezi) break;

                    // Ak hra pokračuje, vrátime späť čistý CLI callback pre konzolu
                    this.plocha.setCallbackVysledku(sprava => {
                        Console.WriteLine("\n=================================");
                        Console.WriteLine($" KONIEC HRY: {sprava}");
                        Console.WriteLine("=================================\n");
                        this.hraBezi = false;
                    });

                    Console.WriteLine("\n[i] Návrat do CLI režimu. Šachovnica aktualizovaná.");
                    continue;
                }

                // Parser ťahu
                string[] casti = vstup.Split(new[] { ' ', '-', ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (casti.Length == 2 && 
                    SkusRozparsovatPoziciu(casti[0], out int r1, out int s1) && 
                    SkusRozparsovatPoziciu(casti[1], out int r2, out int s2))
                {
                    // Vykonanie ťahu pomocou logiky
                    this.plocha.setFigurka(r1, s1, r2, s2);
                }
                else
                {
                    Console.WriteLine("\n[!] Neplatný formát. Použite 'e2 e4' alebo príkaz 'gui'.");
                }
            }
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
                riadok = rdk - '1'; // '1' na index 0, '2' na index 1, atď.
                return true;
            }
            return false;
        }

        private void VykresliPlochu()
        {
            Console.WriteLine("\n    a b c d e f g h");
            Console.WriteLine("  -------------------");
            
            // Slučka ide od 7 (Rank 8, Čierny) dole po 0 (Rank 1, Biely)
            for (int i = 7; i >= 0; i--)
            {
                // Číslo riadku na začiatku
                Console.Write((i + 1) + " | "); 
                
                for (int j = 0; j < 8; j++)
                {
                    Figurka f = this.plocha.getFigurka(i, j);
                    if (f == null)
                    {
                        // Vykreslenie šachovnicového vzoru pre prázdne políčka
                        // (i + j) % 2 == 0 nám zaručí, že políčko a1 (0,0) bude tmavé
                        if ((i + j) % 2 == 0)
                        {
                            Console.Write("□ "); // Tmavé políčko
                        }
                        else
                        {
                            Console.Write("■ "); // Svetlé políčko
                        }
                    }
                    else
                    {
                        char znak = ZiskajZnakFigurky(f);
                        Console.Write(znak + " ");
                    }
                }
                // Číslo riadku na konci
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
            else // CIERNA
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