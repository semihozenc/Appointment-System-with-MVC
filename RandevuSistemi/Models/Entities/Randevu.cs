namespace RandevuSistemi.Models.Entities
{
    public class Randevu
    {
        public int Id { get; set; }
        public int DoktorId { get; set; }
        public int UserId { get; set; }
        public int RandevuSaati { get;set; }
        public string Description { get; set; }
    }
}
