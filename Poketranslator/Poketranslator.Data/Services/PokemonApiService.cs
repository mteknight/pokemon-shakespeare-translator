using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using PokeApiNet;
using Poketranslator.Data.Interfaces.Services;
using Poketranslator.Data.Interfaces.Wrappers;
using Poketranslator.Domain.Interfaces.Domain;
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

        public async Task<IPokemon> GetByName(
            string pokemonName,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(pokemonName))
            {
                throw new ArgumentNullException("Value cannot be null or empty.", nameof(pokemonName));
            }

            var pokemonSpecies = await _pokeApiClientWrapper.GetResourceAsync<PokemonSpecies>(pokemonName, cancellationToken)
                .ConfigureAwait(false);

            return pokemonSpecies is null
                ? default
                : GetPokemonDetails(pokemonSpecies);
        }

        private static IPokemon GetPokemonDetails(PokemonSpecies pokemonSpecies)
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

        private static IPokemon CreatePokemon(string pokemonName, string parsedDescription)
        {
            return new Domain.Pokemon
            {
                Name = pokemonName,
                OriginalDescription = parsedDescription
            };
        }
    }
}