using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DataLayerWrapper.Helper
{
    internal static class ReflectionHelper
    {
        public static Type GetClosedGenericType(object obj, Type openGenericType)
        {
            // If the object is null then just return null

            if (obj == null)
                return null;

            // Otherwise use reflection to get all interfaces implemented by the type

            IEnumerable<Type> implementedInterfaces = obj.GetType().GetTypeInfo().ImplementedInterfaces;

            // Return the first interface matching the specified open generic type (or null if this interface is not implemented)

            return implementedInterfaces.Where(i => i.IsConstructedGenericType && i.GetGenericTypeDefinition() == openGenericType)
                                        .FirstOrDefault();
        }
    }
}
