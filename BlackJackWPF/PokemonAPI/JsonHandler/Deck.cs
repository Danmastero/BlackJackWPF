using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonAPI.JsonHandler
{
        public class Deck
        {
            public bool success { get; set; }
            public string deck_id { get; set; }
            public int remaining { get; set; }
            public bool shuffled { get; set; }
        }

}
