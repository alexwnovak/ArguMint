using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ArguMint
{
   internal class TypeInspector : ITypeInspector
   {
      public IMarkedProperty<TAttribute>[] GetMarkedProperties<TAttribute>( Type hostType ) where TAttribute : Attribute
      {
         return (from p in hostType.GetProperties( BindingFlags.Public | BindingFlags.Instance )
                 let attributes = p.GetCustomAttributes( typeof( TAttribute ), false ).Cast<TAttribute>()
                 where attributes.Any()
                 select new MarkedProperty<TAttribute>( p, attributes.First() )).ToArray();
      }

      public IMarkedMethod<TAttribute>[] GetMarkedMethods<TAttribute>( Type hostType ) where TAttribute : Attribute
      {
         var markedMethods = new List<IMarkedMethod<TAttribute>>();
         var methods = hostType.GetMethods( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance );

         foreach ( var method in methods )
         {
            var attributes = method.GetCustomAttributes( typeof ( TAttribute ), false );
            var thisAttribute = attributes.SingleOrDefault( a => a is TAttribute );

            if ( thisAttribute != null )
            {
               var markedMethod = new MarkedMethod<TAttribute>( method, (TAttribute) thisAttribute  );
               markedMethods.Add( markedMethod );
            }
         }

         return markedMethods.ToArray();
      }
   }
}
