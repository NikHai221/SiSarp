package com.example.Logika;

public class Strelec extends Figurka {

    /**
     * Konštruktor triedy Strelec.
     * 
     * @param x Počiatočná pozícia na osi x
     * @param y Počiatočná pozícia na osi y
     */
    public Strelec(int x, int y, Strana strana) {
        super(TypFigurky.STRELEC, x, y, strana);
    }

    /**
     * Overuje, či je pohyb na danú súradnicu (x, y) platný pre strelec.
     * Ak je pohyb platný, nastaví novú pozíciu figúrky.
     * 
     * @param x cieľová súradnica x
     * @param y cieľová súradnica y
     * @return true, ak je pohyb platný
     */
    public boolean validnyPohyb(int x, int y, Plocha p) {
        if (x < 0 || x > 7 || y < 0 || y > 7) {
            return false;
        }
        if (x - this.getX() == y - this.getY() || x - this.getX() == this.getY() - y) {
            this.setPozicia(x, y);
            return true;
        } else {
            return false;
        }
    }

}
