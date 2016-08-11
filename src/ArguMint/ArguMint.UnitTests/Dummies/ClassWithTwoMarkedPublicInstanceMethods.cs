using System;

namespace ArguMint.UnitTests.Dummies
{
   public class ClassWithTwoMarkedPublicInstanceMethods
   {
      [Obsolete]
      public void MethodOne()
      {
      }

      [Obsolete]
      public void MethodTwo()
      {
      }
   }
}
