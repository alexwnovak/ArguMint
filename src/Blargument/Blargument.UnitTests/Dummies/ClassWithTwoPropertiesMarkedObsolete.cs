using System;

namespace Blargument.UnitTests.Dummies
{
   internal class ClassWithTwoPropertiesMarkedObsolete
   {
      [Obsolete]
      public double X
      {
         get;
         set;
      }

      [Obsolete]
      public double Y
      {
         get;
         set;
      }
   }
}
