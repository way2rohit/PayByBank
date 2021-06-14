
namespace Pokedex.Domain.Models
{
    public class GetTranslationRequest
    {
        public string Habitat { get; set; }
        public bool IsLegendary { get; set; }
        public string Text { get; set; }
    }
}
