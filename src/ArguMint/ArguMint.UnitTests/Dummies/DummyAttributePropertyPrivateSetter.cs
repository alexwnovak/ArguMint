using System;

namespace ArguMint.UnitTests.Dummies
{
   internal class DummyAttributePropertyPrivateSetter : Attribute
   {
      public string StringProperty
      {
         get;
         private set;
      }
   }
}
