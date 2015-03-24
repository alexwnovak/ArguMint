using System;

namespace ArguMint.UnitTests.Dummies
{
   internal class DummyAttributePropertyNoSetter : Attribute
   {
      private string _stringProperty;

      public string StringProperty
      {
         get
         {
            return _stringProperty;
         }
      }
   }
}
