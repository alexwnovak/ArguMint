using System;

namespace ArguMint.UnitTests.Dummies
{
   public class ClassWithOneMarkedPublicStaticMethod
   {
      [Obsolete]
      public static void StaticMethod()
      {
      }
   }
}
