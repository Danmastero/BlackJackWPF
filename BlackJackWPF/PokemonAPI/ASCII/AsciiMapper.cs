using PokemonAPI.JsonHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PokemonAPI.ASCII
{
    public class AsciiMapper
    {
        protected readonly Card _card;
        public AsciiMapper(Card card)
        {
            _card = card;
        }

        public string MapCardToStruct()
        {
            return _card.suit switch
            {
                "SPADES" when _card.value == "2" => AsciiCards.SPADES_2,
                "SPADES" when _card.value == "3" => AsciiCards.SPADES_3,
                "SPADES" when _card.value == "4" => AsciiCards.SPADES_4,
                "SPADES" when _card.value == "5" => AsciiCards.SPADES_5,
                "SPADES" when _card.value == "6" => AsciiCards.SPADES_6,
                "SPADES" when _card.value == "7" => AsciiCards.SPADES_7,
                "SPADES" when _card.value == "8" => AsciiCards.SPADES_8,
                "SPADES" when _card.value == "9" => AsciiCards.SPADES_9,
                "SPADES" when _card.value == "10" => AsciiCards.SPADES_10,
                "SPADES" when _card.value == "JACK" => AsciiCards.SPADES_JACK,
                "SPADES" when _card.value == "QUEEN" => AsciiCards.SPADES_QUEEN,
                "SPADES" when _card.value == "KING" => AsciiCards.SPADES_KING,
                "SPADES" when _card.value == "ACE" => AsciiCards.SPADES_ACE,
                "DIAMONDS" when _card.value == "2" => AsciiCards.DIAMOND_2,
                "DIAMONDS" when _card.value == "3" => AsciiCards.DIAMOND_3,
                "DIAMONDS" when _card.value == "4" => AsciiCards.DIAMOND_4,
                "DIAMONDS" when _card.value == "5" => AsciiCards.DIAMOND_5,
                "DIAMONDS" when _card.value == "6" => AsciiCards.DIAMOND_6,
                "DIAMONDS" when _card.value == "7" => AsciiCards.DIAMOND_7,
                "DIAMONDS" when _card.value == "8" => AsciiCards.DIAMOND_8,
                "DIAMONDS" when _card.value == "9" => AsciiCards.DIAMOND_9,
                "DIAMONDS" when _card.value == "10" => AsciiCards.DIAMOND_10,
                "DIAMONDS" when _card.value == "JACK" => AsciiCards.DIAMOND_JACK,
                "DIAMONDS" when _card.value == "QUEEN" => AsciiCards.DIAMOND_QUEEN,
                "DIAMONDS" when _card.value == "KING" => AsciiCards.DIAMOND_KING,
                "DIAMONDS" when _card.value == "ACE" => AsciiCards.DIAMOND_ACE,
                "HEARTS" when _card.value == "2" => AsciiCards.HEARTS_2,
                "HEARTS" when _card.value == "3" => AsciiCards.HEARTS_3,
                "HEARTS" when _card.value == "4" => AsciiCards.HEARTS_4,
                "HEARTS" when _card.value == "5" => AsciiCards.HEARTS_5,
                "HEARTS" when _card.value == "6" => AsciiCards.HEARTS_6,
                "HEARTS" when _card.value == "7" => AsciiCards.HEARTS_7,
                "HEARTS" when _card.value == "8" => AsciiCards.HEARTS_8,
                "HEARTS" when _card.value == "9" => AsciiCards.HEARTS_9,
                "HEARTS" when _card.value == "10" => AsciiCards.HEARTS_10,
                "HEARTS" when _card.value == "JACK" => AsciiCards.HEARTS_JACK,
                "HEARTS" when _card.value == "QUEEN" => AsciiCards.HEARTS_QUEEN,
                "HEARTS" when _card.value == "KING" => AsciiCards.HEARTS_KING,
                "HEARTS" when _card.value == "ACE" => AsciiCards.HEARTS_ACE,
                "CLUBS" when _card.value == "2" => AsciiCards.CLUBS_2,
                "CLUBS" when _card.value == "3" => AsciiCards.CLUBS_3,
                "CLUBS" when _card.value == "4" => AsciiCards.CLUBS_4,
                "CLUBS" when _card.value == "5" => AsciiCards.CLUBS_5,
                "CLUBS" when _card.value == "6" => AsciiCards.CLUBS_6,
                "CLUBS" when _card.value == "7" => AsciiCards.CLUBS_7,
                "CLUBS" when _card.value == "8" => AsciiCards.CLUBS_8,
                "CLUBS" when _card.value == "9" => AsciiCards.CLUBS_9,
                "CLUBS" when _card.value == "10" => AsciiCards.CLUBS_10,
                "CLUBS" when _card.value == "JACK" => AsciiCards.CLUBS_JACK,
                "CLUBS" when _card.value == "QUEEN" => AsciiCards.CLUBS_QUEEN,
                "CLUBS" when _card.value == "KING" => AsciiCards.CLUBS_KING,
                "CLUBS" when _card.value == "ACE" => AsciiCards.CLUBS_ACE,
                _ => ""
            };
        }
    }
}
