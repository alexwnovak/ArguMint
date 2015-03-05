using System;
using System.Reflection;

namespace Blargument
{
   internal class MarkedProperty<T> where T: Attribute
   {
      private readonly PropertyInfo _propertyInfo;
      public PropertyInfo PropertyInfo
      {
         get
         {
            return _propertyInfo;
         }
      }

      private readonly T _attribute;
      public T Attribute
      {
         get
         {
            return _attribute;
         }
      }

      public MarkedProperty( PropertyInfo propertyInfo, T attribute )
      {
         _propertyInfo = propertyInfo;
         _attribute = attribute;
      }
   }
}
