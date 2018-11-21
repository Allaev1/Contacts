using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Contacts.Services.ContactsRepositoryService;
using Contacts.Models;

namespace Contacts.ViewModels
{
    public class MasterDetailPageViewModel : ViewModelBase
    {
        #region Fields
        IContactRepositoryService _contactRepository;
        List<Contact> _contacts;
        Contact _contact;
        #endregion

        #region Contructors
        public MasterDetailPageViewModel(IContactRepositoryService contactRepository)
        {
            _contactRepository = contactRepository;
            _contacts = contactRepository.GetAll();
        }
        #endregion

        #region Bindable properties
        public List<Contact> Contacts
        {
            get { return _contacts; }
            set { Set(ref _contacts, value); }
        }

        public Contact SelectedContact
        {
            get { return _contact; }
            set { Set(ref _contact, value); }
        }
        #endregion
    }
}
