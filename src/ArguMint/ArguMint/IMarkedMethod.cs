using System;

namespace ArguMint
{
   internal interface IMarkedMethod<out T> where T : Attribute
   {
      T Attribute
      {
         get;
      }

      string Name
      {
         get;
      }

      void Invoke( object instance );
   }
}
