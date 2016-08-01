using System;

namespace ArguMint.UnitTests.Dummies
{
   internal class DummyAttributePropertyNoSetter : Attribute
   {
      public string StringProperty
      {
         get;
      }
   }
}
