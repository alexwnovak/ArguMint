using System;

namespace ArguMint
{
   internal interface ITypeInspector
   {
      IMarkedProperty<TAttribute>[] GetMarkedProperties<TType, TAttribute>() where TAttribute : Attribute;
   }
}
