using System.Collections.Generic;
using System.Windows;

namespace CDMACodeAssigner
{
    public partial class App : Application
    {
    }

    public class User
    {
        public List<int> sequence = new List<int>();

        public string name { get; set; }
        public int sf { get; set; }

        public string userData
        {
            get
            {
                return $"User { name } SF = { sf }";
            }
        }
    }

    public class Sequence
    {
        public List<int> values = new List<int>();
        public bool isBlocked = false;
        public int length;
    }
}
