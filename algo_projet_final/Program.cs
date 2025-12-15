using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace algo_projet_final
{
    internal class Program
    {
    static Jeu jeu;
    static long startingTime;
    static long timeLimit = 60;
    
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("==== CONFIGURATION =====");
            Console.WriteLine();

            // Entrée des noms des joueurs (avec possibilité de quitter)
            Joueur joueur1 = new Joueur();
            Joueur joueur2 = new Joueur();

            Dictionnaire dico = new Dictionnaire("Mots_Français.txt", "francais");

            Console.WriteLine("Entrer la taille de votre grille ");
            int nbLignes = Convert.ToInt32(Console.ReadLine());

            // Création d'un plateau aléatoire
            Plateau p = new Plateau("Lettre.txt", nbLignes, nbLignes);
            string choix;

            do
            {
                Console.Clear();
                Console.WriteLine("==== CHOIX DE LA TAILLE DE LA GRILLE =====");
                Console.WriteLine();
                Console.WriteLine("Plateau généré :");
                Console.WriteLine("");
                Console.WriteLine(p.ToString());

                Console.WriteLine("\nAppuyez sur 1 pour régénérer la grille, ou sur Entrer pour continuer le jeu :");
                choix = Console.ReadLine();

                if (choix == "1")
                {
                    p = new Plateau("Lettre.txt", nbLignes, nbLignes);
                }

            } while (choix == "1");

            // On sort de la boucle 
            Console.WriteLine("\nLe jeu commence...\n");

            Jeu jeu = new Jeu(joueur1, joueur2, dico, p);
            // Petite pause avant de commencer le jeu (1s)
            Thread.Sleep(1000);
            startingTime = DateTimeOffset.Now.ToUnixTimeSeconds();

            while (DateTimeOffset.Now.ToUnixTimeSeconds() - startingTime <= timeLimit)
            {
                //partie d'un joueur
                Console.Clear();
                Console.WriteLine("==== JEU DE MOTS =====");
                Console.WriteLine(p.ToString());

                Console.WriteLine($"C'est au tour de {jeu.JoueurActuel.Nom} de jouer ! Entre un mot valide ou appui sur échape pour passer ton tour.");

                // on réitère jusqu'à ce que le joueur entre un mot valide ou décide de passer son tour
                string mot = "";

                do
                {
                    mot = "";
                    ConsoleKeyInfo keyPressed;

                    // on continue la boucle tant que le joueur n'a pas appuyé sur entrée ou échape
                    do
                    {
                        while (!Console.KeyAvailable)
                        {
                            TerminalClass.ClearLine();
                            Console.Write($"Temps restant {DateTimeOffset.Now.ToUnixTimeSeconds() - startingTime} s : " + mot);
                            Thread.Sleep(100);
                        }
                        keyPressed = Console.ReadKey(true);
                    } while (keyPressed.Key != ConsoleKey.Enter);
                } while (!jeu.motScored(mot));
            }

            // Affichage des scores finaux
            Console.Clear();
            Console.WriteLine("==== RÉSULTAT ====");
            Console.WriteLine();

            if (joueur1.Score > joueur2.Score)
            {
                Console.WriteLine($" Vainqueur : {joueur1.Nom} avec {joueur1.Score} points !");
            }
            else if (joueur2.Score > joueur1.Score)
            {
                Console.WriteLine($" Vainqueur : {joueur2.Nom} avec {joueur2.Score} points !");
            }
            else
            {
                Console.WriteLine(" Match nul ! Les deux joueurs sont à égalité.");
            }

            Console.WriteLine();
            Console.WriteLine();
            

            // Attend une touche pour fermer la fenêtre console
            Console.WriteLine("Appuyez sur une touche pour quitter...");

            Console.ReadKey();
        }
    }
}
