namespace ArguMint.UnitTests.Dummies
{
   public class ClassWithTwoPositionalArguments
   {
      [Argument( Position = ArgumentPosition.First )]
      public string SourceFileName
      {
         get;
         set;
      }

      [Argument( Position = ArgumentPosition.Second )]
      public string DestinationFileName
      {
         get;
         set;
      }
   }
}
