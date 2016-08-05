using System;

namespace ArguMint.UnitTests.Dynamic
{
   public class AttributeWithConstructorParameterAndProperty : Attribute
   {
      public int IntegerValue
      {
         get;
      }

      public bool BooleanValue
      {
         get;
         set;
      }

      public AttributeWithConstructorParameterAndProperty( int integerValue )
      {
         IntegerValue = integerValue;
      }
   }
}
