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
        static long timeLimit = 120;

        static void Main(string[] args)
        {
            //initialisation du terminal pour les effets ANSI
            TerminalClass.init();

            Console.Clear();
            Console.WriteLine($"{TerminalClass.SetUnderline()}==== CONFIGURATION ====={TerminalClass.ResetEffect()}");
            Console.WriteLine();

            List<Joueur> joueurs = new List<Joueur>();

            // Entrée des noms des joueurs (avec possibilité de quitter)
            Joueur joueur1 = new Joueur();
            Joueur joueur2 = new Joueur();

            Dictionnaire dico = new Dictionnaire("Mots_Français.txt", "francais");

            Plateau p = null;
            int nbLignes = 0, nbColonnes = 0;

            // Choix du mode de création/import du plateau
            Console.Clear();
            Console.WriteLine($"{TerminalClass.SetUnderline()}==== CHOIX DU MODE DE PLATEAU ===={TerminalClass.ResetEffect()}\n");
            Console.WriteLine("1. Importer un plateau depuis un fichier");
            Console.WriteLine("2. Générer un plateau aléatoire");
            Console.WriteLine("Tapez 'échape' pour quitter.");

            while (true)
            {
                ConsoleKeyInfo mode = Console.ReadKey(true);
                if (mode.Key == ConsoleKey.Escape) return;

                if (mode.KeyChar == '1')
                {
                    // Import d'un fichier CSV
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
                        Console.WriteLine($"{TerminalClass.SetTextColor(255,0,0)+TerminalClass.SetBold()}Erreur lors de l'import du fichier. " + ex.Message+TerminalClass.ResetEffect());
                        Console.WriteLine("Appuyez sur une touche pour réessayer...");
                        Console.ReadKey();
                    }
                }
                else if (mode.KeyChar == '2')
                {
                    // Génération aléatoire
                    string cheminLettres = "Lettre.txt";
                    while (true)
                    {
                        Console.Write("Entrez la taille de votre grille : ");
                        string saisie = Console.ReadLine();
                        
                        if (int.TryParse(saisie, out nbLignes) && nbLignes > 1 && nbLignes <= 11)
                        {
                            nbColonnes = nbLignes;
                            break;
                        }
                        else
                        {
                            Console.WriteLine($"{TerminalClass.SetTextColor(255,0,0)+TerminalClass.SetBold()}Entrée invalide. Veuillez entrer un nombre supérieur à 1 et inférieure à 12.{TerminalClass.ResetEffect()}");
                        }
                    }
                    p = new Plateau(cheminLettres, nbLignes, nbColonnes);
                    break;
                }
            }

            // Boucle de régénération du plateau si généré aléatoirement
            if (p != null && nbLignes == nbColonnes) // seulement pour la génération aléatoire
            {
                ConsoleKeyInfo choix;
                do
                {
                    Console.Clear();
                    Console.WriteLine($"{TerminalClass.SetUnderline()}==== VOTRE GRILLE ====={TerminalClass.ResetEffect()}\n");
                    Console.WriteLine(p.ToString());
                    Console.WriteLine("\nAppuyez sur 1 pour régénérer la grille, ou sur Entrer pour continuer le jeu.");
                    choix = Console.ReadKey(true);

                    if (choix.KeyChar == '1')
                    {
                        p = new Plateau("Lettre.txt", nbLignes, nbColonnes);
                    }
                } while (!(choix.Key == ConsoleKey.Enter));
            }

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
                Console.WriteLine($"{TerminalClass.SetUnderline()}==== JEU DE MOTS ====={TerminalClass.ResetEffect()}");
                Console.WriteLine(p.ToString());

                Console.WriteLine($"C'est au tour de {jeu.JoueurActuel.Nom} de jouer ! Entre un mot valide ou appui sur échape pour passer ton tour.");

                // on réitère jusqu'à ce que le joueur entre un mot valide ou décide de passer son tour
                string mot = "";

                do
                {
                    if (mot != "")
                    {
                        TerminalClass.ClearLine();
                        Console.Write($"{TerminalClass.SetTextColor(255, 0, 0) + TerminalClass.SetBold()}Mot invalide{TerminalClass.ResetEffect()}");
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
                            TerminalClass.ClearLine();
                            Console.Write($"Temps restant {timeLimit - (DateTimeOffset.Now.ToUnixTimeSeconds() - startingTime)} s : " + mot);
                            Thread.Sleep(100);
                        }
                        if (timeLimit - (DateTimeOffset.Now.ToUnixTimeSeconds() - startingTime) < 0) break;

                        keyPressed = Console.ReadKey(true);

                        if (keyPressed.Key != ConsoleKey.Enter) mot += keyPressed.KeyChar;
                    } while (keyPressed.Key != ConsoleKey.Enter);
                    if (timeLimit - (DateTimeOffset.Now.ToUnixTimeSeconds() - startingTime) < 0) break;

                } while (!jeu.motScored(mot));
                jeu.ChangePlayer();
            }


            // Affichage des scores finaux
            TerminalClass.ClearTerminal();
            Console.WriteLine($"{TerminalClass.SetUnderline()}==== SCORE FINAL ===={TerminalClass.ResetEffect()}\n");
            joueur1.AfficherInfos();
            joueur2.AfficherInfos();

            Console.WriteLine($"\n{TerminalClass.SetUnderline()}==== RÉSULTAT ===={TerminalClass.ResetEffect()}\n");
            if (joueur1.Score > joueur2.Score)
                Console.WriteLine($"Vainqueur : {joueur1.Nom} avec {joueur1.Score} points !");
            else if (joueur2.Score > joueur1.Score)
                Console.WriteLine($"Vainqueur : {joueur2.Nom} avec {joueur2.Score} points !");
            else
                Console.WriteLine("Match nul ! Les deux joueurs sont à égalité.");

            Console.ReadKey();
        }
    }
}
