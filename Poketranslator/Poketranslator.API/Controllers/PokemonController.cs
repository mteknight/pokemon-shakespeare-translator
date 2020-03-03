using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Poketranslator.Domain.Interfaces.Services;
using Poketranslator.Domain.Models;

namespace Poketranslator.API.Controllers
{
    public class PokemonController : ApiController
    {
        private readonly IPokemonTranslationService _pokemonTranslationService;

        public PokemonController(
            IPokemonTranslationService pokemonTranslationService)
        {
            _pokemonTranslationService = pokemonTranslationService ?? throw new ArgumentNullException(nameof(pokemonTranslationService));
        }

        public async Task<IHttpActionResult> Get(
            string pokemonName,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(pokemonName))
            {
                return BadRequest("Value cannot be null or whitespace.");
            }

            var model = await _pokemonTranslationService.Translate(pokemonName, cancellationToken)
                .ConfigureAwait(false);

            return model is null
                ? (IHttpActionResult) NotFound()
                : Ok(model);
        }
    }
}