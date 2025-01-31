using DVLD.DTOs;
using System;

namespace DVLD_Business
{
    public class Driver : Person
    {
        public int DriverId { set; get; }
        public int PersonId { set; get; }
        public int UserId { set; get; }
        public DateTime CreatedDate { get; set; }

        public Driver() { }

        public Driver(DriverDTO dto, Person person) : base(person)
        {
            this.DriverId = dto.DriverID;
            this.UserId = dto.CreatedByUserID;
            this.CreatedDate = dto.CreatedDate;
        }
    }

}
