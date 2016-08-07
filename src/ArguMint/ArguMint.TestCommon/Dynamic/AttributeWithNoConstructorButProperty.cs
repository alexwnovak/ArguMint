using System;

namespace ArguMint.TestCommon.Dynamic
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
