using System;

namespace com.example.Logika
{
    public class Plocha
    {
        private Figurka[,] plocha; 
        private Strana aktualnaStrana; 
        private bool hraSkoncila; 
        private Action<string> callbackVysledku; 

        public Plocha()
        {
            this.aktualnaStrana = Strana.BIELA;
            this.hraSkoncila = false;
            this.callbackVysledku = s => { };
            this.plocha = new Figurka[8, 8];

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    this.plocha[i, j] = null!;
                }
            }

            this.plocha[0, 0] = new Veza(0, 0, Strana.BIELA);
            this.plocha[0, 1] = new Jazdec(0, 1, Strana.BIELA);
            this.plocha[0, 2] = new Strelec(0, 2, Strana.BIELA);
            this.plocha[0, 3] = new Kral(0, 3, Strana.BIELA);
            this.plocha[0, 4] = new Dama(0, 4, Strana.BIELA);
            this.plocha[0, 5] = new Strelec(0, 5, Strana.BIELA);
            this.plocha[0, 6] = new Jazdec(0, 6, Strana.BIELA);
            this.plocha[0, 7] = new Veza(0, 7, Strana.BIELA);

            for (int i = 0; i < 8; i++)
            {
                this.plocha[1, i] = new Pesiak(1, i, Strana.BIELA);
            }

            this.plocha[7, 0] = new Veza(7, 0, Strana.CIERNA);
            this.plocha[7, 1] = new Jazdec(7, 1, Strana.CIERNA);
            this.plocha[7, 2] = new Strelec(7, 2, Strana.CIERNA);
            this.plocha[7, 3] = new Kral(7, 3, Strana.CIERNA);
            this.plocha[7, 4] = new Dama(7, 4, Strana.CIERNA);
            this.plocha[7, 5] = new Strelec(7, 5, Strana.CIERNA);
            this.plocha[7, 6] = new Jazdec(7, 6, Strana.CIERNA);
            this.plocha[7, 7] = new Veza(7, 7, Strana.CIERNA);

