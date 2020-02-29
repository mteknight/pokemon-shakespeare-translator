using System.Collections.Generic;
using PokeApiNet;

namespace Poketranslator.Data.Tests.Services
{
    public static class PokemonSpeciesHelper
    {
        public static PokemonSpecies CreatePokemonSpecies(
            string pokemonName,
            string expectedDescription,
            string language)
        {
            return new PokemonSpecies
            {
                Name = pokemonName,
                FlavorTextEntries = new List<PokemonSpeciesFlavorTexts>
                {
                    CreatePokemonSpeciesFlavorTexts(expectedDescription, language)
                }
            };
        }

        public static PokemonSpeciesFlavorTexts CreatePokemonSpeciesFlavorTexts(
            string expectedDescription,
            string language)
        {
            return new PokemonSpeciesFlavorTexts
            {
                FlavorText = expectedDescription,
                Language = CreateLanguageApiResource(language)

            };
        }

        public static NamedApiResource<Language> CreateLanguageApiResource(string name) => new NamedApiResource<Language> { Name = name };
    }
}
