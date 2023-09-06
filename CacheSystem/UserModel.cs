using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheSystem
{
    public class UserModel
    {
        public UserModel()
        {

        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public UserModel(string name, string surname)
        {
            Name = name;
            LastName = surname; 
            Id=Guid.NewGuid();
        }
    }
}
