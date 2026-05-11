package com.example;

import javafx.scene.Scene;
import javafx.scene.control.Button;
import javafx.scene.control.ColorPicker;
import javafx.scene.control.Label;
import javafx.scene.layout.VBox;
import javafx.scene.paint.Color;
import javafx.stage.Stage;

/**
 * Trieda na nastavenie farieb šachovnice.
 */
public class Nastavenia {

    private static Color svetlaFarba = Color.WHITE;
    private static Color tmavaFarba = Color.GRAY;

    /**
     * Zobrazí okno s nastaveniami farieb šachovnice.
     *
     * @param rodicovskaStage Rodičovské okno, ktoré sa použije ako vlastník pre nové okno.
     */
    public static void zobrazNastavenia(Stage rodicovskaStage) {
        Stage settingsStage = new Stage();

        // Výber farieb
        Label svetlaLabel = new Label("Farba svetlých políčok:");
        ColorPicker svetlyPicker = new ColorPicker(svetlaFarba);

        Label tmavaLabel = new Label("Farba tmavých políčok:");
        ColorPicker tmavyPicker = new ColorPicker(tmavaFarba);

        // Tlačidlo na uloženie
        Button ulozitButton = new Button("Uložiť");
        ulozitButton.setOnAction(e -> {
            svetlaFarba = svetlyPicker.getValue();
            tmavaFarba = tmavyPicker.getValue();
            settingsStage.close();
        });

        VBox root = new VBox(10);
        root.setStyle("-fx-padding: 20; -fx-alignment: center;");
        root.getChildren().addAll(svetlaLabel, svetlyPicker, tmavaLabel, tmavyPicker, ulozitButton);

        Scene scene = new Scene(root, 300, 250);
        settingsStage.setTitle("Nastavenia šachovnice");
        settingsStage.setScene(scene);
        settingsStage.initOwner(rodicovskaStage);
        settingsStage.show();
    }

    /**
     * Získa aktuálne nastavené farby šachovnice.
     *
     * @return Pole farieb [svetlá, tmavá]
     */
    public static Color getSvetlaFarba() {
        return svetlaFarba;
    }

    /**
     * Získa aktuálne nastavené farby šachovnice.
     *
     * @return Pole farieb [svetlá, tmavá]
     */
    public static Color getTmavaFarba() {
        return tmavaFarba;
    }
}
