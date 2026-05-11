package com.example.Logika;

import java.util.function.Consumer;

public class Plocha {
    private Figurka[][] plocha; // Dvojrozmerné pole, ktoré reprezentuje šachovnicu a jej figúrky
    private Strana aktualnaStrana; // Označuje, ktorá strana je na ťahu
    private boolean hraSkoncila; // Indikátor, či hra skončila
    private Consumer<String> callbackVysledku; //callback pre GUI, ktorý sa zavolá pri zobrazení výsledku hry

    /**
     * Konštruktor triedy Plocha inicializuje šachovnicu a rozmiestni figúrky na ich začiatočné pozície.
     */
    public Plocha() {
        this.aktualnaStrana = Strana.BIELA;
        this.hraSkoncila = false;
        this.plocha = new Figurka[8][8];
        
        // Inicializácia všetkých polí šachovnice na null
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                this.plocha[i][j] = null;
            }
        }

        // Inicializácia bielych figúrok
        this.plocha[0][0] = new Veza(0, 0, Strana.BIELA);
        this.plocha[0][1] = new Jazdec(0, 1, Strana.BIELA);
        this.plocha[0][2] = new Strelec(0, 2, Strana.BIELA);
        this.plocha[0][3] = new Kral(0, 3, Strana.BIELA);
        this.plocha[0][4] = new Dama(0, 4, Strana.BIELA);
        this.plocha[0][5] = new Strelec(0, 5, Strana.BIELA);
        this.plocha[0][6] = new Jazdec(0, 6, Strana.BIELA);
        this.plocha[0][7] = new Veza(0, 7, Strana.BIELA);

        for (int i = 0; i < 8; i++) {
            this.plocha[1][i] = new Pesiak(1, i, Strana.BIELA);
        }

        // Inicializácia čiernych figúrok
        this.plocha[7][0] = new Veza(7, 0, Strana.CIERNA);
        this.plocha[7][1] = new Jazdec(7, 1, Strana.CIERNA);
        this.plocha[7][2] = new Strelec(7, 2, Strana.CIERNA);
        this.plocha[7][3] = new Kral(7, 3, Strana.CIERNA);
        this.plocha[7][4] = new Dama(7, 4, Strana.CIERNA);
        this.plocha[7][5] = new Strelec(7, 5, Strana.CIERNA);
        this.plocha[7][6] = new Jazdec(7, 6, Strana.CIERNA);
        this.plocha[7][7] = new Veza(7, 7, Strana.CIERNA);

        for (int i = 0; i < 8; i++) {
            this.plocha[6][i] = new Pesiak(6, i, Strana.CIERNA);
        }
    }

    /**
     * Metóda na získanie figúrky na špecifickej pozícii na šachovnici.
     * 
     * @param x Aktuálny index riadku figúrky.
     * @param y Aktuálny index stlpca figúrky.
     * @return Figúrka na danej pozícii alebo null, ak tam žiadna nie je.
     */
    public Figurka getFigurka(int x, int y) {
        return this.plocha[x][y];
    }

    /**
     * Metóda na presunutie figúrky z jednej pozície na inú.
     * Zahŕňa kontrolu platnosti pohybu, poradia hráčov a špeciálnych pravidiel (napr. pešiak na konci šachovnice).
     * 
     * @param x Aktuálny index riadku figúrky.
     * @param y Aktuálny index stlpca figúrky.
     * @param novyX Cieľový index riadku.
     * @param novyY Cieľový index stlpca.
     */
    public void setFigurka(int x, int y, int novyX, int novyY) {
        if (this.hraSkoncila) {
            System.out.println("Hra uz skoncila. Nie je mozne vykonat dalsi tah.");
            return;
        }
        if (x == 0 && y == 3 && novyX == 0 && (novyY == 0 || novyY == 7) && this.poradie(x, y)) {
            if (this.getFigurka(x, y).getTyp() == TypFigurky.KRAL && !(this.getFigurka(x, y).getZmena())) {
                if (this.rosada(Strana.BIELA, novyX, novyY)) {
                    this.zmenPoradie();
                    return;
                }
            }
        } else if (x == 7 && y == 3 && novyX == 7 && (novyY == 0 || novyY == 7) && this.poradie(x, y)) {
            if (this.getFigurka(x, y).getTyp() == TypFigurky.KRAL && !(this.getFigurka(x, y).getZmena())) {
                if (this.rosada(Strana.CIERNA, novyX, novyY)) {
                    this.zmenPoradie();
                    return;
                }
            }
        }



        if (this.plocha[novyX][novyY] != null &&
            this.plocha[x][y].getStrana() == this.plocha[novyX][novyY].getStrana()) {
            System.out.println("Chyba: Nie je mozne sa presunut na poziciu, ktoru obsadzuje figurka tej istej strany.");
            return;
        } else if (!this.poradie(x, y)) {
            System.out.println(this.aktualnaStrana + " je na tahu");
            return;
        } else if (this.getFigurka(x, y).preskakujeFigurky(novyX, novyY, this)) {
            System.out.println("Chyba: Figurka nemoze preskakovat ine figurky.");
            return;
        } else if (!this.plocha[x][y].validnyPohyb(novyX, novyY, this)) {
            System.out.println("Chyba: Neplatny tah.");
            return;
        } else if (this.getFigurka(x, y).getTyp() == TypFigurky.PESIAK) {
            if (this.plocha[x][y].getStrana() == Strana.BIELA && novyX == 7) {
                this.plocha[novyX][novyY] = new Dama(novyX, novyY, Strana.BIELA);
            } else if (this.plocha[x][y].getStrana() == Strana.CIERNA && novyX == 0) {
                this.plocha[novyX][novyY] = new Dama(novyX, novyY, Strana.CIERNA);
            } else {
                this.plocha[novyX][novyY] = this.plocha[x][y];
                this.plocha[novyX][novyY].setPozicia(novyX, novyY);
            }
        } else {
            this.plocha[novyX][novyY] = this.plocha[x][y];
            this.plocha[novyX][novyY].setPozicia(novyX, novyY);
        } 

        this.plocha[x][y] = null;

        this.zmenPoradie();
        this.kontrolaVyhry();
    }


    /**
     * Metóda na kontrolu, či figúrka preskakuje iné figúrky na ceste k cieľovej pozícii.
     * 
     * @param x Aktuálny index riadku figúrky.
     * @param y Aktuálny index stlpca figúrky.
     * @param novyX Cieľový index riadku.
     * @param novyY Cieľový index stlpca.
     * @return True, ak figúrka preskakuje iné figúrky, inak false.
     */
    

    /**
     * Metóda na kontrolu, či figúrka na danej pozícii patrí hráčovi, ktorý je na ťahu.
     * 
     * @param x Aktuálny index riadku figúrky.
     * @param y Aktuálny index stlpca figúrky.
     * @return True, ak figúrka patrí hráčovi na ťahu, inak false.
     */
    public boolean poradie(int x, int y) {
        if (this.plocha[x][y] == null) {
            System.out.println("Chyba: Na pozícii (" + x + ", " + y + ") sa nenachadza ziadna figurka.");
            return false;
        }

        if (this.plocha[x][y].getStrana() != this.aktualnaStrana) {
            System.out.println("Chyba: Nie je tah strany " + this.plocha[x][y].getStrana() + ".");
            return false;
        }

        return true;
    }

    /**
     * Metóda na zistenie, či hra skončila.>
     * 
     * @return True, ak hra skončila, inak false.
     */
    public void kontrolaVyhry() {
        boolean bielyKralZije = false;
        boolean ciernyKralZije = false;

        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                Figurka figurka = this.plocha[i][j];
                if (figurka != null && figurka.getTyp() == TypFigurky.KRAL) {
                    if (figurka.getStrana() == Strana.BIELA) {
                        bielyKralZije = true;
                    } else if (figurka.getStrana() == Strana.CIERNA) {
                        ciernyKralZije = true;
                    }
                }
            }
        }

        if (!bielyKralZije) {
            this.zobrazVysledok("CIARNA VYHRALA");
            this.hraSkoncila = true;
        } else if (!ciernyKralZije) {
            this.zobrazVysledok("BIELA VYHRALA");
            this.hraSkoncila = true;
        }
    }

    /**
     * Metóda na nastavenie callbacku pre zobrazenie výsledku hry.
     * 
     * @param callback Callback, ktorý sa má zavolať pri zobrazení výsledku.
     */
    public void setCallbackVysledku(Consumer<String> callback) {
        this.callbackVysledku = callback;
    }

    /**
     * Metóda na zobrazenie výsledku hry.
     * 
     * @param sprava Správa, ktorá sa má zobraziť.
     */
    private void zobrazVysledok(String sprava) {
        System.out.println(sprava);
        if (this.callbackVysledku != null) {
            this.callbackVysledku.accept(sprava);
        }
    }

    /**
     * Metóda na zmenu poradia hráčov.
     * Ak je aktuálna strana biela, zmení ju na čiernu a naopak.
     */
    private void zmenPoradie() {
        if (this.aktualnaStrana == Strana.BIELA) {
            this.aktualnaStrana = Strana.CIERNA;
        } else {
            this.aktualnaStrana = Strana.BIELA;
        }

    }

    /**
     * Metóda na kotrolu, ci sa moze vykonat rosada.>
     * 
     * @param strana Strana, ktorej sa rosada tyka.
     * @param novyX Cielovy index riadku.
     * @param novyY Cielovy index stlpca.
     * 
     * @return True, ak mozme vykonat, inak false.
     */
    private boolean rosada(Strana strana, int novyX, int novyY) {
        if (strana == Strana.BIELA) {
            if (this.getFigurka(novyX, novyY).getTyp() == TypFigurky.VEZA && !(this.getFigurka(novyX, novyY).getZmena())) {
                if (novyY == 0) {
                    if (this.plocha[0][1] == null && this.plocha[0][2] == null) {
                        
                        this.plocha[0][1] = this.plocha[0][3];
                        this.plocha[0][1].setPozicia(0, 1);
                        this.plocha[0][3] = null;
                        this.plocha[0][2] = this.plocha[0][0];
                        this.plocha[0][2].setPozicia(0, 2);
                        this.plocha[0][0] = null;
                        return true;
                    }
                } else if (novyY == 7) {
                    if (this.plocha[0][4] == null && this.plocha[0][5] == null && this.plocha[0][6] == null) {
                        
                        this.plocha[0][5] = this.plocha[0][3];
                        this.plocha[0][5].setPozicia(0, 5);
                        this.plocha[0][3] = null;
                        this.plocha[0][4] = this.plocha[0][7];
                        this.plocha[0][4].setPozicia(0, 4);
                        this.plocha[0][7] = null;
                        return true;
                    }
                }
            }
            return false;
        } else if (strana == Strana.CIERNA) {
            if (this.getFigurka(novyX, novyY).getTyp() == TypFigurky.VEZA && !(this.getFigurka(novyX, novyY).getZmena())) {
                if (novyY == 0) {
                    if (this.plocha[7][1] == null && this.plocha[7][2] == null) {
                        
                        this.plocha[7][1] = this.plocha[7][3];
                        this.plocha[7][1].setPozicia(7, 1);
                        this.plocha[7][3] = null;
                        this.plocha[7][2] = this.plocha[7][0];
                        this.plocha[7][2].setPozicia(7, 2);
                        this.plocha[7][0] = null;
                        return true;
                    }
                } else if (novyY == 7) {
                    if (this.plocha[7][4] == null && this.plocha[7][5] == null && this.plocha[7][6] == null) {
                        this.plocha[7][5] = this.plocha[7][3];
                        this.plocha[7][5].setPozicia(7, 5);
                        this.plocha[7][3] = null;
                        this.plocha[7][4] = this.plocha[7][7];
                        this.plocha[7][4].setPozicia(7, 4);
                        this.plocha[7][7] = null;
                        return true;
                    }
                }
            }
        }
        return false;
    }

    



    
}
