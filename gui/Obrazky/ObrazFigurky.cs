using System;
using com.example.Logika;

namespace com.example.Obrazky
{
    public class ObrazFigurky
    {
        private Figurka figurka; // Figurka, ktorej obrazok sa ma zobrazit
        private int cislo; // Cislo figurky
        private string obrazok; // Nazov obrazku figurky

        /// <summary>
        /// Konštruktor vytvára inštanciu triedy ObrazFigurky.
        /// </summary>
        /// <param name="figurka">objekt triedy Figurka, ktorá má byť zobrazená</param>
        public ObrazFigurky(Figurka figurka)
        {
            this.figurka = figurka;

            // Porovnanie v C# sa robí pomocou == namiesto .equals()
            if (this.figurka.getStrana() == Strana.CIERNA)
            {
                this.cislo = 6;
            }
            
            // V C# môžeme získať číselnú hodnotu enumu priamym pretypovaním na (int)
            this.cislo = this.cislo + (int)this.figurka.getTyp();
            vyberObrazok();
        }

        /// <summary>
        /// Metóda vyberá obrazok na základe čísla figurky.
        /// </summary>
        private void vyberObrazok()
        {
            switch (this.cislo)
            {
                case 1:
                    this.obrazok = "bielyPesiak.png";
                    break;
                case 2:
                    this.obrazok = "bielyJazdec.png";
                    break;
                case 3:
                    this.obrazok = "bielaVeza.png";
                    break;
                case 4:
                    this.obrazok = "bielyStrelec.png";
                    break;
                case 5:
                    this.obrazok = "bielyKral.png";
                    break;
                case 6:
                    this.obrazok = "bielaDama.png";
                    break;
                case 7:
                    this.obrazok = "ciernyPesiak.png";
                    break;
                case 8:
                    this.obrazok = "ciernyJazdec.png";
                    break;
                case 9:
                    this.obrazok = "ciernaVeza.png";
                    break;
                case 10:
                    this.obrazok = "ciernyStrelec.png";
                    break;
                case 11:
                    this.obrazok = "ciernyKral.png";
                    break;
                case 12:
                    this.obrazok = "ciernaDama.png";
                    break;
            }
        }

        /// <summary>
        /// Metóda vracia názov obrazku figurky.
        /// </summary>
        /// <returns>názov obrazku figurky</returns>
        public string getObrazok()
        {
            return this.obrazok;
        }
    }
}