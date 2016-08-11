using System;

namespace ArguMint.UnitTests.Dummies
{
   public class ClassWithOneMarkedPrivateInstanceMethod
   {
      [Obsolete]
      private void PrivateInstanceMethod()
      {
      }
   }
}
