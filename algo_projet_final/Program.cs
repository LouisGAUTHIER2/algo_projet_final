using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algo_projet_final
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Joueur> joueurs = new List<Joueur>();

            Console.Write("Entrez le nom du premier joueur : ");
            string nom1 = Console.ReadLine();

            Joueur joueur1 = new Joueur(nom1);
            joueurs.Add(joueur1);

            string nom2;
            do
            {
                Console.Write("Entrez le nom du second joueur : ");
                nom2 = Console.ReadLine();
                if (nom2.ToLower() == nom1.ToLower())
                {
                    Console.WriteLine("Ce nom est déjà pris, choisissez-en un autre.");
                }
            }
            while (nom2.ToLower() == nom1.ToLower());


            // Remplace le chemin par l'endroit où tu as mis Lettres.txt
            string cheminLettres = @"C:\Users\pefer\OneDrive\Documents\EsiLv\Algo projet\algo_projet_final\Lettre.txt";
            int nbLignes = 8;
            int nbColonnes = 8;

            // Création d'un plateau aléatoire
            Plateau p = new Plateau(cheminLettres, nbLignes, nbColonnes);

            // Affichage du plateau dans la console
            string choix = "1";

            while (choix == "1")
            {
                Console.WriteLine("Plateau généré :");
                Console.WriteLine("");
                Console.WriteLine(p.ToString());

                Console.WriteLine("\nAppuyez sur 1 pour régénérer la grille, ou sur une autre touche pour continuer le jeu :");
                choix = Console.ReadLine();

                if (choix == "1")
                {
                    p = new Plateau(cheminLettres, nbLignes, nbColonnes); // ou ta fonction de génération
                }
            }

            // Ici on sort de la boucle → le jeu continue
            Console.WriteLine("\nLe jeu commence...");

            // Attend une touche pour fermer la fenêtre console
            Console.WriteLine("Appuie sur une touche pour quitter...");

            Console.ReadKey();
        }
    }
}
