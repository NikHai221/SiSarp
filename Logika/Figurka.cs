using System;

namespace com.example.Logika
{
    public abstract class Figurka
    {
        private TypFigurky typ; //Typ figúrky 
        private Strana strana; //Strana, ku ktorej figúrka patrí (napríklad biela alebo čierna).
        private int x; //Aktuálna pozícia figúrky na osi x (stĺpec).
        private int y; //Aktuálna pozícia figúrky na osi y (riadok).
        private bool zmena = false; //Indikátor, či bola figúrka už presunutá.

        /// <summary>
        /// Konštruktor na vytvorenie figúrky so špecifikovaným typom a počiatočnou pozíciou.
        /// </summary>
        public Figurka(TypFigurky typ, int x, int y, Strana strana)
        {
            this.strana = strana;
            this.typ = typ;
            this.x = x;
            this.y = y;
        }

        public TypFigurky getTyp()
        {
            return this.typ;
        }

        public void setPozicia(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.zmena = true;
        }

        public int getX()
        {
            return this.x;
        }

        public int getY()
        {
            return this.y;
        }

        public abstract bool validnyPohyb(int x, int y, Plocha p);

        public Strana getStrana()
        {
            return this.strana;
        }

        /// <summary>
        /// Overí, či figúrka preskakuje iné figúrky pri pohybe.
        /// </summary>
        public virtual bool preskakujeFigurky(int novyX, int novyY, Plocha p)
        {
            // Kontrola, či ide vôbec o priamy smer (horizontálne, vertikálne alebo presne diagonálne)
            int absX = Math.Abs(novyX - this.getX());
            int absY = Math.Abs(novyY - this.getY());
            
            // Ak to nie je ani rovný, ani presne diagonálny ťah (napríklad ťah v tvare L), 
            // nemá zmysel kontrolovať preskakovanie. 
            if (absX != 0 && absY != 0 && absX != absY)
            {
                return false; 
            }

            int deltaX = Math.Sign(novyX - this.getX());
            int deltaY = Math.Sign(novyY - this.getY());

            int i = this.getX() + deltaX;
            int j = this.getY() + deltaY;

            // Cyklus pôjde po políčkach, kým nedosiahne cieľovú destináciu
            while (i != novyX || j != novyY)
            {
                // Poistka proti vybehnutiu mimo hracej plochy (keby sa niečo pokazilo)
                if (i < 0 || i > 7 || j < 0 || j > 7) 
                {
                    break;
                }

                if (p.getFigurka(i, j) != null)
                {
                    return true; // Našli sme figúrku, ktorá stojí v ceste
                }
                
                i += deltaX;
                j += deltaY;
            }

            return false;
        }

        public bool getZmena()
        {
            return this.zmena;
        }
    }
}