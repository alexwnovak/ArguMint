using System;
using System.Reflection;

namespace Blargument
{
   internal class MarkedProperty<T> : IMarkedProperty<T> where T : Attribute
   {
      private readonly T _attribute;
      public T Attribute
      {
         get
         {
            return _attribute;
         }
      }

      private readonly PropertyInfo _propertyInfo;
      public string PropertyName
      {
         get
         {
            return _propertyInfo.Name;
         }
      }

      public MarkedProperty( PropertyInfo propertyInfo, T attribute )
      {
         _propertyInfo = propertyInfo;
         _attribute = attribute;
      }

      public void SetProperty( object instance, object value )
      {
         _propertyInfo.SetValue( instance, value, null );
      }
   }
}
