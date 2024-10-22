﻿using System.Threading;
using System.Threading.Tasks;
using PokeApiNet;

namespace Poketranslator.Data.Interfaces.Wrappers
{
    public interface IPokeApiClientWrapper
    {
        Task<TNamedApiResource> GetResourceAsync<TNamedApiResource>(
            string pokemonName,
            CancellationToken cancellationToken)
            where TNamedApiResource : NamedApiResource;
    }
}