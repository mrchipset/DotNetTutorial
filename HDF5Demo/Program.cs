using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace HDF5Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteAndReadArray instance = new WriteAndReadArray();
            List<double> x = new List<double>();
            List<double> y = new List<double>();
            //for (int i = 0; i < 4000; i++)
            //{
            //    x.Add(i);
            //    y.Add(i);
            //}
            //instance.WriteData("/", "test1", x, y);
            bool b = instance.ReadData("/", "test1", ref x, ref y);
            if (b)
            {
                for (int i = 0; i < 4000; i++)
                {
                    if (y[i].Equals(i) && x[i].Equals(i))
                    {
                        Console.WriteLine(i);
                    }
                }
            }
            Console.ReadKey();
        }
    }
}
