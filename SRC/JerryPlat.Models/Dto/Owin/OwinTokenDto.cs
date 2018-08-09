namespace JerryPlat.Models.Dto
{
    public class OwinTokenDto : OwinErrorDto
    {
        public string Access_Token { get; set; }
        public int Expires_In { get; set; }
        public string Refresh_Token { get; set; }
        public string OpenId { get; set; }
        public string Scope { get; set; }
    }
}