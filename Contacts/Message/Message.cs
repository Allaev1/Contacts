using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace Contacts.Message
{
    public enum CRUD { Add, Read, Edit, Delete }

    public class OperationResultMessage : MessageBase
    {
        public CRUD Operation;
    }
}
