using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.ProxyModels
{
    public class ProxyContact
    {
        public String FirstName { get; set; }

        public String LastName { get; set; }

        public String PhoneNumber { get; set; }

        public String PathToImage { get; set; }

        public String Email { get; set; }

        public Int64? IsFavorite { get; set; }

        public Int64? GroupID { get; set; }
    }
}
