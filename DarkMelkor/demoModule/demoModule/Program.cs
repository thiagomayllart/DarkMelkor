using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace demoModule
{
    class Program
    {
        public static void doTheThing(string[] args)
        {
            Console.WriteLine("After Loader");
        }

        static void Main(string[] args)
        {
            doTheThing(args);
        }
    }
}
