using System;

namespace ArguMint
{
   internal interface ITypeInspector
   {
      IMarkedProperty<TAttribute>[] GetMarkedProperties<TType, TAttribute>() where TAttribute : Attribute;
      IMarkedMethod<TAttribute>[] GetMarkedMethods<TType, TAttribute>() where TAttribute : Attribute;
   }
}
