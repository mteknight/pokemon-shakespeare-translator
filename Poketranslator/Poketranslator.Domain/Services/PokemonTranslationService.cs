using System;
using System.Threading;
using System.Threading.Tasks;
using Poketranslator.Data.Interfaces.Services;
using Poketranslator.Domain.Interfaces.Domain;
using Poketranslator.Domain.Interfaces.Models;
using Poketranslator.Domain.Interfaces.Services;
using Poketranslator.Domain.Models;

namespace Poketranslator.Domain.Services
{
    public class PokemonTranslationService : IPokemonTranslationService
    {
        private readonly IPokemonApiService _pokemonApiService;
        private readonly IShakespeareTranslationService _shakespeareTranslationService;

        public PokemonTranslationService(
            IPokemonApiService pokemonApiService,
            IShakespeareTranslationService shakespeareTranslationService)
        {
            _pokemonApiService = pokemonApiService ?? throw new ArgumentNullException(nameof(pokemonApiService));
            _shakespeareTranslationService = shakespeareTranslationService ?? throw new ArgumentNullException(nameof(shakespeareTranslationService));
        }

        public async Task<IPokemonModel> Translate(
            string pokemonName,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(pokemonName))
            {
                throw new ArgumentNullException("Value cannot be null or whitespace.", nameof(pokemonName));
            }

            IPokemon pokemon = await _pokemonApiService.GetByName(pokemonName, cancellationToken)
                .ConfigureAwait(false);

            if (pokemon is null)
            {
                return default;
            }

            string translation = await _shakespeareTranslationService.GetTranslation(pokemon.OriginalDescription, cancellationToken)
                .ConfigureAwait(false);

            return new PokemonModel
            {
                Name = pokemon.Name,
                Description = translation
            };
        }
    }
}