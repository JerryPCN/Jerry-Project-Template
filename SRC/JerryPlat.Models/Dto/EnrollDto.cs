namespace JerryPlat.Models.Dto
{
    public class EnrollDto
    {
        public Enroll Enroll { get; set; }
        public string Course { get; set; }

        public EnrollDto()
        {
            Enroll = new Enroll();
        }
    }
}