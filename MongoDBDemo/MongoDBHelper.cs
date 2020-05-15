using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBDemo
{
    class MongoDBHelper
    {
        private MongoClient client;
        public MongoDBHelper(string serverAddr, string portNum)
        {
            string connectString = "mongodb://" + serverAddr + ":" + portNum;
            client = new MongoClient(connectString);
        }

        public void ListDataBases()
        {
            var dbList = client.ListDatabases().ToList();
            Console.WriteLine("Following shows the dbs in List:");
            foreach(var db in dbList)
            {
                Console.WriteLine(db);
            }
        }

        public IMongoDatabase GetDatabase(string dbName)
        {
            return client.GetDatabase(dbName);
        }


    }
}
