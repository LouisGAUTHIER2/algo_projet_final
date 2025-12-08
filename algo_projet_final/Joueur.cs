using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algo_projet_final
{
    internal class Joueur
    {
        public string Nom;
        public int Score;
        public List<string> MotsTrouves;

        public Joueur(string nom)
        {
            Nom = nom;
            Score = 0;
            MotsTrouves = new List<string>();
        }
        public void Add_Mot(string mot)
        {
            if (!Contient(mot))
            {
                MotsTrouves.Add(mot.ToLower());
            }
        }
        public bool Contient(string mot)
        {
            return MotsTrouves.Contains(mot.ToLower());
        }
        public void Add_Score(int points)
        {
            Score += points;
        }
        public void AfficherInfos()
        {
            Console.WriteLine("Nom : " + Nom);
            Console.WriteLine("Score : " + Score);
            Console.WriteLine("Mots trouvés : " + (MotsTrouves.Count > 0 ? string.Join(", ", MotsTrouves) : "Aucun"));
            Console.WriteLine();
        }
    }
}
}
