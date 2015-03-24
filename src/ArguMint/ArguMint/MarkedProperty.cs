using System;
using System.Reflection;

namespace ArguMint
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
         if ( propertyInfo == null )
         {
            throw new ArgumentNullException( "propertyInfo", "PropertyInfo instance must not be null" );
         }

         if ( attribute == null )
         {
            throw new ArgumentNullException( "attribute", "Attribute instance must not be null" );
         }

         if ( !propertyInfo.CanWrite )
         {
            throw new ArgumentException( "Properties must have getters and setters" );
         }

         _propertyInfo = propertyInfo;
         _attribute = attribute;
      }

      public void SetProperty( object value )
      {
         _propertyInfo.SetValue( _attribute, value, null );
      }
   }
}
