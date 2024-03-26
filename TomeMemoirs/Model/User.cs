using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomeMemoirs.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HashedPassWord { get; set; }
        public static User LoggedInUser { get; set; } //Refers to the current logged in user
    }
}
