package com.example.Logika;

/**
 * Enum TypFigurky reprezentuje rôzne typy figúrok používaných v šachu.
 * Každý typ figúrky je priradený k určitému číselnému hodnoteniu.
 */
public enum TypFigurky {
    PESIAK(1),
    JAZDEC(2),
    VEZA(3),
    STRELEC(4),
    KRAL(5),
    DAMA(6);

    private int cislo; //Číselná hodnota figúrky

    /**
     * Privátny konštruktor, ktorý nastavuje číselnú hodnotu figúrky.
     * 
     * @param cislo číselná hodnota priradená k figúrke
     */
    TypFigurky(int cislo) {
        this.cislo = cislo;
    }

    /**
     * Metóda na získanie číselnej hodnoty figúrky.
     * 
     * @return číselná hodnota figúrky
     */
    public int getCislo() {
        return this.cislo;
    }
}
