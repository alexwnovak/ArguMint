namespace Blargument.UnitTests.Dummies
{
   internal class ClassWithArgumentText
   {
      [Argument( "/?" )]
      public bool HelpText
      {
         get;
         set;
      }
   }
}
