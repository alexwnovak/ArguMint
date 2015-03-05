using System;

namespace Blargument.UnitTests.Dummies
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
