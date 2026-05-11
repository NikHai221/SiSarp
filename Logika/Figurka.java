package com.example.Logika;


public abstract class Figurka {
    
    private TypFigurky typ; //Typ figúrky 
    private Strana strana; //Strana, ku ktorej figúrka patrí (napríklad biela alebo čiarna).
    private int x; //Aktuálna pozícia figúrky na osi x (stĺpec).
    private int y; //Aktuálna pozícia figúrky na osi y (riadok).
    private boolean zmena = false; //Indikátor, či bola figúrka už presunutá.

    /**
     * Konštruktor na vytvorenie figúrky so špecifikovaným typom a počiatočnou pozíciou.
     *
     * @param typ Typ figúrky
     * @param x   Počiatočná pozícia na osi x
     * @param y   Počiatočná pozícia na osi y
     */
    public Figurka(TypFigurky typ, int x, int y, Strana strana) {
        this.strana = strana;
        this.typ = typ;
        this.x = x;
        this.y = y;
    }

    /**
     * Získa typ figúrky.
     *
     * @return Typ figúrky
     */
    public TypFigurky getTyp() {
        return this.typ;
    }

    /**
     * Nastaví novú pozíciu figúrky na hracej doske.
     *
     * @param x Nová pozícia na osi x
     * @param y Nová pozícia na osi y
     */
    public void setPozicia(int x, int y) {
        this.x = x;
        this.y = y;
        this.zmena = true; 
    }

    /**
     * Získa aktuálnu pozíciu figúrky na osi x.
     *
     * @return Pozícia na osi x
     */
    public int getX() {
        return this.x;
    }

    /**
     * Získa aktuálnu pozíciu figúrky na osi y.
     *
     * @return Pozícia na osi y
     */
    public int getY() {
        return this.y;
    }

    /**
     * Overí, či je špecifikovaný pohyb figúrky platný.
     * Predvolená implementácia vracia false, pretože validácia ďalej
     * závisí na konkrétnom type figúrky (napríklad kráľ, veža, atď.).
     *
     * @param x Cieľová pozícia na osi x
     * @param y Cieľová pozícia na osi y
     * @return true, ak je pohyb platný; inak false
     */
    public abstract boolean validnyPohyb(int x, int y, Plocha p);

   
    /**
     * Získa stranu, ku ktorej figúrka patrí.
     *
     * @return Strana figúrky
     */
    public Strana getStrana() {
        return this.strana;
    }


    /**
     * Overí, či figúrka preskakuje iné figúrky pri pohybe.
     * Predpokladá sa, že figúrka sa pohybuje v diagonálnom smere.
     *
     * @param novyX Nová pozícia na osi x
     * @param novyY Nová pozícia na osi y
     * @param p     Hracia doska
     * @return true, ak figúrka preskakuje iné figúrky; inak false
     */
    public boolean preskakujeFigurky(int novyX, int novyY, Plocha p) {
        int deltaX = Integer.signum(novyX - this.getX());
        int deltaY = Integer.signum(novyY - this.getY());

        int i = this.getX() + deltaX;
        int j = this.getY() + deltaY;

        while (i != novyX || j != novyY) {
            if (p.getFigurka(i, j) != null) {
                return true;
            }
            i += deltaX;
            j += deltaY;
        }

        return false;
    }

    public boolean getZmena() {
        return this.zmena;
    }
}