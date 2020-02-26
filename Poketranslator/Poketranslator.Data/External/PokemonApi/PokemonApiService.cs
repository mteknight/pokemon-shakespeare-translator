using System;
using System.Globalization;
using System.Linq;
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
            throw new NotImplementedException();
        }
    }
}
