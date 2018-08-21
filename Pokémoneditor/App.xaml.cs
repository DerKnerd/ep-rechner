using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.IO;

namespace Pokémoneditor
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                if (File.Exists(e.Args[0])==true)
                {
                    new MainWindow(e.Args[0]).Show();
                }
            }
            catch (Exception)
            {
                new MainWindow().Show();
            }
        }
    }
}
