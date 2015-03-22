using System;

namespace Blargument
{
   internal interface ITypeInspector
   {
      IMarkedProperty<TAttribute>[] GetMarkedProperties<TType, TAttribute>() where TAttribute: Attribute;
   }
}
