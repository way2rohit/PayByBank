namespace Pokedex.Domain.Models
{
    public class GetPokemonInfoRequest
    {
        public string PockemonName { get; set; }
        public bool IsTranslationRequired { get; set; }
    }
}
