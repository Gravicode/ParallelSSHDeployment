using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rds.Web.Tools
{
   
    public class ObjectContainer
    {
        private static Dictionary<Type, object> MyContainers = new Dictionary<Type, object>();
        
        public static void Register<T>(T data)
        {
            MyContainers.Add(typeof(T), data);
        }

        public static T Get<T>()
        {
            return (T)MyContainers[typeof(T)];
        }
        
    }
}
