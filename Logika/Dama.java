package com.example.Logika;

public class Dama extends Figurka {

    /**
     * Konštruktor triedy Dama.
     * 
     * @param x Počiatočná pozícia na osi x
     * @param y Počiatočná pozícia na osi y
     */
    public Dama(int x, int y, Strana strana) {
        super(TypFigurky.DAMA, x, y, strana);
    }

    /**
     * Overuje, či je pohyb na danú súradnicu (x, y) platný pre dámu.
     * Ak je pohyb platný, nastaví novú pozíciu figúrky.
     * 
     * @param x cieľová súradnica x
     * @param y cieľová súradnica y
     * @return true, ak je pohyb platný
     */
    @Override
    public boolean validnyPohyb(int x, int y, Plocha p) {
        // Overenie, či sú súradnice v rámci hraníc šachovnice
        if (x < 0 || x > 7 || y < 0 || y > 7) {
            return false;
        }
        // Overenie, či je pohyb v povolených smeroch (horizontálny, vertikálny, diagonálny)
        if (x == this.getX() || y == this.getY() || x - this.getX() == y - this.getY() || x - this.getX() == this.getY() - y) {
            this.setPozicia(x, y);
            return true;
        } else {
            return false;
        }
    }

}
