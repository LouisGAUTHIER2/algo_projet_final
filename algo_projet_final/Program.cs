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
            Console.WriteLine("==== CONFIGURATION =====\n");

            Console.Write($"{terminal.SetTextColor(255, 255, 0)}{terminal.SetBold()}Bienvenue ! Veuillez entrer vos {terminal.SetUnderline()}noms\n{terminal.ResetEffect()}");

            List<Joueur> joueurs = new List<Joueur>();

            // Entrée des noms des joueurs (avec possibilité de quitter)
            string nom1;
            do
            {
                Console.Write("Entrez le nom du premier joueur (ou 'exit' pour quitter) : ");
                nom1 = Console.ReadLine();
                if (nom1.ToLower() == "exit" || nom1.ToLower() == "quit") return;
            } while (string.IsNullOrWhiteSpace(nom1));
            Joueur joueur1 = new Joueur(nom1);
            joueurs.Add(joueur1);

            string nom2;
            do
            {
                Console.Write("Entrez le nom du second joueur (ou 'exit' pour quitter) : ");
                nom2 = Console.ReadLine();
                if (nom2.ToLower() == "exit" || nom2.ToLower() == "quit") return;
                if (nom2.ToLower() == nom1.ToLower())
                {
                    Console.WriteLine("Ce nom est déjà pris, choisissez-en un autre.");
                }
            } while (string.IsNullOrWhiteSpace(nom2) || nom2.ToLower() == nom1.ToLower());
            Joueur joueur2 = new Joueur(nom2);
            joueurs.Add(joueur2);

            Dictionnaire dico = new Dictionnaire("Mots_Français.txt", "francais");

            Plateau p = null;
            int nbLignes = 0, nbColonnes = 0;

            // Choix du mode de création/import du plateau
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==== CHOIX DU MODE DE PLATEAU ====\n");
                Console.WriteLine("1. Importer un plateau depuis un fichier");
                Console.WriteLine("2. Générer un plateau aléatoire");
                Console.WriteLine("Tapez 'exit' pour quitter.");
                Console.Write("\nVotre choix : ");
                string mode = Console.ReadLine();
                if (mode.ToLower() == "exit" || mode.ToLower() == "quit") return;

                if (mode == "1")
                {
                    // Import d'un fichier CSV
                    Console.Write("Entrez le nom du fichier CSV à importer (ou 'exit' pour quitter) : ");
                    string nomFichier = Console.ReadLine();
                    if (nomFichier.ToLower() == "exit" || nomFichier.ToLower() == "quit") return;
                    try
                    {
                        p = new Plateau(nomFichier);
                        nbLignes = p.NbLignes;
                        nbColonnes = p.NbColonnes;
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erreur lors de l'import du fichier. " + ex.Message);
                        Console.WriteLine("Appuyez sur une touche pour réessayer...");
                        Console.ReadKey();
                    }
                }
                else if (mode == "2")
                {
                    // Génération aléatoire
                    string cheminLettres = "Lettre.txt";
                    while (true)
                    {
                        Console.Write("Entrez la taille de votre grille (un nombre, ou 'exit' pour quitter) : ");
                        string saisie = Console.ReadLine();
                        if (saisie.ToLower() == "exit" || saisie.ToLower() == "quit") return;
                        if (int.TryParse(saisie, out nbLignes) && nbLignes > 1)
                        {
                            nbColonnes = nbLignes;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Entrée invalide. Veuillez entrer un nombre supérieur à 1.");
                        }
                    }
                    p = new Plateau(cheminLettres, nbLignes, nbColonnes);
                    break;
                }
                else
                {
                    Console.WriteLine("Choix invalide. Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                }
            }

            // Boucle de régénération du plateau si généré aléatoirement
            if (p != null && nbLignes == nbColonnes) // seulement pour la génération aléatoire
            {
                string choix;
                do
                {
                    Console.Clear();
                    Console.WriteLine("==== VOTRE GRILLE =====\n");
                    Console.WriteLine(p.ToString());
                    Console.WriteLine("\nAppuyez sur 1 pour régénérer la grille, ou sur Entrer pour continuer le jeu. Tapez 'exit' pour quitter.");
                    choix = Console.ReadLine();
                    if (choix.ToLower() == "exit" || choix.ToLower() == "quit") return;
                    if (choix == "1")
                    {
                        p = new Plateau("Lettre.txt", nbLignes, nbColonnes);
                    }
                } while (choix == "1");
            }

            Console.WriteLine("\nLe jeu commence...\n");
            Thread.Sleep(1000);

            int joueurI = 0;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==== JEU =====\n");
                Console.WriteLine("Plateau actuel :\n");
                Console.WriteLine(p.ToString());
                Console.WriteLine($"\nC'est au tour de {joueurs[joueurI].Nom}.");

                Console.Write("Entrez un mot à chercher (ou tapez 'exit' pour quitter) : ");
                string mot = Console.ReadLine();
                if (mot.ToLower() == "exit" || mot.ToLower() == "quit")
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
                    var positionsMot = resultat; // ou adapte selon ton code
                    if (resultat != null)
                    {
                        if (dico.RechDichoRecursif(mot))
                        {
                            Console.WriteLine($"Le mot \"{mot}\" est présent sur la grille et dans le dictionnaire !");
                            p.Maj_Plateau(positionsMot);
                            joueurs[joueurI].Add_Mot(mot);
                            joueurs[joueurI].Add_Score(1); // ou ta règle de score
                            Console.WriteLine($"Bravo {joueurs[joueurI].Nom} !");
                            Console.WriteLine("Score de " + joueurs[joueurI].Nom + " = " + joueurs[joueurI].Score);
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

                joueurI = (joueurI + 1) % joueurs.Count;
                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
            }

            // Affichage des scores finaux
            Console.Clear();
            Console.WriteLine("==== SCORE FINAL ====\n");
            foreach (var j in joueurs)
                j.AfficherInfos();

            Console.WriteLine("\n==== RÉSULTAT ====\n");
            if (joueur1.Score > joueur2.Score)
                Console.WriteLine($"Vainqueur : {joueur1.Nom} avec {joueur1.Score} points !");
            else if (joueur2.Score > joueur1.Score)
                Console.WriteLine($"Vainqueur : {joueur2.Nom} avec {joueur2.Score} points !");
            else
                Console.WriteLine("Match nul ! Les deux joueurs sont à égalité.");

            Console.WriteLine("\nAppuyez sur une touche pour quitter...");
            Console.ReadKey();
        }
    }
}
