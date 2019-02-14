using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Validation;
using Contacts.Models;

namespace Contacts.ProxyModels
{
    public class ProxyContact : ValidatableModelBase
    {
        #region Constructors
        public ProxyContact(Models.Contacts contact)
        {
            FirstName = contact.FirstName;
            LastName = contact.LastName;
            PhoneNumber = contact.PhoneNumber;
            //PathToImage = contact.PathToImage;
            Email = contact.Email;
            IsFavorite = contact.IsFavorite;
            GroupID = contact.GroupID;
        }
        #endregion

        public String FirstName { get { return Read<string>(); } set { Write(value); } }

        public String LastName { get { return Read<string>(); } set { Write(value); } }

        public String PhoneNumber { get { return Read<string>(); } set { Write(value); } }

        //public String PathToImage { get { return Read<string>(); } set { Write(value); } }

        public String Email { get { return Read<string>(); } set { Write(value); } }

        public Int64? IsFavorite { get { return Read<Int64?>(); } set { Write(value); } }

        public Int64? GroupID { get { return Read<Int64?>(); } set { Write(value); } }

        #region Calculated properties
        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }
        #endregion
    }
}
