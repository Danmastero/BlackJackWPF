using PokemonAPI.DeckHandler;
using System;
using System.Collections.Generic;
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
using PokemonAPI.JsonHandler;

namespace BlackJackWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int score = 0;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_TakeCard(object sender, RoutedEventArgs e)
        {
            var card = TakeCard();
            txtUserPoints.Content = $"Punkty: {score}";




            imgCard.Source = new BitmapImage(new Uri((string) card.image, UriKind.RelativeOrAbsolute));
            

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
    }
}
