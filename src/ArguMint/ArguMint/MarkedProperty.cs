using System;
using System.Reflection;

namespace ArguMint
{
   internal class MarkedProperty<T> : IMarkedProperty<T> where T : Attribute
   {
      public T Attribute
      {
         get;
      }

      private readonly PropertyInfo _propertyInfo;
      public string PropertyName => _propertyInfo.Name;
      public Type PropertyType => _propertyInfo.PropertyType;

      public MarkedProperty( PropertyInfo propertyInfo, T attribute )
      {
         if ( propertyInfo == null )
         {
            throw new ArgumentNullException( nameof( propertyInfo ), "PropertyInfo instance must not be null" );
         }

         if ( attribute == null )
         {
            throw new ArgumentNullException( nameof( attribute ), "Attribute instance must not be null" );
         }

         if ( !propertyInfo.CanRead || !propertyInfo.CanWrite )
         {
            throw new ArgumentException( "Properties must have getters and setters" );
         }

         _propertyInfo = propertyInfo;
         Attribute = attribute;
      }

      public void SetPropertyValue( object instance, object value )
      {
         _propertyInfo.SetValue( instance, value, null );
      }
   }
}
