using System;

namespace ArguMint.UnitTests.Dummies
{
   internal class ClassWithOnePropertyMarkedAsObsolete
   {
      [Obsolete]
      public string StringProperty
      {
         get;
         set;
      }
   }
}
