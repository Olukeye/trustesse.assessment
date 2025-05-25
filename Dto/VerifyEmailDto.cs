namespace Trustesse_Assessment.Dto
{
    public class VerifyEmailDto
    {
        public int Id { get; set; }
        public string email { get; set; }
        public string token { get; set; }
    }
}
