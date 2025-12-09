using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algo_projet_final
{
    internal class Plateau
    {
        public char[,] Grille;
        public int nombrecol;
        public int nombrelig;
        private Dictionary<char, int> maxlettre;


        //Constructeur 
        public Plateau(int lignes, int colonnes, string cheminFichierLettres)
        {
            nombrelig = lignes;
            nombrecol = colonnes;
            Grille = new char[lignes, colonnes];
            maxlettre = new Dictionary<char, int>();
            Dictionary<char, int> compteurLettre = new Dictionary<char, int>();


        }
    }
}
