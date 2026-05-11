package com.example;

import javafx.application.Application;
import javafx.application.Platform;
import javafx.scene.Group;
import javafx.scene.Scene;
import javafx.scene.control.Alert;
import javafx.scene.image.Image;
import javafx.stage.Stage;
import java.io.IOException;
import javafx.scene.paint.Color;
import javafx.scene.shape.Rectangle;
import javafx.scene.image.ImageView;
import java.net.URL;

import com.example.Logika.Figurka;
import com.example.Logika.Plocha;
import com.example.Obrazky.ObrazFigurky;

import javafx.scene.input.MouseEvent;


public class App extends Application {   

    
    private Plocha plocha = new Plocha(); // Objekt šachovnice, ktorý uchováva aktuálny stav hry
    private Group koren; // Koreňový uzol grafického rozhrania
    private int[] vybranaPozicia;

    /**
     * Hlavná metóda aplikácie.
     *
     * @param args Argumenty príkazového riadku
     */
    public static void main(String[] args) {
        launch(args);
    }

    /**
     * Metóda na spustenie JavaFX aplikácie.
     *
     * @param stage Hlavné okno aplikácie
     * @throws IOException Ak sa vyskytne chyba pri načítaní súboru
     */
    @Override
    public void start(Stage stage) throws IOException {
        this.koren = new Group(); // Inicializácia koreňového uzla
        Scene scena = new Scene(this.koren, Color.BLACK); // Vytvorenie scény s čiernym pozadím

        this.plocha.setCallbackVysledku(sprava -> {
        // UI update musí byť na JavaFX vlákne
            Platform.runLater(() -> {
                Alert alert = new Alert(Alert.AlertType.INFORMATION);
                alert.setTitle("Koniec hry");
                alert.setHeaderText(null);
                alert.setContentText(sprava);
                alert.showAndWait();
            });
        });

        this.vytvorSachovnicu(); // Vytvorenie grafickej šachovnice
        this.pridajObrazok(); // Pridanie figúrok na šachovnicu

        // Nastavenie obsluhy udalosti kliknutia na myš
        scena.setOnMouseClicked(event -> {
            int[] pozicia = this.getPoziciuMysi(event); // Získanie pozície myši
        
            if (this.vybranaPozicia == null) {
                // Ak ešte nie je vybraná pozícia, uložíme prvú pozíciu
                this.vybranaPozicia = pozicia;
            } else {
                // Ak už máme vybranú prvú pozíciu, vykonáme presun figúrky
                int startX = this.vybranaPozicia[1];
                int startY = this.vybranaPozicia[0];
                int endX = pozicia[1];
                int endY = pozicia[0];
                this.plocha.setFigurka(startX, startY, endX, endY); // Aktualizácia šachovnice

                // Resetovanie vybratej pozície
                this.vybranaPozicia = null;
                this.restartujPlochu(); // Obnovenie grafiky šachovnice
            }
        });

        stage.setTitle("Šach"); // Nastavenie názvu okna
        stage.setScene(scena); // Nastavenie scény na okno
        stage.show(); // Zobrazenie okna
    }

    
    /**
     * Metóda na vytvorenie šachovnice.
     */
    private void vytvorSachovnicu() {
        int velkostPolicka = 50; // Veľkosť jedného políčka na šachovnici

        for (int riadok = 0; riadok < 8; riadok++) {
            for (int stlpec = 0; stlpec < 8; stlpec++) {
                Rectangle policko = new Rectangle();
                policko.setX(stlpec * velkostPolicka); // Nastavenie X pozície políčka
                policko.setY(riadok * velkostPolicka); // Nastavenie Y pozície políčka
                policko.setWidth(velkostPolicka); // Šírka políčka
                policko.setHeight(velkostPolicka); // Výška políčka

                // Striedanie farieb políčok
                if ((riadok + stlpec) % 2 == 0) {
                    policko.setFill(Nastavenia.getSvetlaFarba());
                } else {
                    policko.setFill(Nastavenia.getTmavaFarba());
                }

                this.koren.getChildren().add(policko); // Pridanie políčka do grafického rozhrania
            }
        }
    }

    /**
     * Metóda na pridanie figúrok na šachovnicu.
     */
    private void pridajObrazok() {
        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                // Získanie figúrky z aktuálnej pozície na šachovnici
                Figurka figurka = this.plocha.getFigurka(i, j);
    
                // Ak nie je figúrka na pozícii, pokračujeme
                if (figurka == null) {
                    continue;
                }
    
                // Vytvorenie obrazu figúrky na základe typu figúrky
                ObrazFigurky obrazFigurky = new ObrazFigurky(figurka);
                URL url = getClass().getResource("/com/example/Obrazky/" + obrazFigurky.getObrazok());
                String cestaKObrazku = url.toExternalForm();
                Image obrazok = new Image(cestaKObrazku);
                if (obrazok.isError()) {
                    continue; // Preskočiť, ak sa obraz nepodarilo načítať
                }
    
                ImageView pohladObrazu = new ImageView(obrazok);
                pohladObrazu.setX(j * 50); // Nastavenie X pozície obrazu
                pohladObrazu.setY(i * 50); // Nastavenie Y pozície obrazu
                pohladObrazu.setFitWidth(50); // Šírka obrazu
                pohladObrazu.setFitHeight(50); // Výška obrazu
    
                // Pridanie obrazu figúrky do grafického rozhrania
                this.koren.getChildren().add(pohladObrazu);
            }
        }
    }

    /**
     * Metóda na získanie pozície myši na šachovnici.
     *
     * @param event Udalosť kliknutia myšou
     * @return Pole s pozíciou [riadok, stĺpec]
     */
    private int[] getPoziciuMysi(MouseEvent event) {
        int x = (int)(event.getX() / 50); // Vypočíta stĺpec podľa X súradnice
        int y = (int)(event.getY() / 50); // Vypočíta riadok podľa Y súradnice
        return new int[]{x, y}; // Vráti pozíciu ako pole [riadok, stĺpec]
    }

    /**
     * Metóda na obnovenie grafického zobrazenia šachovnice.
     */
    private void restartujPlochu() {
        this.koren.getChildren().clear(); // Vymaže všetky deti koreňa
        this.vytvorSachovnicu(); // Opätovné vytvorenie šachovnice
        this.pridajObrazok(); // Opätovné pridanie figúrok
    }

}