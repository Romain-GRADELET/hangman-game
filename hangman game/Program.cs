using System;
using System.Collections.Immutable;
using AsciiArt;

namespace hangman_game
{
    class Program
    {
        static void AfficherMot(string mot, List<char> lettres) 
        {

            for (int i = 0; i < mot.Length; i++)
            {
                char lettre = mot[i];
                if (lettres.Contains(lettre))
                {
                    Console.Write(lettre + " ");
                }
                else
                {
                    Console.Write("_ ");
                }
            }
            Console.WriteLine();
        }

        static bool ToutesLettresDevinees(string mot, List<char> lettres) 
        {
            foreach (var lettre in lettres)
            {
                    mot = mot.Replace(lettre.ToString(), "");
            }
            if (mot == "")
            {
                return true;
            }
            return false;   
        }

        static char DemanderUneLettre(string message = "Rentrez une lettre : ")
        {
            while (true) { 
                Console.Write(message);
                string reponse = Console.ReadLine();

                if (reponse.Length == 1)
                {
                    reponse = reponse.ToUpper();
                    return reponse[0];
                }
                Console.WriteLine("ERREUR: Vous devez rentrer une lettre");
            }
        }

        static void DevinerMot(string mot)
        {

            var lettresDevinees = new List<char>();
            var lettresExclues = new List<char>();

            const int NB_VIES = 6;
            int viesRestantes = NB_VIES;

            while (viesRestantes > 0)
            {
                Console.WriteLine(Ascii.PENDU[NB_VIES-viesRestantes]);

                AfficherMot(mot, lettresDevinees);
                Console.WriteLine();

                var lettre = DemanderUneLettre();

                Console.Clear();

                if (mot.Contains(lettre))
                {
                    Console.WriteLine("Cette lettre est dans le mot");
                    lettresDevinees.Add(lettre);
                    
                    if (ToutesLettresDevinees(mot, lettresDevinees) == true)
                    {
                        break;
                    };
                }
                else if (lettresExclues.Contains(lettre))
                {
                    Console.WriteLine("Vous avez déjà essayé cette lettre");
                }
                else 
                {
                    Console.WriteLine("Cette lettre n'est pas dans le mot");

                    lettresExclues.Add(lettre);
                    viesRestantes--;
                    Console.WriteLine("Vies restantes : " + viesRestantes);
                }
                if (lettresExclues.Count != 0) {
                    Console.WriteLine("Le mot ne comprend pas les lettres : " + string.Join(", ", lettresExclues));
                    Console.WriteLine();
                }
            }

            Console.WriteLine(Ascii.PENDU[NB_VIES - viesRestantes]);

            if (viesRestantes == 0)
            {
                Console.WriteLine("PERDU !, le mot était : " + mot);
            }
            else
            {
                AfficherMot(mot, lettresDevinees);
                Console.WriteLine();
                Console.WriteLine("GAGNE !");
            }

            Rejouer();
        }

        static string[] ChargerLesMots(string nomFichier)
        {
            try
            {
                return File.ReadAllLines(nomFichier);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur de lecture du fichier : " + nomFichier + " (" + ex.Message + ")");
            }

            return null;
        }
        
        static bool Rejouer()
        {
            char reponse = DemanderUneLettre("Voulez-vous refaire une partie ? (o/n) : ");

            if ((reponse == 'o') || (reponse == 'O'))
            {
                return true ;
            }
            else if ((reponse == 'n') || (reponse == 'N'))
            {
                return false;
            }
            else
            {
                Console.Write("veuillez saisir : \n" +
                    "o => rejouer \n" +
                    "n => abandonner  ");
                return Rejouer();
            }
        }

        static void Main(string[] args)
        {
            var mots = ChargerLesMots("mots.txt");

            if ((mots == null) || (mots.Length == 0))
            {
                Console.WriteLine("La liste de mot est vide");
            }
            else
            {
                while (true)
                {
                    Random r = new Random();
                    int i = r.Next(mots.Length);

                    string mot = mots[i].Trim().ToUpper();

                    DevinerMot(mot);

                    if (!Rejouer())
                    {
                        break;
                    }
                    Console.Clear();
                }

                Console.WriteLine("Merci et à bientôt");
            }


        }
    }
}