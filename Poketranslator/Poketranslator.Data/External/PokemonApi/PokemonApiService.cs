using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Poketranslator.Domain;
using PokeApiNet;
using Poketranslator.Data.Interfaces.External.PokemonApi;
using Pokemon = PokeApiNet.Pokemon;

namespace Poketranslator.Data.External.PokemonApi
{
    public class PokemonApiService : IPokemonApiService
    {
        private async Task<Domain.Pokemon> GetByNamePrototype(string pokemonName)
        {
            using (var client = new PokeApiClient())
            {
                var pokemonSpecies = await client.GetResourceAsync<PokemonSpecies>(pokemonName).ConfigureAwait(false);
                var name = pokemonSpecies.Name;
                var description = pokemonSpecies.FlavorTextEntries.FirstOrDefault(texts => texts.Language.Name == "en");

                return new Domain.Pokemon
                {
                    Name = name,
                    OriginalDescription = description?.FlavorText
                };
            }
        }

        public async Task<Domain.Pokemon> GetByName(string pokemonName)
        {
            if (string.IsNullOrEmpty(pokemonName))
            {
                throw new ArgumentNullException("Value cannot be null or empty.", nameof(pokemonName));
            }

            using (var client = new PokeApiClient())
            {
                var pokemonSpecies = await client.GetResourceAsync<PokemonSpecies>(pokemonName).ConfigureAwait(false);
                var description = pokemonSpecies.FlavorTextEntries
                    .FirstOrDefault(texts => texts.Language.Name == "en");

                var parsedDescription = Regex.Replace(description?.FlavorText ?? string.Empty, @"\t|\n|\r", " ");

                return new Domain.Pokemon
                {
                    Name = pokemonSpecies.Name,
                    OriginalDescription = parsedDescription
                };
            }
        }
    }

    public interface IPokeApiClientFactory
    {
        PokeApiClient Create();
    }

    public class PokeApiClientFactory : IPokeApiClientFactory
    {
        public PokeApiClient Create() => new PokeApiClient();
    }
}
