namespace DVLD_Business
{
    public class User
    {
        public int UserID { set; get; }
        public int PersonID { set; get; }
        public Person Person { set; get; }
        public string UserName { set; get; }
        public string Password { set; get; }
        public bool IsActive { set; get; }

    }

}
