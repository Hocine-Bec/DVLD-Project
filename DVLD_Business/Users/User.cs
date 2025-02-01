using DVLD.DTOs;

namespace DVLD_Business
{
    public class User : Person
    {
        public int UserID { set; get; }
        public string UserName { set; get; }
        public string Password { set; get; }
        public bool IsActive { set; get; }

        public User() { }

        public User(UsersDTO dto, Person person) : base(person)
        {
            this.UserID =   dto.UserID;
            this.UserName = dto.Username;
            this.Password = dto.Password;
            this.IsActive = dto.IsActive;
        }

    }

}
