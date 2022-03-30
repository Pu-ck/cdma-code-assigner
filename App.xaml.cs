using System.Collections.Generic;
using System.Windows;

namespace CDMACodeAssigner
{
    public partial class App : Application
    {
    }

    public class User
    {
        public List<int> Sequence = new List<int>();

        public string Name { get; set; }
        public int SF { get; set; }

        public string UserData
        {
            get
            {
                return $"User { Name } SF = { SF }";
            }
        }
    }

    public class Sequence
    {
        public List<int> Values = new List<int>();
        public bool IsBlocked = false;
        public int Length;
    }
}
