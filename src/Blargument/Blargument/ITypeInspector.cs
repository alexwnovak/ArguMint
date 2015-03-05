using System;

namespace Blargument
{
   internal interface ITypeInspector
   {
      MarkedProperty<TAttribute>[] GetMarkedProperties<TType, TAttribute>() where TAttribute: Attribute;
   }
}
