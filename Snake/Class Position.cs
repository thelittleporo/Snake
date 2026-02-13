using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Snake
{
    //https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.contains?view=net-10.0


    public class Position : IEquatable<Position>
    {
        public int x = 0;
        public int y = 0;     

        public bool Equals(Position? other)
        {
            if (this.x == other.x && this.y == other.y) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
