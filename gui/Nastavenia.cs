using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace com.example
{
    /// <summary>
    /// Trieda na nastavenie farieb šachovnice.
    /// </summary>
    public class Nastavenia
    {
        // Vo WPF používame SolidColorBrush namiesto Color pre vyplňovanie tvarov
        private static SolidColorBrush svetlaFarba = Brushes.White;
        private static SolidColorBrush tmavaFarba = Brushes.Gray;

        /// <summary>
        /// Zobrazí okno s nastaveniami farieb šachovnice.
        /// </summary>
        public static void zobrazNastavenia(Window rodicovskaStage)
        {
            Window settingsStage = new Window();
            settingsStage.Title = "Nastavenia šachovnice";
            settingsStage.Width = 300;
            settingsStage.Height = 300;
            settingsStage.Owner = rodicovskaStage;
            settingsStage.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            settingsStage.ResizeMode = ResizeMode.NoResize;

            StackPanel root = new StackPanel();
            root.Margin = new Thickness(20);
            root.VerticalAlignment = VerticalAlignment.Center;
            root.HorizontalAlignment = HorizontalAlignment.Center;

            // Výber svetlej farby
            TextBlock svetlaLabel = new TextBlock { Text = "Farba svetlých políčok:", Margin = new Thickness(0, 0, 0, 5) };
            ComboBox svetlyPicker = VytvorColorPicker();
            NastavAktualnuFarbu(svetlyPicker, svetlaFarba);

            // Výber tmavej farby
            TextBlock tmavaLabel = new TextBlock { Text = "Farba tmavých políčok:", Margin = new Thickness(0, 15, 0, 5) };
            ComboBox tmavyPicker = VytvorColorPicker();
            NastavAktualnuFarbu(tmavyPicker, tmavaFarba);

            // Tlačidlo na uloženie
            Button ulozitButton = new Button { Content = "Uložiť", Margin = new Thickness(0, 25, 0, 0), Padding = new Thickness(20, 5, 20, 5) };
            ulozitButton.Click += (s, e) => {
                svetlaFarba = (SolidColorBrush)((ComboBoxItem)svetlyPicker.SelectedItem).Tag;
                tmavaFarba = (SolidColorBrush)((ComboBoxItem)tmavyPicker.SelectedItem).Tag;
                settingsStage.Close(); // Zatvorenie okna
            };

            root.Children.Add(svetlaLabel);
            root.Children.Add(svetlyPicker);
            root.Children.Add(tmavaLabel);
            root.Children.Add(tmavyPicker);
            root.Children.Add(ulozitButton);

            settingsStage.Content = root;
            
            // ShowDialog() zablokuje rodičovské okno, kým sa toto nezavrie
            settingsStage.ShowDialog(); 
        }

        public static SolidColorBrush getSvetlaFarba()
        {
            return svetlaFarba;
        }

        public static SolidColorBrush getTmavaFarba()
        {
            return tmavaFarba;
        }

        /// <summary>
        /// Pomocná metóda na vytvorenie ComboBoxu so zoznamom farieb, 
        /// keďže WPF nemá vstavaný ColorPicker.
        /// </summary>
        private static ComboBox VytvorColorPicker()
        {
            ComboBox cb = new ComboBox();
            cb.Width = 150;
            
            // Načítanie základných farieb z triedy Brushes
            PropertyInfo[] colors = typeof(Brushes).GetProperties(BindingFlags.Static | BindingFlags.Public);
            foreach (PropertyInfo color in colors)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = color.Name;
                item.Tag = (SolidColorBrush)color.GetValue(null, null);
                cb.Items.Add(item);
            }
            return cb;
        }

        /// <summary>
        /// Nastaví vybranú položku v ComboBoxe podľa aktuálnej farby.
        /// </summary>
        private static void NastavAktualnuFarbu(ComboBox cb, SolidColorBrush farba)
        {
            foreach (ComboBoxItem item in cb.Items)
            {
                if (((SolidColorBrush)item.Tag).Color == farba.Color)
                {
                    cb.SelectedItem = item;
                    break;
                }
            }
        }
    }
}