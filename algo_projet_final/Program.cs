using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Text;

namespace algo_projet_final
{
    internal class Program
    {
        // Attributs globaux
        static Jeu jeu;
        static long startingTime;
        static long timeLimit = 60;

        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("==== CONFIGURATION =====");
            Console.WriteLine();

            // Titre
            Console.WriteLine("╔════════════════════════════════════════════════════╗");
            Console.WriteLine("║           BIENVENUE DANS LE JEU MOTS GLISSÉS      ║");
            Console.WriteLine("╚════════════════════════════════════════════════════╝\n");

            // Saisie et validation des noms de joueurs
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Entrez le nom du joueur 1 : ");
            string nomJ1 = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(nomJ1))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Le nom ne peut pas être vide, réessayez : ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                nomJ1 = Console.ReadLine();
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Entrez le nom du joueur 2 : ");
            string nomJ2 = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(nomJ2))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Le nom ne peut pas être vide, réessayez : ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                nomJ2 = Console.ReadLine();
            }

            Joueur joueur1 = new Joueur(nomJ1);
            Joueur joueur2 = new Joueur(nomJ2);

            // Chargement du dictionnaire
            Console.ForegroundColor = ConsoleColor.Green;
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
                else if (mode.KeyChar == '2')
                {
                    string cheminLettres = "Lettre.txt";
                    while (true)
                    {
                        Console.Write("Entrez la taille de votre grille (un nombre, ou 'exit' pour quitter) : ");
                        string saisie = Console.ReadLine();
                        
                        if (int.TryParse(saisie, out nbLignes) && nbLignes > 1 && nbLignes <= 11)
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

            // Régénération du plateau si généré aléatoirement
            if (p != null && nbLignes == nbColonnes)
            {
                ConsoleKeyInfo choix;
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
                } while (!(choix.Key == ConsoleKey.Enter));
            }

            // DEMANDE DU TEMPS TOTAL ET TEMPS PAR JOUEUR
            int tempsTotal = 0, tempsParJoueur = 0;
            Console.ForegroundColor = ConsoleColor.Cyan;
            while (tempsTotal <= 0)
            {
                Console.Write("Entrez la durée totale de la partie (en secondes, ex: 180) : ");
                int.TryParse(Console.ReadLine(), out tempsTotal);
                if (tempsTotal <= 0) Console.WriteLine("Entrée invalide.");
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            while (tempsParJoueur <= 0 || tempsParJoueur > tempsTotal)
            {
                Console.Write("Entrez le temps maximum par joueur à chaque tour (en secondes) : ");
                int.TryParse(Console.ReadLine(), out tempsParJoueur);
                if (tempsParJoueur <= 0 || tempsParJoueur > tempsTotal)
                    Console.WriteLine("Entrée invalide.");
            }

            // Création du jeu
            jeu = new Jeu(joueur1, joueur2, dico, p);

            // Démarrage du chrono global
            DateTime startGlobal = DateTime.Now;


            // Boucle principale du jeu
            while ((DateTime.Now - startGlobal).TotalSeconds < tempsTotal )
            {
                // Gestion du chrono par joueur
                DateTime tourStart = DateTime.Now;
                int tempsRestant = tempsParJoueur;
                bool motValide = false;
                string mot = "";

                // Affichage plateau et infos
                Console.Clear();
                Console.WriteLine("==== JEU DE MOTS =====");
                Console.WriteLine(p.ToString());
                Console.WriteLine($"\nTour de {jeu.JoueurActuel.Nom} ({jeu.JoueurActuel.MotsTrouves.Count} mots trouvés, {jeu.JoueurActuel.Score} points)");
                Console.WriteLine($"Temps restant pour ce tour : {tempsRestant} s");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Tapez votre mot (Entrée pour valider, Echap pour passer)");

                // Saisie du mot avec gestion des corrections (Backspace)
                StringBuilder input = new StringBuilder();
                Console.ForegroundColor = ConsoleColor.White;

                while ((DateTime.Now - tourStart).TotalSeconds < tempsParJoueur)
                {
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write("Mot : " + input.ToString() + " ");
                    int left = Console.CursorLeft;
                    int top = Console.CursorTop;

                    // Affiche le chrono du tour en direct
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write($"  Temps restant: {tempsParJoueur - (int)(DateTime.Now - tourStart).TotalSeconds}s   ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(left, top);

                do
                {
                    if (mot != "")
                    {
                        TerminalClass.ClearLine();
                        Console.Write("Mot invalide");
                        Thread.Sleep(1000);
                        TerminalClass.ClearLine();
                    }

                    mot = "";
                    ConsoleKeyInfo keyPressed;

                    // on continue la boucle tant que le joueur n'a pas appuyé sur entrée ou échape
                    do
                    {
                        while (!Console.KeyAvailable && timeLimit - (DateTimeOffset.Now.ToUnixTimeSeconds() - startingTime) >= 0)
                        {
                            input.Append(char.ToLower(key.KeyChar));
                        }
                    }
                    Thread.Sleep(50);
                }

                // Si temps écoulé sans validation, on passe au joueur suivant
                if (string.IsNullOrWhiteSpace(mot))
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("\nVous avez passé votre tour !");
                    Thread.Sleep(1000);
                    jeu.ChangePlayer();
                    continue;
                }

                // Validation du mot (selon ta logique de jeu)
                if (!jeu.motScored(mot))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nMot invalide ou déjà trouvé !");
                    Thread.Sleep(1400);
                    continue; // le joueur rejoue tant qu'il ne donne pas un mot valide ou passe
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nMot validé !");
                    Console.WriteLine("Score de " + jeu.JoueurActuel.Nom + " = " + jeu.JoueurActuel.Score);
                    Thread.Sleep(1300);
                    jeu.ChangePlayer();
                }
            }

            // Affichage des scores finaux
            TerminalClass.ClearTerminal();
            Console.WriteLine("==== SCORE FINAL ====\n");
            joueur1.AfficherInfos();
            joueur2.AfficherInfos();

            Console.WriteLine($"\n{TerminalClass.SetUnderline()}==== RÉSULTAT ===={TerminalClass.ResetEffect()}\n");
            if (joueur1.Score > joueur2.Score)
                Console.WriteLine($"Vainqueur : {joueur1.Nom} avec {joueur1.Score} points !");
            else if (joueur2.Score > joueur1.Score)
                Console.WriteLine($"Vainqueur : {joueur2.Nom} avec {joueur2.Score} points !");
            else
                Console.WriteLine("Match nul ! Les deux joueurs sont à égalité.");

            Console.ResetColor();
            Console.WriteLine("\nAppuyez sur une touche pour quitter...");
            Console.ReadKey();
        }
    }
}