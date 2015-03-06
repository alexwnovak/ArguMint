using System;

namespace Blargument.UnitTests.Dummies
{
   internal class ClassWithTwoPropertiesMarkedObsolete
   {
      [Obsolete( "Property X" )]
      public double X
      {
         get;
         set;
      }

      [Obsolete( "Property Y" )]
      public double Y
      {
         get;
         set;
      }
   }
}
