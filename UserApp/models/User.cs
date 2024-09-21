using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserApp.models
{
    internal class User: BasePerson
    {
        public override void DisplayUserRole()
        {
            Console.WriteLine($"User role: {UserRole}");
        }
    }
}
