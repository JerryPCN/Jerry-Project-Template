namespace JerryPlat.Utils.Models
{
    public class BaseSearchModel : ISearchModel
    {
        public int Id { get; set; }
        public string Sort { get; set; }
    }
}