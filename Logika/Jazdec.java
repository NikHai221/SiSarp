package com.example.Logika;

public class Jazdec extends Figurka {

    /**
     * Konštruktor triedy Jazdec.
     * 
     * @param x Počiatočná pozícia na osi x
     * @param y Počiatočná pozícia na osi y
     */
    public Jazdec(int x, int y, Strana strana) {
        super(TypFigurky.JAZDEC, x, y, strana);
    }

    /**
     * Overuje, či je pohyb na danú súradnicu (x, y) platný pre jazdca.
     * Ak je pohyb platný, nastaví novú pozíciu figúrky.
     * 
     * @param x cieľová súradnica x
     * @param y cieľová súradnica y
     * @return true, ak je pohyb platný
     */
    @Override
    public boolean validnyPohyb(int x, int y, Plocha p) {
        if (x < 0 || x > 7 || y < 0 || y > 7) {
            return false;
        }
        if ((x == this.getX() + 2 && y == this.getY() + 1) || (x == this.getX() + 2 && y == this.getY() - 1)
            || (x == this.getX() - 2 && y == this.getY() + 1) || (x == this.getX() - 2 && y == this.getY() - 1)
            || (x == this.getX() + 1 && y == this.getY() + 2) || (x == this.getX() + 1 && y == this.getY() - 2) 
            || (x == this.getX() - 1 && y == this.getY() + 2) || (x == this.getX() - 1 && y == this.getY() - 2)) {
            this.setPozicia(x, y);
            return true;
        } else {
            return false;
        }
    }

    @Override
    public boolean preskakujeFigurky(int novyX, int novyY, Plocha p) {
        return false;
    }

    

}
