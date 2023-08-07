namespace RandevuSistemi.Models.Entities
{
    public class Randevu
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public CalismaSaatleri randevuSaati { get; set; }
    }
}
