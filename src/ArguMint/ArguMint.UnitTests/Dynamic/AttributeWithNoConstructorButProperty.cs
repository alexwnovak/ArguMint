using System;

namespace ArguMint.UnitTests.Dynamic
{
   public class AttributeWithNoConstructorButProperty : Attribute
   {
      public char CharValue
      {
         get;
         set;
      }
   }
}
