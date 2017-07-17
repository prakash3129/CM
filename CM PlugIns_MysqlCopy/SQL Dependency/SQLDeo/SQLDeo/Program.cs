using System;
using System.Collections.Generic;
using System.Text;
using ServiceBrokerListener.Domain;
using System.Xml;

namespace SQLDeo
{
    class Program
    {
        static void Main(string[] args)
        {
            var listener = new SqlDependencyEx("user id=mssuser;password=M3r!7db#uk$@9wd;data source=172.27.137.181;initial catalog=MVC;Application Name=Campaign Manager;", "MVC", "mytable", listenerType: SqlDependencyEx.NotificationTypes.Update);

            listener.TableChanged += (o, e) => Console.WriteLine("Your table was changed!:" + e.Data );

            //listener.Stop();

            

            listener.Start();



            Console.WriteLine("started");
            Console.ReadLine();
          //  listener.Stop();

        }
    }
}
