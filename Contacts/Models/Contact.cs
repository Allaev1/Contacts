using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.Models
{
    public class Contact
    {
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string Email { set; get; }
        public string Notes { set; get; }
        public Uri PathToImage { set; get; }
        public string ID { set; get; }
    }
}
