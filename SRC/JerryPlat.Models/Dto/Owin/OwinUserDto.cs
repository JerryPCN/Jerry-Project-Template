namespace JerryPlat.Models.Dto
{
    public class OwinUserDto : OwinErrorDto
    {
        public string OpenId { get; set; }
        public string NickName { get; set; }
        public string Sex { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string country { get; set; }
        public string HeadImgUrl { get; set; }
    }
}