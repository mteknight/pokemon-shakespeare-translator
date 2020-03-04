using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Poketranslator.Domain.Interfaces.Models;
using Poketranslator.Domain.Interfaces.Services;

namespace Poketranslator.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonTranslationService _pokemonTranslationService;

        public PokemonController(
            IPokemonTranslationService pokemonTranslationService)
        {
            _pokemonTranslationService = pokemonTranslationService ?? throw new ArgumentNullException(nameof(pokemonTranslationService));
        }

        [HttpGet]
        [Route("{pokemonName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IPokemonModel>> Get(
            [FromRoute] string pokemonName,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(pokemonName))
            {
                return BadRequest("Value cannot be null or whitespace.");
            }

            var model = await _pokemonTranslationService.Translate(pokemonName, cancellationToken)
                .ConfigureAwait(false);

            return model is null
                ? (ActionResult<IPokemonModel>) NotFound()
                : Ok(model);
        }
    }
}