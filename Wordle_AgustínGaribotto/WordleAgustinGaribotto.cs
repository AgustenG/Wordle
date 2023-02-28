/*
* AUTHOR: Agustín Garibotto Villanueva
* DATE: 23/02/2023
* DESCRIPTION: PROJECTE WORDLE UF3
*/
using System;
using System.IO;
using System.Collections.Generic;

namespace Wordle
{
    public class WordleAgustinGaribotto
    {

        public static void Main()
        {
            Directory.SetCurrentDirectory(@"..\..\..\..\Diccionaris");
            Console.SetWindowSize(130, 30); // Para que se vea bien el titulo
            Menu();
        }
        /// <summary>
        /// Crea un array de 4 llistes, les modifica y les retorna.
        /// </summary>
        /// <returns></returns>
        public static List<string>[] CrearLlistes()
        {
            List<string> easyCat = new List<string>(), hardCat = new List<string>(), easyEs = new List<string>(), hardEs = new List<string>();
            List<string>[] llistesCat = ModifyList(easyCat, hardCat, false);
            List<string>[] llistesEs = ModifyList(easyEs, hardEs, true);
            List<string>[] llistes = { llistesCat[0], llistesCat[1], llistesEs[0], llistesEs[1] };
            return llistes;
        }
        /// <summary>
        /// Fent servir dues llistes donades i el idioma,
        /// listEasy, listHard i idioma, afegeix a la llista
        /// easy les paraules que no tinguin ÑÇ ni accents, i 
        /// a hard la resta.
        /// </summary>
        /// <param name="listEasy"></param>
        /// <param name="listHard"></param>
        /// <param name="idioma"></param>
        /// <returns></returns>
        public static List<string>[] ModifyList(List<string> listEasy, List<string> listHard, bool idioma)
        {
            List<string>[] llistesMod = new List<string>[2];
            string[] files = { "diccionariCat.txt", "diccionariEs.txt" };
            string line;
            if (!idioma)
            {
                StreamReader sr = new StreamReader(files[0]); // Llegeix el fitxer de l'idioma seleccionat
                while ((line = sr.ReadLine()) != null) // Leemos todas las palabras del diccionario.
                {
                    if (NoTeAccentÑÇ(line)) listEasy.Add(line.ToUpper());  //Si no tienen acentos ni Ñ ni Ç, la añadimos a la lista easy
                    else listHard.Add(line.ToUpper()); // Si tienen acentos, Ñ o Ç,, la añadimos a la lista hard
                }
            }
            else
            {
                StreamReader sr = new StreamReader(files[1]); // Llegeix el fitxer de l'idioma seleccionat
                while ((line = sr.ReadLine()) != null) // Leemos todas las palabras del diccionario.
                {
                    if (NoTeAccentÑÇ(line)) listEasy.Add(line.ToUpper());  //Si no tienen acentos ni Ñ ni Ç, la añadimos a la lista easy
                    else listHard.Add(line.ToUpper()); // Si tienen acentos, Ñ o Ç,, la añadimos a la lista hard
                }
            }

            llistesMod[0] = listEasy;
            llistesMod[1] = listHard;
            return llistesMod;
        }
        /// <summary>
        /// Eligeix la llista depenent de l'idioma i la dificultat i la retorna
        /// </summary>
        /// <param name="idioma"></param>
        /// <param name="dificultat"></param>
        /// <param name="llistes"></param>
        /// <returns></returns>
        public static List<string> GetList(string idioma, bool dificultat, List<string>[] llistes)
        {
            List<string> llista = new List<string>();
            if (idioma == "1" && !dificultat) llista = llistes[0]; // llista cat fàcil
            else if (idioma == "1" && dificultat) llista = llistes[1]; // llista cat difícil
            else if (idioma == "2" && !dificultat) llista = llistes[2]; // llista es fàcil
            else if (idioma == "2" && dificultat) llista = llistes[3]; // llsita es difícil
            return llista;
        }

