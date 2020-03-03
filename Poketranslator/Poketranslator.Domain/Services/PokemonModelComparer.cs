using System.Collections.Generic;
using Poketranslator.Domain.Interfaces.Models;
using Poketranslator.Domain.Models;

namespace Poketranslator.Domain.Services
{
    public class PokemonModelComparer : IEqualityComparer<IPokemonModel>
    {
        public bool Equals(
            IPokemonModel left,
            IPokemonModel right)
        {
            return left != null &&
                   right != null &&
                   left.Name == right.Name &&
                   left.Description == right.Description;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="pokemon">The Pokemon object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        /// <remarks>https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-overriding-gethashcode</remarks>
        public int GetHashCode(IPokemonModel pokemon)
        {
            unchecked // Overflow is fine, just wrap
            {
                var hash = 17;

                // Suitable nullity checks etc, of course :)
                hash *= GetHashCode(pokemon.Name);
                hash *= GetHashCode(pokemon.Description);

                return hash;
            }
        }

        private static int GetHashCode(object obj) => 23 + obj.GetHashCode();
    }
}