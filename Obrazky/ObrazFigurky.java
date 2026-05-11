package com.example.Obrazky;

import com.example.Logika.Figurka;
import com.example.Logika.Strana;

public class ObrazFigurky {

    private Figurka figurka; // Figurka, ktorej obrazok sa ma zobrazit
    private int cislo; // Cislo figurky
    private String obrazok; // Nazov obrazku figurky

/**
     * Konštruktor vytvára inštanciu triedy ObrazFigurky.
     *
     * @param figurka objekt triedy Figurka, ktorá má byť zobrazená
     */
    public ObrazFigurky(Figurka figurka) {
        this.figurka = figurka;

        if(this.figurka.getStrana().equals(Strana.CIERNA)){
            this.cislo = 6;
        }
        this.cislo = this.cislo + this.figurka.getTyp().getCislo();
        vyberObrazok();
    }

    /**
     * Metóda vyberá obrazok na základe čísla figurky.
     */
    private void vyberObrazok() {
        switch (this.cislo) {
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

    /**
     * Metóda vracia názov obrazku figurky.
     *
     * @return názov obrazku figurky
     */
    public String getObrazok() {
        return this.obrazok;
    }
}
