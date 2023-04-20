using Hotel_Bilyana.Models.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Hotel_Bilyana.Models.Domain;
using Hotel_Bilyana.Models.DTO;


namespace Hotel_Bilyana.Models
{
    public class ReservationModel
    {
        public decimal CalculateFinalPrice(DatabaseContext dbContext)
        {
            var room = dbContext.RoomModel.FirstOrDefault(r => r.RoomId == this.RoomId);

            decimal basePrice = (decimal)room.Price * (decimal)(DateOfLeaving - DateOfArrival).TotalDays;

            decimal breakfastPrice = 0;
            if (IsIncludedBreakfast)
            {
                breakfastPrice = 20 * (decimal)(DateOfLeaving - DateOfArrival).TotalDays;
            }

            decimal allInclusivePrice = 0;
            if (AllInclusive)
            {
                allInclusivePrice = 60 * (decimal)(DateOfLeaving - DateOfArrival).TotalDays;
            }

            return basePrice + breakfastPrice + allInclusivePrice;
        }

        [Key]
        public int ReservationId { get; set; }
        public ReservationModel()
        {
              this.Clients = new HashSet<ClientModel>();
              this.Rooms = new HashSet<RoomModel>();
        }
        public int ClientId { get; set; }
        public virtual ICollection<ClientModel> Clients { get; set; }
        public virtual ClientModel Client { get; set; }
        //public int ClientId { get; set; }
        //public virtual ClientModel Client { get; set; }

        public int RoomId { get; set; }
        public virtual ICollection<RoomModel> Rooms { get; set; }
        public virtual RoomModel Room { get; set; }
        //public int RoomId { get; set; }
        //public virtual RoomModel Room { get; set; }

        [DisplayName("Date of arrival: ")]
        public DateTime DateOfArrival { get; set; }

        [DisplayName("Date of leaving: ")]
        public DateTime DateOfLeaving { get; set; }
        public bool IsIncludedBreakfast { get; set; }
        public bool AllInclusive { get; set; }
        public  decimal FinalPrice { get; set; }
    }
}
