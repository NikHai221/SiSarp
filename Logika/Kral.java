package com.example.Logika;

public class Kral extends Figurka {

    /**
     * Konštruktor triedy Kral.
     * 
     * @param x Počiatočná pozícia na osi x
     * @param y Počiatočná pozícia na osi y
     */
    public Kral(int x, int y, Strana strana) {
        super(TypFigurky.KRAL, x, y, strana);
    }

    /**
     * Overuje, či je pohyb na danú súradnicu (x, y) platný pre kráľa.
     * Ak je pohyb platný, nastaví novú pozíciu figúrky.
     * 
     * @param x cieľová súradnica x
     * @param y cieľová súradnica y
     * @return true, ak je pohyb platný
     */
    @Override
    public boolean validnyPohyb(int x, int y, Plocha plocha) {
        
        if (x < 0 || x > 7 || y < 0 || y > 7) {
            return false;
        }
        if (Math.abs(x - this.getX()) <= 1 && Math.abs(y - this.getY()) <= 1) {
            this.setPozicia(x, y);
            return true;
        }
        return false;
    }
        
}
