using PokemonAPI.DeckHandler;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.PerformanceData;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PokemonAPI;
using PokemonAPI.ASCII;
using PokemonAPI.JsonHandler;

namespace BlackJackWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int score = 0;
        private int bet = 0;
        private bool isBetPlaced = false;
        public int coins = 1000;
        public int cardPosition = 50;
        List<Image> list = new List<Image>();

        public MainWindow()
            {
                InitializeComponent();
                txtCoins.Content = "Żetony " + coins;
                txtGameInfo.Content = "Witaj w kasynie :) ";
            }

        private void Button_TakeCard(object sender, RoutedEventArgs e)
        {

            if(isBetPlaced is false)
            {
                txtGameInfo.Content = "Musisz obstawić zakład przed rozpoczęciem gry";
            }
            else
            {


                txtGameInfo.Content = "Rozpoczales gre, powodzenia!";
                var card = TakeCard();
                txtUserPoints.Content = $"Punkty: {score}";

                var cardImage = CreateCard();

                list.Add((cardImage));
                cardPosition += 30;


                GridMain.Children.Add(cardImage);
                cardImage.Source = new BitmapImage(new Uri((string)card.image, UriKind.RelativeOrAbsolute));

                if (score > BlackJack.MAX_SCORE)
                {

                    txtUserPoints.Visibility = 0;
                    coins -= bet;

                    MessageBox.Show("Przekroczyłeś 21 punktów");

                    if (coins == 0)
                    {
                        MessageBox.Show($"Przegrałeś wszystkie żetony, zostajesz wyrzucony z kasyna :( {AsciiCards.Pepe}");
                        Close();
                    }

                    UserContiunationDecision("Przegrałeś");

                }

                if (score == BlackJack.MAX_SCORE)
                {
                    coins += bet;
                    MessageBox.Show("WYGRAŁEŚ! BLACKJACK!");
                    UserContiunationDecision("Wygrałeś");
                }


                

            }
        }

        private void RestartScoreAndBet()
        {
            bet = 0;
            score = 0;
            txtBet.Content = bet;
            txtUserPoints.Content = score;
            txtGameInfo.Content = "Obstaw zakład";
            txtPlacedBet.Content = 0;
            cardPosition = 50;
            isBetPlaced = false;
            txtCoins.Content = "Żetony " + coins;
            ClearAllImages();




        }

        private void ClearAllImages()
        {
            foreach (var image in list)
            {
                image.Height = 0;
                image.Width = 0;
            }
            list.Clear();
        }

        private void UserContiunationDecision(string v)
        {
            string messageBoxText = "";

            if (v == "Przegrałeś")
            {
                 messageBoxText = $"{v} {bet}, czy chcesz kontynuować grę? {AsciiCards.smallPepe}";
            }
                messageBoxText = $"{v} {bet}, czy chcesz kontynuować grę?";


            string caption = "Decyzja";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);

            switch (result)
            {
                case MessageBoxResult.Cancel:
                    MessageBox.Show($"Dziekuje za gre, wygrałeś {coins} dolarów");
                    Close();
                    break;
                case MessageBoxResult.Yes:
                    MessageBox.Show("Gramy dalej");
                    if (coins == 0)
                    {
                        MessageBox.Show($"Krupier dostrzegł, że skończyły Ci się żetony i zostałeś wyrzucony z kasyna :( {AsciiCards.smallPepe}")
                        ;
                        Close();
                    }
                    RestartScoreAndBet();
                    break;
                case MessageBoxResult.No:
                    MessageBox.Show($"Dziekuje za gre, wygrałeś {coins} dolarów");
                    Close();
                    break;
            }
        }


        private Card TakeCard()
        {
            var deck = DeckManager.ShuffleDeck();
            var cardList = DeckManager.DrawACardFromADeck(deck.deck_id);
            Card card = cardList.cards[0];

            score += card.value switch
            {
                "KING" => 10,
                "QUEEN" => 10,
                "JACK" => 10,
                "ACE" => 11,
                _ => Convert.ToInt32(card.value),
            };

            return card;
        }


        private void Button_Add500ToBetClick(object sender, RoutedEventArgs e)
        {
            bet += 500;
            txtBet.Content = bet;

        }

        private void Button_Add100ToBetClick(object sender, RoutedEventArgs e)
        {
            bet += 100;
            txtBet.Content = bet;


        }

        private void Button_Add5ToBetClick(object sender, RoutedEventArgs e)
        {
            bet += 5;
            txtBet.Content = bet;

        }

        private void Button_Add25ToBetClick(object sender, RoutedEventArgs e)
        {
            bet += 25;
            txtBet.Content = bet;


        }

        private void Button_Add1ToBetClick(object sender, RoutedEventArgs e)
        {
            bet += 1;
            txtBet.Content = bet;

        }

        private void Button_Stand(object sender, RoutedEventArgs e)
        {
            if (isBetPlaced is false)
            {
                txtGameInfo.Content = "Musisz być w trakcie gry!";
                return;
                
            }

            if (score == 0)
            {
                txtGameInfo.Content = "Nie można pasować mając 0 punktów!";
                return;
                
            }

            var krupierScore = BlackJack.KrupierScore(score);
            var value = BlackJack.CompareScore(score, krupierScore);
            switch (value)
            {
                case -1:
                    coins -= bet;

                    if (coins == 0)
                    {
                        MessageBox.Show($" Krupier miał {krupierScore} w kartach, przegrałeś wszystkie żetony, zostajesz wyrzucony z kasyna :( {AsciiCards.Pepe}");
                        Close();
                        return;
                    }

                    MessageBox.Show($"Krupier miał w kartach {krupierScore}, przegrałeś!");

                    UserContiunationDecision("Przegrałeś");
                    
                    RestartScoreAndBet();

                    break;
                case 0:
                    
                    MessageBox.Show($"REMIS, krupier miał {krupierScore} w kartach");
                    RestartScoreAndBet();

                    break;
                case 1:
                    MessageBox.Show($"Krupier miał w kartach {krupierScore}, wygrałeś!");
                    coins += bet;

                    RestartScoreAndBet();
                    break;
            }

           
        }

       

        private void Button_PlaceBet(object sender, RoutedEventArgs e)
        {
            if (bet == 0)
                txtGameInfo.Content = "Nie możesz obstawić 0";
            else if (bet > coins)
                txtGameInfo.Content = "Nie masz tylu żetonów";
            else
            {
                txtGameInfo.Content = "Zaklad obstawiono pomyślnie";


                isBetPlaced = true;
                txtPlacedBet.Content = $"Obstawiles: {bet}";
                txtBet.Content = 0;
            }
        }

        private Image CreateCard()
        {
            Image img = new Image
            {
                Height = 140,
                Width = 90,
                Margin = new Thickness(cardPosition,150,0,0)

            };
            return img;
        }

        private void Button_RestartBet_Click(object sender, RoutedEventArgs e)
        {
            if (isBetPlaced is false)
            {
                bet = 0;
                txtBet.Content = 0;
                txtGameInfo.Content = "Zrestartowales swoj zaklad";
            }
            else
            {

                txtGameInfo.Content = "Nie mozesz restartować zakładu w trakcie gry!";
            }
        }
    }
}