            for (int i = 0; i < 8; i++)
            {
                this.plocha[6, i] = new Pesiak(6, i, Strana.CIERNA);
            }
        }

        public Figurka getFigurka(int x, int y)
        {
            return this.plocha[x, y];
        }

        public void setFigurka(int x, int y, int novyX, int novyY)
        {
            if (this.hraSkoncila)
            {
                return;
            }

            if (x < 0 || x > 7 || y < 0 || y > 7 || novyX < 0 || novyX > 7 || novyY < 0 || novyY > 7)
            {
                return;
            }

            if (this.plocha[x, y] == null)
            {
                return; 
            }

            if (!this.poradie(x, y))
            {
                return;
            }

            if (x == 0 && y == 3 && novyX == 0 && (novyY == 0 || novyY == 7))
            {
                if (this.getFigurka(x, y).getTyp() == TypFigurky.KRAL && !(this.getFigurka(x, y).getZmena()))
                {
                    if (this.rosada(Strana.BIELA, novyX, novyY))
                    {
                        this.zmenPoradie();
                        return;
                    }
                }
            }
            else if (x == 7 && y == 3 && novyX == 7 && (novyY == 0 || novyY == 7))
            {
                if (this.getFigurka(x, y).getTyp() == TypFigurky.KRAL && !(this.getFigurka(x, y).getZmena()))
                {
                    if (this.rosada(Strana.CIERNA, novyX, novyY))
                    {
                        this.zmenPoradie();
                        return;
                    }
                }
            }

            if (this.plocha[novyX, novyY] != null && this.plocha[x, y].getStrana() == this.plocha[novyX, novyY].getStrana())
            {
                return; 
            }
            else if (this.getFigurka(x, y).preskakujeFigurky(novyX, novyY, this))
            {
                return; 
            }
            else if (!this.plocha[x, y].validnyPohyb(novyX, novyY, this))
            {
                return; 
            }

            if (this.getFigurka(x, y).getTyp() == TypFigurky.PESIAK)
            {
                if (this.plocha[x, y].getStrana() == Strana.BIELA && novyX == 7)
                {
                    this.plocha[novyX, novyY] = new Dama(novyX, novyY, Strana.BIELA);
                }
                else if (this.plocha[x, y].getStrana() == Strana.CIERNA && novyX == 0)
                {
                    this.plocha[novyX, novyY] = new Dama(novyX, novyY, Strana.CIERNA);
                }
                else
                {
                    this.plocha[novyX, novyY] = this.plocha[x, y];
                    this.plocha[novyX, novyY].setPozicia(novyX, novyY);
                }
            }
            else
            {
                this.plocha[novyX, novyY] = this.plocha[x, y];
                this.plocha[novyX, novyY].setPozicia(novyX, novyY);
            }

            this.plocha[x, y] = null; 

            this.zmenPoradie();
            this.kontrolaVyhry();
        }

        public bool poradie(int x, int y)
        {
            if (this.plocha[x, y] == null)
            {
                Console.WriteLine("Chyba: Na pozícii (" + x + ", " + y + ") sa nenachadza ziadna figurka.");
                return false;
            }

            if (this.plocha[x, y].getStrana() != this.aktualnaStrana)
            {
                Console.WriteLine("Chyba: Nie je tah strany " + this.plocha[x, y].getStrana() + ".");
                return false;
            }

            return true;
        }

        public void kontrolaVyhry()
        {
            bool bielyKralZije = false;
            bool ciernyKralZije = false;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Figurka figurka = this.plocha[i, j];
                    if (figurka != null && figurka.getTyp() == TypFigurky.KRAL)
                    {
                        if (figurka.getStrana() == Strana.BIELA)
                        {
                            bielyKralZije = true;
                        }
                        else if (figurka.getStrana() == Strana.CIERNA)
                        {
                            ciernyKralZije = true;
                        }
                    }
                }
            }

            if (!bielyKralZije)
            {
                this.zobrazVysledok("CIERNA VYHRALA");
                this.hraSkoncila = true;
            }
            else if (!ciernyKralZije)
            {
                this.zobrazVysledok("BIELA VYHRALA");
                this.hraSkoncila = true;
            }
        }

        public void setCallbackVysledku(Action<string> callback)
        {
            this.callbackVysledku = callback;
        }

        private void zobrazVysledok(string sprava)
        {
            Console.WriteLine(sprava);
            if (this.callbackVysledku != null)
            {
                this.callbackVysledku.Invoke(sprava);
            }
        }

        private void zmenPoradie()
        {
            if (this.aktualnaStrana == Strana.BIELA)
            {
                this.aktualnaStrana = Strana.CIERNA;
            }
            else
            {
                this.aktualnaStrana = Strana.BIELA;
            }
        }

        private bool rosada(Strana strana, int novyX, int novyY)
        {
            if (strana == Strana.BIELA)
            {
                if (this.getFigurka(novyX, novyY) != null && this.getFigurka(novyX, novyY).getTyp() == TypFigurky.VEZA && !(this.getFigurka(novyX, novyY).getZmena()))
                {
                    if (novyY == 0)
                    {
                        if (this.plocha[0, 1] == null && this.plocha[0, 2] == null)
                        {
                            this.plocha[0, 1] = this.plocha[0, 3];
                            this.plocha[0, 1].setPozicia(0, 1);
                            this.plocha[0, 3] = null!;
                            this.plocha[0, 2] = this.plocha[0, 0];
                            this.plocha[0, 2].setPozicia(0, 2);
                            this.plocha[0, 0] = null!;
                            return true;
                        }
                    }
                    else if (novyY == 7)
                    {
                        if (this.plocha[0, 4] == null && this.plocha[0, 5] == null && this.plocha[0, 6] == null)
                        {
                            this.plocha[0, 5] = this.plocha[0, 3];
                            this.plocha[0, 5].setPozicia(0, 5);
                            this.plocha[0, 3] = null!;
                            this.plocha[0, 4] = this.plocha[0, 7];
                            this.plocha[0, 4].setPozicia(0, 4);
                            this.plocha[0, 7] = null!;
                            return true;
                        }
                    }
                }
                return false;
            }
            else if (strana == Strana.CIERNA)
            {
                if (this.getFigurka(novyX, novyY) != null && this.getFigurka(novyX, novyY).getTyp() == TypFigurky.VEZA && !(this.getFigurka(novyX, novyY).getZmena()))
                {
                    if (novyY == 0)
                    {
                        if (this.plocha[7, 1] == null && this.plocha[7, 2] == null)
                        {
                            this.plocha[7, 1] = this.plocha[7, 3];
                            this.plocha[7, 1].setPozicia(7, 1);
                            this.plocha[7, 3] = null!;
                            this.plocha[7, 2] = this.plocha[7, 0];
                            this.plocha[7, 2].setPozicia(7, 2);
                            this.plocha[7, 0] = null!;
                            return true;
                        }
                    }
                    else if (novyY == 7)
                    {
                        if (this.plocha[7, 4] == null && this.plocha[7, 5] == null && this.plocha[7, 6] == null)
                        {
                            this.plocha[7, 5] = this.plocha[7, 3];
                            this.plocha[7, 5].setPozicia(7, 5);
                            this.plocha[7, 3] = null!;
                            this.plocha[7, 4] = this.plocha[7, 7];
                            this.plocha[7, 4].setPozicia(7, 4);
                            this.plocha[7, 7] = null!;
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}