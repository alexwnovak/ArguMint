using System;

namespace ArguMint.UnitTests.Dummies
{
   internal class DummyAttributePropertyNoGetter : Attribute
   {
      public string StringProperty
      {
         set
         {
         }
      }
   }
}
