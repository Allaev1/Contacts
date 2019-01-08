//This code was generated by a tool.
//Changes to this file will be lost if the code is regenerated.
// See the blog post here for help on using the generated code: http://erikej.blogspot.dk/2014/10/database-first-with-sqlite-in-universal.html
using SQLite;
using System;

namespace Contacts.Models
{
    public class SQLiteDb
    {
        string _path;
        public SQLiteDb(string path)
        {
            _path = path;
        }
        
         public void Create()
        {
            using (SQLiteConnection db = new SQLiteConnection(_path))
            {
                db.CreateTable<Contacts>();
            }
        }
    }
    public partial class Contacts
    {
        [PrimaryKey]
        public String ID { get; set; }
        
        public String FirstName { get; set; }
        
        public String LastName { get; set; }
        
        public String PhoneNumber { get; set; }
        
        public String PathToImage { get; set; }
        
        public String Email { get; set; }
        
        public Int64? IsFavorite { get; set; }
        
        public Int64? GroupID { get; set; }
        
    }
    
}
