using System;
using System.Linq;
using System.Reflection;

namespace Blargument
{
   internal class TypeInspector : ITypeInspector
   {
      public IMarkedProperty<TAttribute>[] GetMarkedProperties<TType, TAttribute>() where TAttribute : Attribute
      {
         return (from p in typeof( TType ).GetProperties( BindingFlags.Public | BindingFlags.Instance )
                 let attributes = p.GetCustomAttributes( typeof( TAttribute ), false ).Cast<TAttribute>()
                 where attributes.Any()
                 select new MarkedProperty<TAttribute>( p, attributes.First() )).ToArray();
      }
   }
}
