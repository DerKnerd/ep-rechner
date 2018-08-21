using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xaml;
using System.Xml.Serialization;
using Microsoft.Win32;
using System.ComponentModel;
using System.Windows.Threading;
using Pokémoneditor;

namespace EP_Rechner
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            dt.Tick += new EventHandler(dt_Tick);
        }

        void dt_Tick(object sender, EventArgs e)
        {
            listView1.SelectionChanged += new SelectionChangedEventHandler(listView1_SelectionChanged);
        }

        DispatcherTimer dt = new DispatcherTimer()
        {
            Interval = TimeSpan.FromMilliseconds(2)
        };
        PokémonCollection _pokémon = new PokémonCollection();
        ICollectionView _pokémoncollection;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;
        GridViewColumnHeader _lastClickedHeader = null;
        
        private void berechnen(byte b)
        {
            byte L = (byte)slider1.Value;
            float a = 1;
            if (getauscht.IsChecked == true)
                a += 0.5f;
            if (trainerkampf.IsChecked == true)
                a += 0.5f;
            float e = a * (b * L / 7);
            if (glücksei.IsChecked == true)
                e *= 1.5f;
            runep.Text = Math.Round(e).ToString();
        }
        private void open(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PokémonCollection));

            StreamReader sr = new StreamReader(filename);
            PokémonCollection poke = (PokémonCollection)serializer.Deserialize(sr);
            setView(poke);
        }
        private void setView(PokémonCollection poké)
        {
            this._pokémon = poké;
            this._pokémoncollection = new ListCollectionView(poké);
            this.DataContext = _pokémoncollection;
            listView1.SelectedIndex = _pokémon.Count - 1;
        }
        private void Sort(string sortBy, ListSortDirection direction)
        {
            _pokémoncollection.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            _pokémoncollection.SortDescriptions.Add(sd);
            _pokémoncollection.Refresh();
        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            berechnen(Convert.ToByte(basiswert.Text.ToString()));
        }
        private void listView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            berechnen(Convert.ToByte(basiswert.Text.ToString()));
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".poké";
            ofd.Filter = "poké-Datei(.poké)|*.poké|xml-Datei(.xml)|*.xml";
            ofd.Title = "poké-Datei öffnen";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == true)
            {
                open(ofd.FileName);
            }
        }
        private void gvcheader_Click(object sender, RoutedEventArgs e)
        {
            ListSortDirection direction;
            GridViewColumnHeader clickedHeader = e.OriginalSource as GridViewColumnHeader;
            if (clickedHeader != null)
            {
                if (clickedHeader.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (clickedHeader != _lastClickedHeader)
                        direction = ListSortDirection.Ascending;
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                            direction = ListSortDirection.Descending;
                        else
                            direction = ListSortDirection.Ascending;
                    }

                    string header = (clickedHeader.Column.DisplayMemberBinding as Binding).Path.Path;
                    Sort(header, direction);

                    _lastClickedHeader = clickedHeader;
                    _lastDirection = direction;
                }
            }
        }
        private void items_werte_Checked(object sender, RoutedEventArgs e)
        {
            berechnen(Convert.ToByte(basiswert.Text.ToString()));
        }
        private void items_werte_Unchecked(object sender, RoutedEventArgs e)
        {
            berechnen(Convert.ToByte(basiswert.Text.ToString()));
        }
    }
}
