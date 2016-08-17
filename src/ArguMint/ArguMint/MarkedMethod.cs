using System;
using System.Linq;
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

      public Type[] ParameterTypes
      {
         get;
      }

      public MarkedMethod( MethodInfo methodInfo, T attribute )
      {
         Attribute = attribute;
         _methodInfo = methodInfo;

         ParameterTypes = methodInfo.GetParameters().Select( p => p.ParameterType ).ToArray();
      }

      public void Invoke( object instance ) => Invoke( instance, new object[0] );

      public void Invoke( object instance, object[] parameters ) => _methodInfo.Invoke( instance, parameters );
   }
}
