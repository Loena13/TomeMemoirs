using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomeMemoirs.Model
{
    public class BookUser
    {
        public int Id { get; set; }
        public User User { get; set; } //Refers to User model
        public int UserId { get; set; } //Uses reference to get the User Id
        public Book Book { get; set; } //Refers to Book model
        public int BookId { get; set; } //Uses reference to get Book id
        public int Rating { get; set; }
        public string Image { get; set; }
    }
}
