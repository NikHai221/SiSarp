package com.example;

import javafx.application.Application;
import javafx.scene.Scene;
import javafx.scene.Group;
import javafx.scene.control.Button;
import javafx.scene.layout.VBox;
import javafx.scene.shape.Rectangle;
import javafx.stage.Stage;

/**
 * Hlavné menu aplikácie so šachovnicovým pozadím.
 */
public class StartMenu extends Application {


    /**
     * Metóda na spustenie JavaFX aplikácie.
     *
     * @param primaryStage Hlavné okno aplikácie.
     */
    @Override
    public void start(Stage primaryStage) {
        Group root = new Group(); // Základný koreň

        // Šachovnicové pozadie
        this.vytvorSachovnicuPozadie(root);

        // Tlačidlá
        Button playButton = new Button("Play");
        Button settingsButton = new Button("Nastavenia");
        Button exitButton = new Button("Ukončiť");

        playButton.setStyle("-fx-font-size: 16px; -fx-padding: 10 20;");
        settingsButton.setStyle("-fx-font-size: 16px; -fx-padding: 10 20;");
        exitButton.setStyle("-fx-font-size: 16px; -fx-padding: 10 20;");

        playButton.setOnAction(event -> {
            try {
                App hra = new App();
                hra.start(primaryStage);
            } catch (Exception e) {
                e.printStackTrace();
            }
        });

        settingsButton.setOnAction(event -> this.zobrazNastavenia(primaryStage));
        exitButton.setOnAction(event -> System.exit(0));

        VBox menuBox = new VBox(20, playButton, settingsButton, exitButton);
        menuBox.setLayoutX(150);
        menuBox.setLayoutY(150);
        menuBox.setStyle("-fx-alignment: center;");

        root.getChildren().add(menuBox);

        Scene scene = new Scene(root, 400, 400);
        primaryStage.setTitle("Šach - Menu");
        primaryStage.setScene(scene);
        primaryStage.show();
    }

    /**
     * Vytvorí šachovnicové pozadie.
     *
     * @param root Koreňový uzol, do ktorého sa pridá šachovnica.
     */
    private void vytvorSachovnicuPozadie(Group root) {
        int velkostPolicka = 50;
        for (int riadok = 0; riadok < 8; riadok++) {
            for (int stlpec = 0; stlpec < 8; stlpec++) {
                Rectangle policko = new Rectangle();
                policko.setX(stlpec * velkostPolicka);
                policko.setY(riadok * velkostPolicka);
                policko.setWidth(velkostPolicka);
                policko.setHeight(velkostPolicka);

                if ((riadok + stlpec) % 2 == 0) {
                    policko.setFill(Nastavenia.getSvetlaFarba());
                } else {
                    policko.setFill(Nastavenia.getTmavaFarba());
                }

                root.getChildren().add(policko);
            }
        }
    }

    /**
     * Zobrazí nastavenia šachovnice.
     *
     * @param stage Hlavné okno aplikácie.
     */
    private void zobrazNastavenia(Stage stage) {
        Nastavenia.zobrazNastavenia(stage);
    }

    /**
     * Hlavná metóda na spustenie aplikácie.
     *
     * @param args Argumenty príkazového riadku.
     */
    public static void main(String[] args) {
        launch(args);
    }
}
