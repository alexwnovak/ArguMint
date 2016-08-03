namespace ArguMint.UnitTests.Dummies
{
   public class SecondPositionArgumentOnly
   {
      [Argument( Position = ArgumentPosition.Second )]
      public string SomeArgument
      {
         get;
         set;
      }
   }
}
