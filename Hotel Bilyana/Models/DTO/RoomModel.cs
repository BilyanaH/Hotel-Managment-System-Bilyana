using System.ComponentModel.DataAnnotations;

namespace Hotel_Bilyana.Models.DTO
{
    public class RoomModel
    {
        public enum RoomType
        {
            Single = 0,
            Double = 1,
            Triple = 2,
            Quad = 3,
            Queen = 4,
            Other = 5
        }

        [Key]
        public int RoomId { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        public int Capacity { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public RoomType Type { get; set; }
        [Required]
        public bool IsFree { get; set; }
      //  public virtual ICollection<ReservationModel> Reservations { get; set; }
    }
}
