using Poketranslator.Domain.Interfaces.Domain;

namespace Poketranslator.Domain
{
    public class Pokemon : IPokemon
    {
        public string Name { get; set; }

        public string OriginalDescription { get; set; }

        public string Translation { get; set; }
    }
}
