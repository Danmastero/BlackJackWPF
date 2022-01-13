using PokemonAPI.ASCII;
using PokemonAPI.DeckHandler;
using PokemonAPI.JsonHandler;
using Spectre.Console;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace PokemonAPI
{
    internal class BlackJack
    {
        public const int MAX_SCORE = 21;
        private const int CardImageStartPosition = 6;
        private const int CardImageEndPosition = 60;
        public static int _points = 0;
        public static int _coins = 1000;
        public static int _roundNumber = 0;

        public static int placeBetStartPosition = 0;
        private static int placeBetEndPosition = 3;

        public static int takeCardStartPositionY = 0;
        //private static int takeCardEndPositionY = ;
        public static int takeCardStartPositionX = 0;
        private static int takeCardEndPositionX = 6;

        public static void PlayBlackJack() => GameLoop();

        private static void GameLoop()
        {
            Console.WriteLine(AsciiCards.CasinoLogo);
            Console.WriteLine(AsciiCards.CasinoCards);

            var enter = EnterCasino();

            if (enter == "WCHODZĘ")
            {
                ClearOnsoleInGivenRange(0, 100);

                Console.SetCursorPosition(0, 0);
                Console.WriteLine(AsciiCards.Croupier);


                ShuffleSimulation();


                var deck = DeckManager.ShuffleDeck();

                while (_coins > 0)
                {
                    ClearOnsoleInGivenRange(0, 100);

                    Console.SetCursorPosition(0, 0);

                    int bet = PlaceBet();

                    while (_points <= MAX_SCORE)
                    {

                        _roundNumber++;
                        string selectedOption = PlayOrPassOptions();
                        var cardList = DeckManager.DrawACardFromADeck(deck.deck_id);
                        Card card = cardList.cards[0];
                        AsciiMapper asciiMapper = PrintCardAndRefreshThisObject(card);
                        asciiMapper.MapCardToStruct();
                        if (selectedOption == "DOBIERAM")
                        {


                            _points += card.value switch
                            {
                                "KING" => 10,
                                "QUEEN" => 10,
                                "JACK" => 10,
                                "ACE" => 11,
                                _ => Convert.ToInt32(card.value),
                            };
                            AnsiConsole.WriteLine($"");
                            WritePointBar();
                            if (_points == MAX_SCORE)
                            {
                                AnsiConsole.WriteLine("BLACKJACK WYGRAŁEŚ!! ");
                                MultipleCoins(bet);
                                _points = 0;
                                _roundNumber = 0;
                                Thread.Sleep(3000);

                                break;
                            }
                            if (_points > MAX_SCORE)
                            {

                                AnsiConsole.WriteLine($"Niestety przegrałeś, tracisz {bet} żetonów");
                                ReduceCoins(bet);
                                _roundNumber = 0;
                                _points = 0;
                                Thread.Sleep(3000);

                                break;
                            }
                        }
                        else
                        {
                            AnsiConsole.WriteLine("Zakończyłeś turę");
                            GameResultLogic(bet);
                            _points = 0;
                            _roundNumber = 0;

                            Thread.Sleep(3000);
                            break;
                        }
                    }

                }
            }
            //AnsiConsole.WriteLine("Skończyły Ci się żetony, czy chcesz pożyczyć 100 monet i zagrać ponownie?");
        }

        private static string EnterCasino()
        {




            return AnsiConsole.Prompt(
                                                    new SelectionPrompt<string>()
                                                        .Title("CZY [green]WCHODZISZ DO KASYNA[/]?")
                                                        .PageSize(10)
                                                        .AddChoices(new[] {
                                                                "WCHODZĘ","WYMIĘKAM"
                                                        }));
        }


        private static void ShuffleSimulation()
        {
            AnsiConsole.Status()
                .Start("POBIERANIE KART Z API ...", ctx =>
                {
                    AnsiConsole.MarkupLine("Karty są tasowane");
                    Thread.Sleep(5000);
                    ctx.Status("Krupier tasuje ");
                    ctx.Spinner(Spinner.Known.Star);
                    ctx.SpinnerStyle(Style.Parse("green"));
                    AnsiConsole.MarkupLine("Rozdanie");
                    Thread.Sleep(5000);
                });
        }

        private static AsciiMapper PrintCardAndRefreshThisObject(Card card)
        {
            for (int i = CardImageStartPosition; i < CardImageEndPosition; i++)
            {
                Console.SetCursorPosition(0, i - 1);
                ClearCurrentConsoleLine();
            }

            AsciiMapper asciiMapper = new AsciiMapper(card);
            var image = asciiMapper.MapCardToStruct();
            Console.SetCursorPosition(100, CardImageStartPosition);
            AnsiConsole.Write(image);
            return asciiMapper;
        }

        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        private static void WritePointBar()
        {

            AnsiConsole.Write(new BarChart()
                .CenterLabel()
                .Width(60)
                .Label("[green bold underline]Liczba punktów[/]")
                .CenterLabel()
                .AddItem($"RUNDA {_roundNumber}", _points, Color.Blue3)
                );
        }

        private static string PlayOrPassOptions()
        {
            ClearOnsoleInGivenRange(takeCardStartPositionX, takeCardEndPositionX);
            Console.SetCursorPosition(takeCardStartPositionX, takeCardStartPositionY);

            return AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Czy [green]dobierasz kartę[/]?").PageSize(10).AddChoices(new[] { "DOBIERAM", "PAS" }));
        }

        public static void GameResultLogic(int bet)
        {

            var krupierScore = KrupierScore();
            if (krupierScore <= 21)
            {
                if (_points > krupierScore)
                {
                    MultipleCoins(bet);
                }
                else ReduceCoins(bet);
            }

            else if (_points == krupierScore)
            {
                _coins = _coins;
            }

            else
                MultipleCoins(bet);

        }

        public static void MultipleCoins(int bet)
        {
            AnsiConsole.WriteLine($"Dodaje {bet} żetonów");

            _coins += bet;
        }

        public static void ReduceCoins(int bet)
        {
            _coins -= bet;
            AnsiConsole.WriteLine($"Odejmuje {bet} żetonów");

            if (_coins <= 0)
            {
                AnsiConsole.WriteLine("Przegrałeś wszystkie żetony :(");
                Console.WriteLine(AsciiCards.Pepe);

            }
        }



        public static int PlaceBet()
        {
            Console.SetCursorPosition(100, 0);
            ClearOnsoleInGivenRange(placeBetStartPosition, placeBetEndPosition);
            int bet = 0;
            bool isBetValid = false;
            while (isBetValid is false)
            {
                Console.SetCursorPosition(100, 0);

                Console.Write($"Jaki zakład stawiasz? Posiadasz aktualnie {_coins}");
                Console.SetCursorPosition(100, 1);

                string userInput = Console.ReadLine();
                Console.SetCursorPosition(100, 2);

                if (int.TryParse(userInput, out bet))
                {
                    bet = Convert.ToInt32(userInput);
                    if (bet <= _coins)
                    {
                        Console.SetCursorPosition(100, 3);
                        AnsiConsole.Write($"Obstawiłeś {bet}");
                        isBetValid = true;
                    }
                    else
                    {
                        Console.SetCursorPosition(100, 3);
                        AnsiConsole.Write($"Nie masz tylu żetonów!");
                        isBetValid = false;
                    }
                }
            }

            Console.SetCursorPosition(0, 10);

            Console.WriteLine(AsciiCards.Croupier);

            AnsiConsole.Status()
                .Start("Krupier patrzy na żetony", ctx =>
                {
                    AnsiConsole.MarkupLine("Krupier przyjmuje zakład");
                    Thread.Sleep(5000);
                    ctx.Status("Krupier tasuje ");
                    ctx.Spinner(Spinner.Known.Star);
                    ctx.SpinnerStyle(Style.Parse("green"));
                    
                });


            return bet;
        }

        public static void ClearOnsoleInGivenRange(int startPosition, int endPosition)
        {
            for (int i = startPosition; i < endPosition; i++)
            {
                Console.SetCursorPosition(0, i);
                ClearCurrentConsoleLine();
            }
        }

        public static int CompareScore(int playerScore, int maklerScore)
        {
            if (playerScore > maklerScore)
            {
                AnsiConsole.WriteLine("Wygrałeś z krupierem!");
                return 1;
            }
            if (playerScore == maklerScore)
            {
                AnsiConsole.WriteLine("REMIS!");
                return 0;
            }
            Console.WriteLine("Następnym razem się odkujesz");
            return -1;
        }

        public static int KrupierScore()
        {
            var krupierScore = 0;
            Random r = new Random();
            while (krupierScore <= 17)
            {
                int firstTake = r.Next(2, 11);
                krupierScore += firstTake;
            }
            Console.WriteLine($"Krupier miał w kartach {krupierScore} punktów, a Ty {_points}");
            return krupierScore;
        }
    }
}