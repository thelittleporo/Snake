using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    [Serializable()]
    public class HighScore 
    {
        public int Score { get; set; }
        public string PlayerName { get; set; }

    }
}
