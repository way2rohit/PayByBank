namespace Pokedex.Domain.Models
{
    public class GetPokemonSpeicesRequest
    {
        public string PockemonName { get; set; }
        public bool IsTranslationRequired { get; set; }
    }
}
