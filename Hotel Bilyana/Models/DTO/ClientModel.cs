using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel_Bilyana.Models
{
    public class ClientModel
    {
        [Key]
        public int ClientId { get; set; }
        [StringLength(50, MinimumLength = 3)]
        [Required]
        public string FirstName { get; set; }
        [StringLength(50, MinimumLength = 3)]
        [Required]
        public string Surname { get; set; }

        // [RegularExpression(@"[^0-9]", ErrorMessage = "Please enter propar contact data!")]
        [StringLength(10, MinimumLength = 10)]
        [Phone]
        [Required]
        public string PhoneNumber { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public bool IsAdult { get; set; }
       // public int ReservationId { get; set; }

        //[NotMapped]
   
     //public virtual ICollection<ReservationModel> Reservations { get; set; }
     // public virtual ReservationModel Reservation { get; set; }
    }
}
