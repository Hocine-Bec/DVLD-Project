using clsPerson_Layer;
using Database_Layer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer
{
   public class clsUsers
    {
        public enum enMode { AddNew = 1, Update = 2 }
        enMode Mode;

        public clsPerson Person { get; set; }
        public int UserID { get; set; }
        public int PersonID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        public clsUsers(int UserID, int PersonID, string Username, string Password, bool IsActive)
        {
            this.UserID = UserID;
            this.Person = clsPerson.Find(PersonID);
            this.Username = Username;
            this.Password = Password;
            this.IsActive = IsActive;

            Mode = enMode.Update;
        }

        public clsUsers()
        {
            Mode = enMode.AddNew;
        }

        private bool _AddNewUser()
        {
            if (PersonID != -1)
            {
                UserID = clsUserDataLayer.AddNewUser(PersonID, Username, Password, IsActive);
                return (UserID != 0);
            }
            else
            {
                return false;
            }
        }

        private bool _UpdateUser()
        {
            return (clsUserDataLayer.UpdateUser(UserID, Username, Password, IsActive));
        }

        public bool DeleteUser()
        {
            return clsUserDataLayer.Delete(this.UserID);
        }

        public static clsUsers FindUserWithPersonID(int PersonID)
        {
            int UserID = 0;
            string Username = "", Password = "";
            bool IsActive = false;


            //After checking, This method fills this instance with User's data
            if (clsUserDataLayer.FindUserWithPersonID(ref UserID, ref PersonID, ref Username, ref Password, ref IsActive))
            {
                return new clsUsers(UserID, PersonID, Username, Password, IsActive);
            }

            return null;
        }

        public static clsUsers FindUserWithID(int UserID)
        {
            int PersonID = 0;
            string Username = "", Password = "";
            bool IsActive = false;
                    
            //After checking, This method fills this instance with User's data
            if (clsUserDataLayer.FindUserWithUserID(ref UserID, ref PersonID, ref Username, ref Password, ref IsActive))
            {
                return new clsUsers(UserID, PersonID, Username, Password, IsActive);
            }

            return null;
        }

        public static clsUsers FindUserWithNationalNo(string NationalNo)
        {
            int PersonID = 0, UserID = 0;
            string Username = "", Password = "";
            bool IsActive = false;

            if (clsUserDataLayer.FindUserWithNationalNo(NationalNo, ref UserID, ref PersonID, ref Username, 
                ref Password, ref IsActive))
            {
                return new clsUsers(UserID, PersonID, Username, Password, IsActive);
            }

            return null;
        }

        public static clsUsers FindUsersWithFilters(string Filter, string name)
        {
            int PersonID = 0, UserID = 0;
            string Username = "", Password = "";
            bool IsActive = false;

            if (clsUserDataLayer.FindUsersWithFilters(Filter, name, ref UserID, ref PersonID, ref Username, 
                ref Password, ref IsActive))
            {
                return new clsUsers(UserID, PersonID, Username, Password, IsActive);
            }

            return null;
        }

        public static DataTable GetUsersList()
        {
            return clsUserDataLayer.GetUsersList();
        }

        public static bool IsCorrectPassword(int ID, string password)
        {
            return clsUserDataLayer.IsCorrectPassword(ID, password);
        }

        public static bool IsUserExist(int UserID)
        {
            return clsUserDataLayer.IsUserExist(UserID);
        }

        public static bool IsUser(int PersonID)
        {
            return clsUserDataLayer.IsUser(PersonID);
        }

        public bool Save()
        {

            switch (Mode)
            {
                case enMode.AddNew:
                    Mode = enMode.Update;
                    return _AddNewUser();

                case enMode.Update:
                    return _UpdateUser();

            }
            return false;

        }

        public static bool UpdatePassword(int ID, string NewPassword)
        {
            return clsUserDataLayer.UpdatePassword(ID, NewPassword);
        }
    }
}
