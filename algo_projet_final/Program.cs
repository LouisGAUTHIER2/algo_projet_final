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


static void Main(string[] args)
    {
        TerminalClass terminal = new TerminalClass();
        Console.Clear();
        Console.WriteLine("==== CONFIGURATION =====");
        Console.WriteLine();

        Console.Write($"{terminal.SetTextColor(255, 255, 0)}{terminal.SetBold()}Bienvenue ! Veuillez entrer vos {terminal.SetUnderline()}noms\n{terminal.ResetEffect()}");

        List<Joueur> joueurs = new List<Joueur>();

        // Entrée des noms des joueurs
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
        } while (nom2.ToLower() == nom1.ToLower());

        Joueur joueur2 = new Joueur(nom2);
        joueurs.Add(joueur2);

        Dictionnaire dico = new Dictionnaire("Mots_Français.txt", "francais");

            // Remplace le chemin par l'endroit où tu as mis Lettres.txt
         string cheminLettres = "Lettre.txt";
         int nbLignes ;
         int nbColonnes ;
         Console.WriteLine("Entrer la taille de votre grille ");
         nbLignes = Convert.ToInt32(Console.ReadLine());
         nbColonnes = nbLignes;

        // Création d'un plateau aléatoire
        Plateau p = new Plateau(cheminLettres, nbLignes, nbColonnes);
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
                p = new Plateau(cheminLettres, nbLignes, nbColonnes);
            }

        } while (choix == "1");

        // On sort de la boucle 
        Console.WriteLine("\nLe jeu commence...\n");

        // Petite pause avant de commencer le jeu (1s)
            Thread.Sleep(1000);

        int joueurI = 0;


            while (true)
            {
                Console.Clear();
                Console.WriteLine("==== JEU =====");
                Console.WriteLine();
                Console.WriteLine("Plateau actuel :");
                Console.WriteLine();
                Console.WriteLine(p.ToString());
                Console.WriteLine($"\nC'est au tour de {joueurs[joueurI].Nom}.");

                Console.Write("Entrez un mot à chercher (ou tapez 'exit' pour quitter) : ");
                string mot = Console.ReadLine();
                if (mot.ToLower() == "exit")
                    break;

                if (mot.Length < 2)
                {
                    Console.WriteLine("Le mot doit faire au moins 2 lettres.");
                }
                else if (joueurs[joueurI].Contient(mot))
                {
                    Console.WriteLine($"Vous avez déjà trouvé le mot \"{mot}\".");
                }
                else
                {
                    var resultat = p.Recherche_Mot(mot);
                    var positionsMot = p.Recherche_Mot(mot);
                   
                        if (resultat != null)
                    {
                        // Vérification dans le dictionnaire
                        if (dico.RechDichoRecursif(mot))
                        {
                            Console.WriteLine($"Le mot \"{mot}\" est présent sur la grille et dans le dictionnaire !");
                            //Faire glisser les mots
                            p.Maj_Plateau(positionsMot);
                            // Ajoute à la base de donnée de mots trouvés
                            joueurs[joueurI].Add_Mot(mot);
                            //Ajoute 1 au score 
                            joueurs[joueurI].Add_Score(1); 
                            Console.WriteLine($"Bravo {joueurs[joueurI].Nom} !");
                            Console.WriteLine("Score de "+ joueurs[joueurI].Nom+ " = " +joueurs[joueurI].Score);
                        }
                        else
                        {
                            Console.WriteLine($"Le mot \"{mot}\" n'est pas dans le dictionnaire français.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Le mot \"{mot}\" n'est PAS présent sur la grille.");
                    }
                }

                // Changer de joueur
                joueurI = (joueurI + 1) % joueurs.Count;

                // Attendre pour que l'utilisateur puisse lire le résultat avant de passer au joueur suivant
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
            }

            //Afficher les scores des deux joueurs 
            Console.Clear();
            Console.WriteLine("==== SCORE ====");
            Console.WriteLine();
            joueur1.AfficherInfos();
            joueur2.AfficherInfos();

            Thread.Sleep(3000);

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
