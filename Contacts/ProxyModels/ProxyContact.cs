using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Validation;
using Contacts.Message;
using Contacts.Models;
using GalaSoft.MvvmLight.Messaging;

namespace Contacts.ProxyModels
{
    public class ProxyContact : ValidatableModelBase
    {
        #region 
        Models.Contacts originalContact;
        #endregion

        #region Constructors
        public ProxyContact(Models.Contacts contact)
        {
            originalContact = contact;
            //Current = new ProxyContact(contact);

            FirstName = contact.FirstName;
            LastName = contact.LastName;
            PhoneNumber = contact.PhoneNumber;
            PathToImage = contact.PathToImage;
            Email = contact.Email;
            IsFavorite = contact.IsFavorite;
            GroupID = contact.GroupID;
        }
        #endregion

        //private ProxyContact Current { get; }

        public String FirstName { get { return Read<string>(); } set { Write(value); CheckIsDirty("FirstName"); } }

        public String LastName { get { return Read<string>(); } set { Write(value); CheckIsDirty("LastName"); } }

        public String PhoneNumber { get { return Read<string>(); } set { Write(value); CheckIsDirty("PhoneNumber"); } }

        public String PathToImage { get { return Read<string>(); } set { Write(value); CheckIsDirty("PathToImage"); } }

        public String Email { get { return Read<string>(); } set { Write(value); CheckIsDirty("Email"); } }

        public Int64? IsFavorite { get { return Read<Int64?>(); } set { Write(value); CheckIsDirty("IsFavorite"); } }

        public Int64? GroupID { get { return Read<Int64?>(); } set { Write(value); CheckIsDirty("GroupID"); } }

        /// <summary>
        /// Проверяет есть ли различия между временным контактом и первоночальным
        /// </summary>
        private void CheckIsDirty(string propertyName)
        {
            bool isDirty;
            //if (Current.Equals(originalContact))
            //    isDirty = false;
            //else
            //    isDirty = true;
            switch (propertyName)
            {
                case "FirstName":
                    if (originalContact.FirstName == FirstName) isDirty = false;
                    else isDirty = true;
                    break;
                case "LastName":
                    if (originalContact.LastName == LastName) isDirty = false;
                    else isDirty = true;
                    break;
                case "PhoneNumber":
                    if (originalContact.PhoneNumber == PhoneNumber) isDirty = false;
                    else isDirty = true;
                    break;
                case "PathToImage":
                    if (originalContact.PathToImage == PathToImage) isDirty = false;
                    else isDirty = true;
                    break;
                case "Email":
                    if (originalContact.Email == Email) isDirty = false;
                    else isDirty = true;
                    break;
                case "IsFavorite":
                    if (originalContact.IsFavorite == IsFavorite) isDirty = false;
                    else isDirty = true;
                    break;
                case "GroupID":
                    if (originalContact.GroupID == GroupID) isDirty = false;
                    else isDirty = true;
                    break;
                default:
                    isDirty = true;
                    break;
            }
            var message = new IsDirtyMessage() { IsDirty = isDirty };
            Messenger.Default.Send<IsDirtyMessage>(message);
        }

        #region Calculated properties
        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }
        #endregion
    }
}
