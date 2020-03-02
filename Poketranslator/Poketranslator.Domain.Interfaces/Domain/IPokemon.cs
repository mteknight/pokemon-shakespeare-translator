namespace Poketranslator.Domain.Interfaces.Domain
{
    public interface IPokemon
    {
        string Name { get; set; }

        string OriginalDescription { get; set; }

        string Translation { get; set; }
    }
}