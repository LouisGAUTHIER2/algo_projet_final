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
        //Constructeur
        public Joueur(string nom)
        {
            Nom = nom;
            Score = 0;
            MotsTrouves = new List<string>();
        }
        //Ajoute un mot à la liste de mots trouvés
        public void Add_Mot(string mot)
        {
            if (!Contient(mot))
            {
                MotsTrouves.Add(mot.ToLower());
            }
        }
        //Vérifie si le mot est déjà dans la liste
        public bool Contient(string mot)
        {
            return MotsTrouves.Contains(mot.ToLower());
        }

        //Ajoute un point si le mot est nouveau dans la liste
        public void Add_Score(int points)
        {
            Score += points;
        }

        //Affiche les différentes information et sur les joueurs
        public void AfficherInfos()
        {
            Console.WriteLine("Nom : " + Nom);
            Console.WriteLine("Score : " + Score);
            Console.WriteLine("Mots trouvés : " + (MotsTrouves.Count > 0 ? string.Join(", ", MotsTrouves) : "Aucun"));
            Console.WriteLine();
        }

        public string toString()
        {
            return $"Nom: {Nom}, Score: {Score}, Mots Trouvés: {(MotsTrouves.Count > 0 ? string.Join(", ", MotsTrouves) : "Aucun")}";
        }
    }
}

