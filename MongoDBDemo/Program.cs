using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB;
using MongoDB.Driver;
namespace MongoDBDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            MongoDBHelper client = new MongoDBHelper("localhost", "27017");
            client.ListDataBases();
            Console.ReadKey();
        }
    }
}
