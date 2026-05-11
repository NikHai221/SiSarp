package com.example.Logika;

public class Pesiak extends Figurka {
    private final int prvaPoziciaX;
    private final int prvaPoziciaY;
    
    /**
     * Konštruktor triedy Pesiak.
     *
     * @param strana tým pre ktorý patrí
     * @param x   Počiatočná pozícia na osi x
     * @param y   Počiatočná pozícia na osi y
     */
    public Pesiak(int x, int y, Strana strana) {
        super(TypFigurky.PESIAK, x, y, strana);
        this.prvaPoziciaX = x;
        this.prvaPoziciaY = y;
    }

    /**
     * Overuje, či je pohyb na danú súradnicu (x, y) platný pre pesiaka.
     * Ak je pohyb platný, nastaví novú pozíciu figúrky.
     * Pesiak ma vlastnu metodu na overenie platnosti pohybu, pretoze na svoj pohyb potrebuje poznat polohu ostatnych figurok.
     * 
     * @param x cieľová súradnica x
     * @param y cieľová súradnica y
     * @param plocha hracia plocha
     * @return true, ak je pohyb platný
     */
    @Override
    public boolean validnyPohyb(int x, int y, Plocha plocha) {
        
        if (x < 0 || x > 7 || y < 0 || y > 7) {
            return false;
        }
        
        if (this.getStrana() == Strana.BIELA) {
            if (y == this.getY() && x == this.getX() + 1 && plocha.getFigurka(x, y) == null) {
                this.setPozicia(x, y);
                return true;
            } else if (y == this.getY() && x == this.getX() + 2 
                && (this.prvaPoziciaX == this.getX() && this.prvaPoziciaY == this.getY()) && plocha.getFigurka(x, y) == null && plocha.getFigurka(x - 1, y) == null) {
                this.setPozicia(x, y);
                return true;
            } else if (y == this.getY() + 1 && x == this.getX() + 1 && plocha.getFigurka(x, y) != null) {
                this.setPozicia(x, y);
                return true;
            } else if (y == this.getY() - 1 && x == this.getX() + 1 && plocha.getFigurka(x, y) != null) {
                this.setPozicia(x, y);
                return true;
            } else {
                return false;
            }


               
        } else {
            if (y == this.getY() && x == this.getX() - 1 && plocha.getFigurka(x, y) == null) {
                this.setPozicia(x, y);
                return true;
            } else if (y == this.getY() && x == this.getX() - 2 
                && (this.prvaPoziciaX == this.getX() && this.prvaPoziciaY == this.getY()) && plocha.getFigurka(x, y) == null && plocha.getFigurka(x + 1, y) == null) {
                this.setPozicia(x, y);
                return true;
            } else if (y == this.getY() + 1 && x == this.getX() - 1 && plocha.getFigurka(x, y) != null) {
                this.setPozicia(x, y);
                return true;
            } else if (y == this.getY() - 1 && x == this.getX() - 1 && plocha.getFigurka(x, y) != null) {
                this.setPozicia(x, y);
                return true;
            } else {
                return false;
            }
        }      
    }
}
