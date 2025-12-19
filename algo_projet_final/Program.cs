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

        static void Main(string[] args)
        {
            // Mise en place du thème de couleur
            TerminalClass.init();
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            // Titre
            Console.WriteLine("╔════════════════════════════════════════════════════╗");
            Console.WriteLine("║           BIENVENUE DANS LE JEU MOTS GLISSÉS       ║");
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

            Console.ForegroundColor = ConsoleColor.Cyan;
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

            // Choix du mode de plateau
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("==== CHOIX DU MODE DE PLATEAU ====\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("1. Importer un plateau depuis un fichier");
            Console.WriteLine("2. Générer un plateau aléatoire");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Tapez échape' pour quitter.");

            while (true)
            {
                ConsoleKeyInfo mode = Console.ReadKey(true);
                if (mode.Key == ConsoleKey.Escape) return;

                if (mode.KeyChar == '1')
                {
                    Console.Write("Entrez le nom du fichier CSV à importer : ");
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
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Erreur lors de l'import du fichier : " + ex.Message);
                        Console.WriteLine("Appuyez sur une touche pour réessayer...");
                        Console.ReadKey();
                    }
                }
                else if (mode.KeyChar == '2')
                {
                    string cheminLettres = "Lettre.txt";
                    while (true)
                    {
                        Console.Write("Entrez la taille de votre grille (nombre > 1) : ");
                        string saisie = Console.ReadLine();

                        if (int.TryParse(saisie, out nbLignes) && nbLignes > 1)
                        {
                            nbColonnes = nbLignes;
                            break;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Entrée invalide. Veuillez entrer un nombre supérieur à 1.");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                    p = new Plateau(cheminLettres, nbLignes, nbColonnes);
                    break;
                }
            }

            // Régénération du plateau si généré aléatoirement
            if (p != null && nbLignes == nbColonnes)
            {
                ConsoleKeyInfo choix;
                do
                {
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("==== VOTRE GRILLE =====\n");
                    Console.WriteLine(p.ToString());
                    Console.WriteLine("\nAppuyez sur <- pour régénérer la grille, ou Entrée pour continuer.");
                    choix = Console.ReadKey(true);
                    if (choix.Key ==ConsoleKey.Backspace)
                    {
                        p = new Plateau("Lettre.txt", nbLignes, nbColonnes);
                    }
                } while (choix.Key == ConsoleKey.Backspace);
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
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                if (jeu.JoueurActuel == joueur1)
                    Console.ForegroundColor = ConsoleColor.Cyan;
                else
                    Console.ForegroundColor = ConsoleColor.Yellow;

                Console.WriteLine("==== JEU DE MOTS =====");
                Console.ForegroundColor = ConsoleColor.White;
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
                    TerminalClass.ClearLine();
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write("Mot : " + input.ToString() + " ");
                    int left = Console.CursorLeft;
                    int top = Console.CursorTop;

                    // Affiche le chrono du tour en direct
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write($"  Temps restant: {tempsParJoueur - (int)(DateTime.Now - tourStart).TotalSeconds}s, temps total restant {tempsTotal - (int)(DateTime.Now - startGlobal).TotalSeconds}s");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(left, top);

                    // Lecture de touche sans bloquer le timer
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Enter)
                        {
                            mot = input.ToString();
                            break;
                        }
                        else if (key.Key == ConsoleKey.Escape)
                        {
                            mot = "";
                            break;
                        }
                        else if (key.Key == ConsoleKey.Backspace)
                        {
                            if (input.Length > 0)
                            {
                                input.Length--;
                                // Efface le dernier caractère à l'écran
                                Console.SetCursorPosition(5 + input.Length, Console.CursorTop);
                                Console.Write(" ");
                                Console.SetCursorPosition(5 + input.Length, Console.CursorTop);
                            }
                        }
                        else if (char.IsLetter(key.KeyChar) && input.Length < 30)
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

            // FIN DE PARTIE
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("╔═════════════════════════════════╗");
            Console.WriteLine("║          SCORE FINAL            ║");
            Console.WriteLine("╚═════════════════════════════════╝\n");
            joueur1.AfficherInfos();
            joueur2.AfficherInfos();

            Console.WriteLine("\n==== RÉSULTAT ====\n");
            if (joueur1.Score > joueur2.Score)
                Console.WriteLine($"Vainqueur : {joueur1.Nom} avec {joueur1.Score} points !");
            else if (joueur2.Score > joueur1.Score)
                Console.WriteLine($"Vainqueur : {joueur2.Nom} avec {joueur2.Score} points !");
            else
                Console.WriteLine("Match nul ! Les deux joueurs sont à égalité.");



            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\nVoulez-vous sauvegarder la grille pour plus tard ? (o/n) : ");
            string reponse = Console.ReadLine().Trim().ToLower();

            if (reponse == "o" || reponse == "oui")
            {
                Console.Write("Nom du fichier CSV (ex: grille.csv) : ");
                string nomFichier = Console.ReadLine();

                try
                {
                    p.ToFile(nomFichier);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Grille sauvegardée avec succès !");
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Erreur lors de la sauvegarde.");
                }
            }



            Console.ForegroundColor= ConsoleColor.White;
            Console.WriteLine("\nAppuyez sur une touche pour quitter...");
            Console.ReadKey();
        }
    }
}