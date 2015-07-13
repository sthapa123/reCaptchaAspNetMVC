using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace reCaptcha.Models
{
    public class UserModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        public bool isValid()
        {
            return true;    //Sir, yes sir
        }

    }



    // Users Class
    public class Users
    {
        // List variable
        public List<UserModel> _usrList = new List<UserModel>();

        public Users()
        {   
            // Add dummy users
            _usrList.Add(new UserModel
            {
                UserName = "admin",
                Password = "admin"
            });
            _usrList.Add(new UserModel
            {
                UserName = "demo",
                Password = "demo"
            });
        }

        
        /*
         * Returns UserModel object specified by entered UserName
         * null if not found.
         */
        public UserModel GetUser(string uid)
        {
            UserModel usrMdl = null;

            foreach (UserModel um in _usrList)
                if (um.UserName == uid)
                    usrMdl = um;

            return usrMdl;
        }

    }
}