using System;

namespace ArguMint
{
   internal interface ITypeInspector
   {
      IMarkedProperty<TAttribute>[] GetMarkedProperties<TAttribute>( Type hostType ) where TAttribute : Attribute;
      IMarkedMethod<TAttribute>[] GetMarkedMethods<TAttribute>( Type hostType ) where TAttribute : Attribute;
   }
}
