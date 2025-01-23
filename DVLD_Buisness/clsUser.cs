using System;
using System.Data;
using System.Runtime.InteropServices;
using DVLD.DTOs;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public  class clsUser
    {
        //This is for testing purpose, it will be updated later
        private PersonService _personService = new PersonService();

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int UserID { set; get; }
        public int PersonID { set; get; }
        public Person PersonInfo;
        public string UserName { set; get; }
        public string Password { set; get; }
        public bool IsActive { set; get; }
     
        public clsUser()

        {     
            this.UserID = -1;
            this.UserName = "";
            this.Password = "";
            this.IsActive = true;
            Mode = enMode.AddNew;
        }

        private clsUser(UsersDTO usersDTO)
        {
            this.UserID = usersDTO.UserID; 
            this.PersonID = usersDTO.PersonID;
            this.PersonInfo = _personService.Find(PersonID);
            this.UserName = usersDTO.Username;
            this.Password = usersDTO.Password;
            this.IsActive = usersDTO.IsActive;

            Mode = enMode.Update;
        }

        private bool _AddNewUser()
        {
            var usersDTO = new UsersDTO()
            {
                PersonID = this.PersonID,
                Username = this.UserName,
                Password = this.Password,
                IsActive = this.IsActive
            };

            this.UserID = clsUserData.AddNewUser(usersDTO);

            return (this.UserID != -1);
        }

        private bool _UpdateUser()
        {
            var usersDTO = new UsersDTO()
            {
                UserID = this.UserID,
                PersonID = this.PersonID,
                Username = this.UserName,
                Password = this.Password,
                IsActive = this.IsActive
            };

            return clsUserData.UpdateUser(usersDTO);
        }
 
        public static clsUser FindByUserID(int UserID)
        {
            var usersDTO = clsUserData.GetUserInfoByUserId(UserID);

            if (usersDTO != null)
                return new clsUser(usersDTO);
            else
                return null;
        }
        
        public static clsUser FindByPersonID(int PersonID)
        {
            var usersDTO = clsUserData.GetUserInfoByPersonId(PersonID);

            if (usersDTO != null)
                return new clsUser(usersDTO);
            else
                return null;
        }
        
        public static clsUser FindByUsernameAndPassword(string username,string password)
        {
            var usersDTO = clsUserData.GetUserInfoByUsernameAndPassword(username, password);

            if (usersDTO != null)
                return new clsUser(usersDTO);
            else
                return null;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateUser();

            }

            return false;
        }

        public static DataTable GetAllUsers()
        {
            return clsUserData.GetAllUsers();
        }

        public static bool DeleteUser(int UserID)
        {
            return clsUserData.DeleteUser(UserID); 
        }

        public static bool isUserExist(int UserID)
        {
           return clsUserData.IsUserExist(UserID);
        }

        public static bool isUserExist(string UserName)
        {
            return clsUserData.IsUserExist(UserName);
        }

        public static bool isUserExistForPersonID(int PersonID)
        {
            return clsUserData.IsUserExistForPersonId(PersonID);
        }


    }
}
