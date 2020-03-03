using System;
using System.Collections.Generic;
using System.Text;
using Poketranslator.Domain.Interfaces.Models;

namespace Poketranslator.Domain.Models
{
    public class PokemonModel : IPokemonModel
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
