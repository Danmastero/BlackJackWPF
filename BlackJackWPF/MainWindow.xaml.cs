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

            public MainWindow()
            {
                InitializeComponent();
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

                if (score > BlackJack.MAX_SCORE)
                {

                    BlackJack.ReduceCoins(bet);
                    txtUserPoints.Visibility = 0;

                    //Zeruje punkty
                    RestartScoreAndBet();
                    UserContiunationDecision();
                }

                imgCard.Source = new BitmapImage(new Uri((string) card.image, UriKind.RelativeOrAbsolute));

            }
        }

        private void RestartScoreAndBet()
        {
            bet = 0;
            score = 0;
            txtBet.Content = bet;
            txtUserPoints.Content = score;
        }

        private void UserContiunationDecision()
        {
            string messageBoxText = "Czy chcesz kontynuować grę, czy zabierasz żetony?";
            string caption = "Word Processor";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);

            switch (result)
            {
                case MessageBoxResult.Cancel:
                    MessageBox.Show("Dziekuje za gre");
                    Close();
                    break;
                case MessageBoxResult.Yes:
                    MessageBox.Show("Gramy dalej"); 
                    break;
                case MessageBoxResult.No:
                    MessageBox.Show("Dziekuje za gre");
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
            var krupierScore = BlackJack.KrupierScore();
            var value = BlackJack.CompareScore(score, krupierScore);

           
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
                Height = 200,
                Width = 150,
                Margin = new Thickness(100,100,100,100)

            };
            return img;
        }
    }
}
