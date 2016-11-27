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

      public bool ArgumentsOmitted
      {
         get;
         private set;
      }

      public ArgumentError ArgumentError
      {
         get;
         private set;
      }

      [ArgumentsOmittedHandler]
      public void ArgumentsOmittedHandler() => ArgumentsOmitted = true;

      [ArgumentErrorHandler]
      public void ArgumentHandler( ArgumentError argumentError ) => ArgumentError = argumentError;
   }
}
