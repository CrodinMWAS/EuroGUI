using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuroGUI
{
    internal class Song
    {
        int ev;
        string orszag;
        string eloado;
        string cim;
        int helyezes;
        int pontszam;

        public Song(int ev, string orszag, string eloado, string cim, int helyezes, int pontszam)
        {
            this.ev = ev;
            this.orszag = orszag;
            this.eloado = eloado;
            this.cim = cim;
            this.helyezes = helyezes;
            this.pontszam = pontszam;
        }

        public int Ev { get => ev;}
        public string Orszag { get => orszag;}
        public string Eloado { get => eloado;}
        public string Cim { get => cim;}
        public int Helyezes { get => helyezes;}
        public int Pontszam { get => pontszam; }
    }
}
