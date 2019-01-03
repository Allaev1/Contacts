using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Contacts.Models
{
    /// <summary>
    /// Menus` item 
    /// </summary>
    public class MenuItem
    {
        public object Content;
        /// <summary>
        /// Symbol that shown next to content
        /// </summary>
        public Symbol Symbol;
        /// <summary>
        /// Page to that make navigation after item was tapped
        /// </summary>
        public Type PageType; 
    }
}
