using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Pokémoneditor
{
    [Serializable]
    public class Pokémon
    {
        public Pokémon()
        {
            
        }

        private string nummer;
        private string name;
        private string basiswert;
        private string sprite;

        public string Nummer
        {
            get { return nummer; }
            set { nummer = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Basiswert
        {
            get { return basiswert; }
            set { basiswert = value; }
        }
        public string Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }
    }
    [Serializable]
    public class PokémonCollection : ObservableCollection<Pokémon>
    {
    }
}
