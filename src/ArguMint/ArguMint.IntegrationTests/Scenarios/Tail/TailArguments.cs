namespace ArguMint.IntegrationTests.Scenarios.Tail
{
   public class TailArguments
   {
      [Argument( Position = ArgumentPosition.Last )]
      public string FileName
      {
         get;
         set;
      }
   }
}
