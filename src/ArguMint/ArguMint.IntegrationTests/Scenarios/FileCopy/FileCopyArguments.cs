namespace ArguMint.IntegrationTests.Scenarios.FileCopy
{
   public class FileCopyArguments
   {
      [Argument( Position = ArgumentPosition.First )]
      public string SourceFile
      {
         get;
         set;
      }

      [Argument( Position = ArgumentPosition.Second )]
      public string DestinationFile
      {
         get;
         set;
      }

      [Argument( "/force" )]
      public bool ForceCopy
      {
         get;
         set;
      }
   }
}
