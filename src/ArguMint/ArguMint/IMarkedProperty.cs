using System;

namespace ArguMint
{
   internal interface IMarkedProperty<out T> where T : Attribute
   {
      T Attribute
      {
         get;
      }

      string PropertyName
      {
         get;
      }

      void SetProperty( object instance, object value );
   }
}
