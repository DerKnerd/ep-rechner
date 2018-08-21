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
using System.Diagnostics;

namespace Pokémoneditor
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(string datei)
        {
            InitializeComponent();
            open(datei);
        }

        PokémonCollection _pokémon = new PokémonCollection();
        ICollectionView _pokémoncollection;
        Pokémon dummy;
        string dateiname;
        bool gespeichert = false;
        int index;
        int index2;

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Multiselect = false,
                Filter = "Alle Bilddateitypen(*.jpg, *.jpeg, *.jfif, *.jpe, *.png, *.gif, *.bmp)|*.jpg;*.jpeg;*.jfif;*.jpe;*.png;*.gif;*.bmp|JPEG-Datei(*.jpg, *.jpeg, *.jfif, *.jpe)|*.jpg;*.jpeg;*.jfif;*.jpe|Portable Network Graphics(.*png)|*.png|Graphics Interchange Format(*.gif)|*.gif|TIFF-Datei(*.tif, *.tiff)|*.tif;*.tiff|Windows-Bitmap(*.bmp)|*.bmp;*.dib",
                FilterIndex = 0
            };
            if (ofd.ShowDialog() == true)
                sprite.Text = ofd.FileName;
        }
        private void save(string filename)
        {
            gespeichert = true;
            XmlSerializer serializer = new XmlSerializer(_pokémon.GetType());

            StreamWriter sw = new StreamWriter(filename);
            serializer.Serialize(sw, _pokémon);
            sw.Close();
        }
        private void open(string filename)
        {
            gespeichert = true;
            XmlSerializer serializer = new XmlSerializer(typeof(PokémonCollection));

            StreamReader sr = new StreamReader(filename);
            PokémonCollection poke = (PokémonCollection)serializer.Deserialize(sr);
            setView(poke);
            listView1.SelectedItem = null;
        }
        private void setView(PokémonCollection poké)
        {
            this._pokémon = poké;
            this._pokémoncollection = new ListCollectionView(poké);
            this.DataContext = _pokémoncollection;
            listView1.SelectedIndex = _pokémon.Count - 1;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CommandBindings.Add(new CommandBinding(ApplicationCommands.New, new_Executed));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, open_Executed));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, save_Executed));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.SaveAs, saveas_Executed));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Help, help_Executed));
        }
        private void new_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (gespeichert == false)
            {
                switch (MessageBox.Show("Wollen Sie die Liste speichern?", "Speichern", MessageBoxButton.YesNoCancel))
                {
                    case MessageBoxResult.Cancel:
                        break;
                    case MessageBoxResult.No:
                        add.IsEnabled = true;
                        gespeichert = false;
                        dateiname = null;
                        _pokémon = new PokémonCollection();
                        setView(_pokémon);
                        break;
                    case MessageBoxResult.Yes:
                        if (dateiname == null)
                        {
                            SaveFileDialog sfd = new SaveFileDialog()
                            {
                                DefaultExt = "*.poké|*.poké",
                                Filter = "*.poké|*.poké",
                                AddExtension = true,
                                Title = "Pokémonliste speichern..."
                            };
                            if (sfd.ShowDialog() == true)
                            {
                                save(sfd.FileName);
                                dateiname = sfd.FileName;
                            }
                        }
                        else
                        {
                            save(dateiname);
                        }
                        break;
                }
            }
        }
        private void open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                DefaultExt = "*.poké|*.poké",
                Filter = "*.poké|*.poké|*.xml|*.xml",
                Title = "Pokémonliste öffnen...",
                Multiselect = false
            };
            if (ofd.ShowDialog() == true)
            {
                open(ofd.FileName);
                dateiname = ofd.FileName;
            }
        }
        private void save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (dateiname == null)
            {
                SaveFileDialog sfd = new SaveFileDialog()
                {
                    DefaultExt = "*.poké|*.poké",
                    Filter = "*.poké|*.poké",
                    AddExtension = true,
                    Title = "Pokémonliste speichern..."
                };
                if (sfd.ShowDialog() == true)
                {
                    save(sfd.FileName);
                    dateiname = sfd.FileName;
                }
            }
            else
            {
                save(dateiname);
            }
        }
        private void saveas_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                DefaultExt = "*.poké|*.poké",
                Filter = "*.poké|*.poké",
                AddExtension = true,
                Title = "Pokémonliste speichern..."
            };
            if (sfd.ShowDialog() == true)
            {
                save(sfd.FileName);
                dateiname = sfd.FileName;
            }
        }
        private void help_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            (new Help() as Window).Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            gespeichert = false;
            if (basiswert.Text == "")
                MessageBox.Show("Bitte den Basiswert eingeben", "Basiswert", MessageBoxButton.OK);
            if (nummer.Text == "")
                MessageBox.Show("Bitte die Nummer eingeben", "Nummer", MessageBoxButton.OK);
            if (name.Text == "")
                MessageBox.Show("Bitte den Namen eingeben", "Name", MessageBoxButton.OK);
            if (sprite.Text == "")
                MessageBox.Show("Bitte den Pfad zum Sprite eingeben", "Sprite", MessageBoxButton.OK);
            if (basiswert.Text != "" && nummer.Text != "" && name.Text != "" && sprite.Text != "")
            {
                dummy = new Pokémon();
                dummy.Basiswert = basiswert.Text;
                dummy.Name = name.Text;
                dummy.Nummer = nummer.Text;
                dummy.Sprite = sprite.Text;
                _pokémon.Insert(index2 + 1, dummy);
                setView(_pokémon);
                add.IsEnabled = false;
            }
        }
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            add.IsEnabled = true;
            index2 = index;
            listView1.SelectedItem = null;
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (gespeichert == false)
            {
                switch (MessageBox.Show("Wollen Sie die Liste speichern?", "Speichern", MessageBoxButton.YesNoCancel))
                {
                    case MessageBoxResult.Cancel: e.Cancel = true;
                        break;
                    case MessageBoxResult.No:
                        break;
                    case MessageBoxResult.Yes:
                        if (dateiname == null)
                        {
                            SaveFileDialog sfd = new SaveFileDialog()
                            {
                                DefaultExt = "*.poké|*.poké",
                                Filter = "*.poké|*.poké",
                                AddExtension = true,
                                Title = "Pokémonliste speichern..."
                            };
                            if (sfd.ShowDialog() == true)
                            {
                                save(sfd.FileName);
                                dateiname = sfd.FileName;
                            }
                        }
                        else
                        {
                            save(dateiname);
                        }
                        break;
                }
            }
        }
        private void delete_Click(object sender, RoutedEventArgs e)
        {
            switch (MessageBox.Show("Wollen Sie das Pokémon wirklich aus der Liste entfernen?", "Wirklich löschen?", MessageBoxButton.YesNo, MessageBoxImage.Warning))
            {
                case MessageBoxResult.No:
                    break;
                case MessageBoxResult.Yes: _pokémon.RemoveAt(listView1.SelectedIndex);
                    setView(_pokémon);
                    gespeichert = false;
                    break;
            }
        }
        private void listView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            index2 = index;
            index = listView1.SelectedIndex;
        }
    }
}