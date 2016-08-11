using System;

namespace ArguMint.UnitTests.Dummies
{
   public class ClassWithOneMarkedPrivateStaticMethod
   {
      [Obsolete]
      private static void PrivateStaticMethod()
      {
      }
   }
}
