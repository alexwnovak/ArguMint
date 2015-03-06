using System;

namespace Blargument.UnitTests.Dummies
{
   internal class ClassWithTwoAttributesOneMarkedAsObsolete
   {
      public string TheString
      {
         get;
         set;
      }

      [Obsolete]
      public int TheInt
      {
         get;
         set;
      }
   }
}
