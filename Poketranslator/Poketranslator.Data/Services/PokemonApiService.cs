using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PokeApiNet;
using Poketranslator.Data.Interfaces.External.PokemonApi;
using Poketranslator.Data.Interfaces.Services;
using Pokemon = PokeApiNet.Pokemon;

namespace Poketranslator.Data.Services
{
    public class PokemonApiService : IPokemonApiService
    {
        private readonly IPokeApiClientWrapper _pokeApiClientWrapper;

        public PokemonApiService(
            IPokeApiClientWrapper pokeApiClientWrapper)
        {
            _pokeApiClientWrapper = pokeApiClientWrapper ?? throw new ArgumentNullException(nameof(pokeApiClientWrapper));
        }

        public async Task<Domain.Pokemon> GetByName(string pokemonName)
        {
            if (string.IsNullOrEmpty(pokemonName))
            {
                throw new ArgumentNullException("Value cannot be null or empty.", nameof(pokemonName));
            }

            var pokemonSpecies = await _pokeApiClientWrapper.GetResourceAsync<PokemonSpecies>(pokemonName).ConfigureAwait(false);

            return pokemonSpecies is null
                ? default
                : GetPokemonDetails(pokemonSpecies);
        }

        private static Domain.Pokemon GetPokemonDetails(PokemonSpecies pokemonSpecies)
        {
            var parsedDescription = GetDescription(pokemonSpecies);
            var pokemonName = pokemonSpecies.Name;

            return CreatePokemon(pokemonName, parsedDescription);
        }

        private static string GetDescription(PokemonSpecies pokemonSpecies)
        {
            return pokemonSpecies.FlavorTextEntries
                .Where(flavorTexts => flavorTexts.Language.Name == "en")
                .Select(ParseLineBreaks)
                .FirstOrDefault();
        }

        private static string ParseLineBreaks(PokemonSpeciesFlavorTexts flavorTexts)
            => Regex.Replace(flavorTexts.FlavorText, @"\t|\n|\r", " ");

        private static Domain.Pokemon CreatePokemon(string pokemonName, string parsedDescription)
        {
            return new Domain.Pokemon
            {
                Name = pokemonName,
                OriginalDescription = parsedDescription
            };
        }
    }
}