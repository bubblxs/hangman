using System.Reflection;

namespace hangman
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "hangman";

            List<string> wordList = new List<string>();
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream? stream = assembly.GetManifestResourceStream("hangman.words.txt");
            StreamReader reader;

            if (stream == null)
            {
                HttpClient client = new();
                HttpResponseMessage res = await client.GetAsync("https://raw.githubusercontent.com/bubblxs/hangman/main/words.txt");
                Stream content = res.Content.ReadAsStream();
                reader = new(content);
            }
            else
            {
                reader = new(stream);
            }

            string? line = string.Empty;
            while ((line = reader.ReadLine()) != null)
            {
                wordList.Add(line);
            }

            if (wordList.Count == 0)
            {
                throw new Exception("words not found");
            }

            do
            {
                Hangman hangman = new();
                Word word = new(wordList[new Random().Next(0, wordList.Count - 1)]);
                int attempt = 0;
                int maxNumAttempts = hangman.GetNumOfSprites() - 1;
                char[] mask = word.BuildWordMask().ToCharArray();

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine(hangman.GetHangman());
                    Console.WriteLine($"{new string(mask)}");
                    Console.WriteLine($"\n[!] wrong attempts: {attempt}/{maxNumAttempts}\n");

                    if (attempt >= maxNumAttempts)
                    {
                        Console.WriteLine($"[-] you lost. the word was \"{word.GetWord()}\"");
                        break;
                    }

                    if (word.IsEqual(mask))
                    {
                        Console.WriteLine("[-] you won!");
                        break;
                    }

                    Console.Write("[+] try: ");
                    char letter = Console.ReadKey().KeyChar;
                    List<int> indexes = word.GetIndexesOf(letter);

                    if (indexes.Count == 0)
                    {
                        attempt += 1;
                        hangman.UpdateSprite(attempt);
                    }
                    else
                    {
                        indexes.ForEach((l) => mask[l] = letter);
                    }
                }

                Console.Write("[-] would you like to play again? [y/n]: ");
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.Y:
                        Console.WriteLine("\n[-] starting a new game");
                        Thread.Sleep(1500);
                        break;
                    case ConsoleKey.N:
                        Console.WriteLine("\n[-] quitting");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("\n[-] i will take that as a no :3");
                        Environment.Exit(0);
                        break;
                }
            } while (true);
        }
    }
}