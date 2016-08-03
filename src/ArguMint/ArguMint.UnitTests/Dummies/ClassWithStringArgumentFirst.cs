namespace ArguMint.UnitTests.Dummies
{
   public class ClassWithStringArgumentFirst
   {
      [Argument( Position = ArgumentPosition.First )]
      public string FileName
      {
         get;
         set;
      }
   }
}
