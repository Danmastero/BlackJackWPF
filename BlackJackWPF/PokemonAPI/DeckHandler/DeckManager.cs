using Newtonsoft.Json;
using PokemonAPI.JsonHandler;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PokemonAPI.DeckHandler
{
    public class DeckManager
    {
        public static Deck ShuffleDeck()
        {
            var client = new RestClient($"https://deckofcardsapi.com/api/deck/new/shuffle/?deck_count=6");
            var request = new RestRequest();

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string rawResponse = response.Content;

                var result = JsonConvert.DeserializeObject<Deck>(rawResponse);

                return result;

            }

            return new Deck();
        }

        public static CardFromDeck DrawACardFromADeck(string deckId)
        {
            var client = new RestClient($"https://deckofcardsapi.com/api/deck/{deckId}/draw/?count=1");
            var request = new RestRequest();

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string rawResponse = response.Content;

                var result = JsonConvert.DeserializeObject<CardFromDeck>(rawResponse);

                return result;

            }

            return new CardFromDeck();

        }
    }
}
