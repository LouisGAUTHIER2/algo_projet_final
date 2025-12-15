using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Versioning;
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
        public Joueur()
        {
            string nom = "";
            Console.WriteLine("Entrez un nom de joueur valide :");
            do
            {
                TerminalClass.ClearLine();
                nom = Console.ReadLine();
                Console.SetCursorPosition(0, Console.CursorTop - 1);
            } while (nom == "");

            Console.WriteLine();

            this.Nom = nom;
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

        //Ajoute différents points pour chaque mot
        public void Add_Score(string mot)
        {
            int Score = 0;
            string[] lignes = File.ReadAllLines("Lettre.txt");

            foreach (char c in mot.ToUpper())
            {
                foreach (string l in lignes)
                {
                    string[] parts = l.Split(',');

                    if (parts[0][0] == c)
                    {
                        Score += Convert.ToInt32(parts[2]);
                        break;
                    }
                }
            }

            this.Score += Score;
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

