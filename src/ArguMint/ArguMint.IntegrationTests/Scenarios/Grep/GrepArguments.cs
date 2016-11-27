namespace ArguMint.IntegrationTests.Scenarios.Grep
{
   public class GrepArguments
   {
      [Argument( "--ignore-case" )]
      public bool IgnoreCase
      {
         get;
         set;
      }

      [Argument( Position = ArgumentPosition.Last )]
      public string FileName
      {
         get;
         set;
      }
   }
}
