using System;
using System.Reflection;

namespace ArguMint
{
   internal class MarkedMethod<T> : IMarkedMethod<T> where T : Attribute
   {
      public T Attribute
      {
         get;
      }

      private readonly MethodInfo _methodInfo;
      public string Name => _methodInfo.Name;

      public MarkedMethod( MethodInfo methodInfo, T attribute )
      {
         Attribute = attribute;
         _methodInfo = methodInfo;
      }

      public void Invoke( object instance )
      {
         _methodInfo.Invoke( instance, new object[0] );
      }
   }
}