        /// <summary>
        /// Comprova cada caracter d'un string s donat, si es menor a 128 vol dir que no té ni acents ni ç ni ñ
        /// No filtra caràcters especials ja que el diccionari només conté paraules.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool NoTeAccentÑÇ(string s)
        {
            bool ascii = false;
            foreach (char c in s)
            {
                if (c < 128) ascii = true;
                else return false;
            }
            return ascii;
        }
        /// <summary>
        /// Donada la llista corresponent a la partida retorna una paraula aleatòria dins d'aquesta llista
        /// </summary>
        /// <param name="llista"></param>
        /// <returns></returns>
        public static string WordGenerator(List<string> llista)
        {
            int num;
            string wordToGuess;
            Random rnd = new Random();
            num = rnd.Next(0, llista.Count);
            wordToGuess = llista[num];
            return wordToGuess;
        }
        /// <summary>
        /// Donat el idioma de la partida demana la dificultat, i retorna aquesta en forma de bool, false=fàcil, true=difícil
        /// </summary>
        /// <param name="idioma"></param>
        /// <returns></returns>
        public static bool DemanarDificultat(string idioma)
        {
            if (idioma == "1") Console.WriteLine("Introdueix la dificultat que vulguis \n1.Fàcil \n2.Difícil(accents i Ç)");
            else Console.WriteLine("Introduce la dificultad que quieras \n1.Fácil \n2.Difícil(acentos y Ñ)");
            string opcio;
            bool dificultat = false; // False fàcil, True difícil
            do
            {
                opcio = Console.ReadLine();
                if (opcio == "2") dificultat = !dificultat;
                else if (opcio == "1") dificultat = false;
                else Console.WriteLine(ErrorMessage(idioma));
            } while (opcio != "1" && opcio != "2");
            return dificultat;
        }
        /// <summary>
        /// Donada la paraula que introdueix el usuari i la paraula que hi ha que endevinar 
        /// mostra per pantalla cada lletra del string usrWord amb un color de fons que depèn
        /// de si està en la posició correcta(verd), posició incorrecta(groc), no està(gris)
        /// respecte a la paraula a endevinar.
        /// </summary>
        /// <param name="usrWord"></param>
        /// <param name="wordToGuess"></param>
        public static int[] WordFeedback(string usrWord, string wordToGuess)
        {
            int[] wordReturn = { 0, 0, 0, 0, 0 }; // 0 per les lletres que no es troben a wordToGuess
            string nonGuessedLetters = wordToGuess;
            string guessedWrongPos = wordToGuess;
            for (int i = 0; i < usrWord.Length; i++) // Hem de posar aquest for fora, ja que com el programa comprova de dreta a esquerra, llavors si ho posem dins no valorarà els casos on la lletra repetida sigui al principi i sigui acertada al final
            {
                if (usrWord[i] == wordToGuess[i]) nonGuessedLetters = nonGuessedLetters.Remove(nonGuessedLetters.IndexOf(usrWord[i]), 1);
            }
            // Fem un recorregut de les lletres encertades de la paraula que ha introduit el usuari i les quitem del string nonGuessedLetters.
            for (int i = 0; i < usrWord.Length; i++)
            {
                ConsoleColor color = ConsoleColor.DarkGray;
                if (usrWord[i] == wordToGuess[i])
                {
                    wordReturn[i] = 2; // 2 per les lletres correctes
                    color = ConsoleColor.DarkGreen;
                } // Ha de complir que la lletra estigui a la mateixa posicio
                else if (wordToGuess.Contains(usrWord[i]) && nonGuessedLetters.Contains(usrWord[i]) && guessedWrongPos.Contains(usrWord[i]))
                {
                    wordReturn[i] = 1; // 1 per les lletres en posició incorrecta
                    color = ConsoleColor.DarkYellow;
                    guessedWrongPos = guessedWrongPos.Remove(guessedWrongPos.IndexOf(usrWord[i]), 1);
                    //Treiem del string de lletres en posicio incorrecta la lletra, així en cas d'haver només 1 a la paraula wordToGuess 
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = color;
                Console.Write(" " + usrWord[i] + " "); //Escrivim la lletra amb el color corresponent
                Console.ResetColor();
                Console.Write(" ");
            }
            return wordReturn;
        }
        //Ha de complir que la lletra estigui a la paraula i !IMPORTANT que no hagi sigut encertada, perquè sino les paraules amb lletres 
        //repetides que introdueixi l'usuari sortiran amb un output erroni. Exemple si no fesim servir aquesta condició:
        //wordToGuess = anell
        //usrWord = acora -> el output donarà #(🇷)red(🇬)green(🇾)yellow->   "a"(🇬) "c"(🇷) "o"(🇷) "r"(🇷) "a"(🇾) aquest output sería incorrecte,
        //el correcte sería el següent->                                  "a"(🇬)" c"(🇷) "o"(🇷) "r"(🇷) *"a"(🇷) ja que la "a" ya ha sigut encertada i no hi ha més.
        /// <summary>
        /// Funció que retorna un array de strings amb la informació de la partida
        /// desprès d'haver sigut jugada. Demana una paraula a l'usuari,  i en cas de ser la paraula
        /// a esdevinar o que no quedin intents, deixa de demanar paraules i mostra per pantalla el resultat final
        /// finalment retorna si ha guayat o perdut la partida, els intents que li quedàven i el idioma en que s'ha jugat
        /// </summary>
        /// <param name="idioma"></param>
        /// <param name="wordToGuess"></param>
        /// <param name="diff"></param>
        /// <returns></returns>
        public static string[] PlayGame(string idioma, string wordToGuess, bool diff)
        {
            string usrWord;
            int intents = 6;
            if (diff) intents = 10; // he posat 10 intents en dificil perquè és molt difícil 
            bool isWin = false;

            while (intents > 0 && !isWin)
            {
                FeedbackIntents(idioma, intents);
                usrWord = AskUserWord(idioma);
                if (usrWord == wordToGuess) isWin = true;
                else
                {
                    WordFeedback(usrWord, wordToGuess);
                    Console.WriteLine();// Feedback a l'usuari
                    intents--;
                }
            }
            Console.ResetColor();
            string isWinText = ShowLostVictory(idioma, isWin, wordToGuess);
            ClearConsole(idioma);
            string[] dades = { isWinText, Convert.ToString(intents) };
            return dades;
        }
        /// <summary>
        /// Demana una paraula per consola al usuari, en cas de no tenir una longitud = 5 caràcters
        /// la torna a demanar.
        /// Finalment en cas de introduir una paraula de 5 lletres la retorna per consola.
        /// </summary>
        /// <param name="idioma"></param>
        /// <returns></returns>
        public static string AskUserWord(string idioma)
        {
            string usrWord = Console.ReadLine().ToUpper();
            // Vaig pensar en afegir una condició perquè l'usuari nomès poguès introduir paraules que existeixen, però això ho fa molt més difícil
            while (usrWord.Length != 5)
            {
                if (idioma == "1") Console.WriteLine("La paraula ha de ser d'exactament 5 lletres");
                else Console.WriteLine("La palabra tiene que tener 5 letras ");
                usrWord = Console.ReadLine().ToUpper();
            }
            return usrWord;
        }
        /// <summary>
        /// Mostra per pantalla cuants intents queden 
        /// </summary>
        /// <param name="idioma"></param>
        /// <param name="intents"></param>
        public static void FeedbackIntents(string idioma, int intents)
        {
            if (idioma == "1") Console.Write($"\nIntents restantes {intents}:\nIntrodueix la teva paraula: ");
            else Console.Write($"\nIntentos restantes {intents}:\nIntroduce tu palabra: ");
        }
        /// <summary>
        /// Retorna la un string depenent de l'idioma i si ha guanyat o no l'usuari.
        /// </summary>
        /// <param name="idioma"></param>
        /// <param name="isWin"></param>
        /// <param name="wordToGuess"></param>
        /// <returns></returns>
        public static string ShowLostVictory(string idioma, bool isWin, string wordToGuess)
        {
            ConsoleColor color = ConsoleColor.DarkRed;
            string frase;
            if (idioma == "1" && isWin) frase = "VICTÒRIA";
            else if (idioma == "2" && isWin) frase = "VICTORIA";
            else frase = "DERROTA";

            if (isWin) color = ConsoleColor.DarkGreen;
            Console.WriteLine("");
            foreach (char c in frase)
            {
                Console.BackgroundColor = color;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(" " + c + " ");
                Console.ResetColor();
            }
            Console.WriteLine("");
            Console.WriteLine("");
            WordFeedback(wordToGuess, wordToGuess);
            return frase;
        }
        /// <summary>
        /// Mostra l'historial de partides per pantalla fent servir un fitxer txt
        /// </summary>
        public static void ShowGames()
        {
            string readText = File.ReadAllText("gameHistory.txt");
            Console.WriteLine(readText);
        }
        /// <summary>
        /// donat el array de strings de la partida introdueix al fitxer gameHistory una nova línia amb
        /// les dades del jugador les quals es troven a l'array.
        /// </summary>
        /// <param name="partida"></param>
        public static void SaveGame(string[] partida)
        {
            string idioma;
            string path = "gameHistory.txt";
            StreamWriter sw = File.AppendText(path);
            if (partida[4] == "1") idioma = "Català";
            else idioma = "Español";
            sw.WriteLine($"·Name:{partida[0]} / Word:{partida[1]},{partida[2]} / Tries left:{partida[3]} / Lang: {idioma} / Mode: {partida[5]}");
            sw.Close();
        }
        /// <summary>
        /// Rep totes les llistes de les paraules.
        /// Demana el nom i l'afegeix a l'array.
        /// Demana la dificultat i l'afegeix a l'array.
        /// Mostra les Regles per pantalla.
        /// Genera la paraula aleatòria i l'afegeix a l'array.
        /// Inicia la partida y aquesta retorna si ha guanyat, els intents i l'idioma i l'afegeix a l'array.
        /// retorna l'array.
        /// </summary>
        /// <param name="idioma"></param>
        /// <param name="llistes"></param>
        /// <returns></returns>
        public static string[] DadesPartida(string idioma, List<string>[] llistes, string usrName, bool dificultat) // Dades de la partida a l'inici i al final
        {
            string wordToGuess, winOrLose, intents;
            Console.WriteLine(Regles(idioma));
            wordToGuess = WordGenerator(GetList(idioma, dificultat, llistes)); // WordToGuess
            string[] dades = PlayGame(idioma, wordToGuess, dificultat); //  Dades de la partida al finalitzar
            winOrLose = dades[0]; // WinOrLose
            intents = dades[1]; // Intents
            string[] partida = { usrName, wordToGuess, winOrLose, intents, idioma, dificultat.ToString() };
            return partida;
        }
        /// <summary>
        /// Demana el seu nom a l'usuari en l'idioma seleccionat i retorna aquest.
        /// </summary>
        /// <param name="idioma"></param>
        /// <returns></returns>
        public static string AskUserName(string idioma)
        {
            if (idioma == "1") Console.WriteLine("Introdueix el teu nom: ");
            else Console.WriteLine("Introduce tu nombre: ");
            return Console.ReadLine();
        }
        /// <summary>
        /// Mostra per pantalla el títol i demana opció per jugar fins que l'usuari premi 0 per sortir del programa.
        /// </summary>
        public static void Menu()
        {
            string idioma;
            string[] dades;
            List<string>[] llistes = CrearLlistes(); //easyCat, hardCat, easyEs, hardEs
            do
            {
                ShowTitle();
                Console.WriteLine();
                idioma = Console.ReadLine();
                switch (idioma)
                {
                    case "1":
                    case "2":
                        dades = DadesPartida(idioma, llistes, AskUserName(idioma), DemanarDificultat(idioma));
                        SaveGame(dades);
                        break;
                    case "3":
                        ShowGames();
                        ReadKey();
                        break;
                    case "0":
                        break;
                    default:
                        idioma = "1";
                        Console.WriteLine(ErrorMessage(idioma));
                        ReadKey();
                        break;
                }
            } while (idioma != "0");
        }
        /// <summary>
        /// Printa per pantalla el títol ASCII ART amb colors aleatoris a cada caracter i 
        /// printa també el menú.
        /// </summary>
        public static void ShowTitle()
        {
            Random rnd = new Random();
            int num;

            ConsoleColor[] colors = { ConsoleColor.Cyan, ConsoleColor.Cyan, ConsoleColor.Magenta, ConsoleColor.Blue, ConsoleColor.DarkRed };
            string readText = File.ReadAllText("wordle.txt");
            foreach (char c in readText)
            {
                num = rnd.Next(0, colors.Length);
                Console.ForegroundColor = colors[num];
                Console.Write(c);
                Console.ResetColor();
            }
            string readText2 = File.ReadAllText("title.txt");
            Console.WriteLine();
            Console.WriteLine(readText2);

        }
        /// <summary>
        /// Mostra per pantalla un missatge depenent de l'idioma i demana polsar qualsevol tecla per netejar la consola.
        /// </summary>
        /// <param name="idioma"></param>
        public static void ClearConsole(string idioma)
        {
            if (idioma == "1") Console.WriteLine("\nPrem cualsevol teca per tornar al menú");
            else Console.WriteLine("\nPulsa cualquier tecla para volver al menú");
            Console.ReadKey();
            Console.Clear();
        }
        /// <summary>
        /// Demana pulsar una tecla i fa un clear a la consola.
        /// </summary>
        public static void ReadKey()
        {
            Console.WriteLine("\nPulsa cualquier tecla para volver al menú: ");
            Console.ReadKey();
            Console.Clear();
        }
        /// <summary>
        /// Retorna un string amb les regles segons l'idioma
        /// </summary>
        /// <param name="idioma"></param>
        /// <returns></returns>
        public static string Regles(string idioma)
        {
            string regles;
            if (idioma == "1") regles = "Intenta esdevinar la paraula de 5 lletres";
            else regles = "Intenta adivinar la palabra de 5 letras";
            return regles;
        }
        /// <summary>
        /// Donat un idioma 1 o 2 retorna un string amb un missatge.
        /// </summary>
        /// <param name="idioma"></param>
        /// <returns></returns>
        public static string ErrorMessage(string idioma)
        {
            string message;
            if (idioma == "1") message = "Opció incorrecta";
            else message = "Opción incorrecta";
            return message;
        }
    }
}
