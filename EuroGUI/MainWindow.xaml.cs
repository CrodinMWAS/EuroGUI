using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace EuroGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static MySqlConnection connection = new MySqlConnection("datasource=127.0.0.1;port=3306;database=eurovizio;username=root;password=;");
        List<Song> songs = new List<Song>();
        public MainWindow()
        {
            InitializeComponent();

            try
            {
                connection.Open();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
                Environment.Exit(1);
            }

            loadData();
            connection.Close();
        }

        public void loadData()
        {
            try
            {
                MySqlDataReader returned = Query("SELECT * FROM dal");
                while (returned.Read())
                {
                    Song newSong = new Song(
                        returned.GetInt32("ev"),
                        returned.GetString("orszag"),
                        returned.GetString("eloado"),
                        returned.GetString("cim"),
                        returned.GetInt32("helyezes"),
                        returned.GetInt32("pontszam")
                        );
                    songs.Add(newSong);
                }
                returned.Close();
                dgOutput.ItemsSource = songs;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
                Environment.Exit(1);
                throw;
            }
        }

        public MySqlDataReader Query(string queryString)
        {
            MySqlCommand mySqlCommand = new MySqlCommand(queryString,connection);
            try
            {
                MySqlDataReader reader = mySqlCommand.ExecuteReader();
                return reader;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
                Environment.Exit(1);
                throw;
            }
        }

        private void btn1_click(object sender, RoutedEventArgs e)
        {
            int hungarians = 0;
            int place = songs.Count();
            foreach (var item in songs)
            {
                if (item.Orszag == "Magyarország")
                {
                    hungarians += 1;
                    if (item.Helyezes < place)
                    {
                        place = item.Helyezes;
                    }
                }
            }
            MessageBox.Show($"{hungarians} Magyar versenyző van és {place} a legjobb helyezés");
        }

        private void btn2_click(object sender, RoutedEventArgs e)
        {
            double sum = 0;
            double number = 0;
            foreach (var item in songs)
            {
                if (item.Orszag == "Németország")
                {
                    sum += item.Pontszam;
                    number += 1;
                }
            }
            MessageBox.Show($"Németország pontátlaga : {(sum/number).ToString("#.##")}");
        }

        private void btn3_click(object sender, RoutedEventArgs e)
        {
            string exit = "";
            foreach (var item in songs)
            {
                if (item.Eloado.Contains("Luck") || item.Cim.Contains("Luck"))
                {
                    exit += $"{item.Eloado} - {item.Cim},\n";
                }
            }
            MessageBox.Show(exit);
        }

        private void btn4_click(object sender, RoutedEventArgs e)
        {
            List<string> rwresults = new List<string>();
            List<string> results = new List<string>();
            foreach (var item in songs)
            {
                if (item.Eloado.Contains(tbSearchBar.Text))
                {
                    rwresults.Add(item.Eloado + ";" + item.Cim);
                }
            }
            rwresults.Sort();
            foreach (var item in rwresults)
            {
                results.Add(item.Split(";")[1]);
            }
            results.Sort();
            lbSearchAnswer.ItemsSource = results;
        
        }

        private void btn5_click(object sender, RoutedEventArgs e)
        {
            int index = dgOutput.SelectedIndex;
            if (index != -1)
            {
                Song searchFor = songs[index];
                
                connection.Open();
                MySqlDataReader returned = Query($"SELECT * FROM verseny WHERE ev = {searchFor.Ev}");
                while (returned.Read())
                {
                    lblDatum.Content = ($"{returned.GetString("datum")} {returned.GetString("varos")} {returned.GetString("orszag")} {returned.GetString("induloszam")}");
                }

                connection.Close();
            }
        }
    }
}
