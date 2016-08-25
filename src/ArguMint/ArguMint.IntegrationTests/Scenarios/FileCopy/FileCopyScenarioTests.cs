using ArguMint.TestCommon.Helpers;
using FluentAssertions;

namespace ArguMint.IntegrationTests.Scenarios.FileCopy
{
   public class FileCopyScenarioTests
   {
      public void FileCopyScenario_ContainsTheTwoArguments_MatchesArguments()
      {
         const string sourceFile = @"C:\Temp\Source.txt";
         const string destinationFile = @"C:\Temp\Destination.txt";
         var stringArgs = ArrayHelper.Create( sourceFile, destinationFile );

         // Act

         var argumentAnalyzer = new ArgumentAnalyzer();

         var argumentClass = argumentAnalyzer.Analyze<FileCopyArguments>( stringArgs );

         // Asserts

         argumentClass.SourceFile.Should().Be( sourceFile );
         argumentClass.DestinationFile.Should().Be( destinationFile );
      }

      public void FileCopyScenario_DestinationFileIsOmitted_NotifiesWithArgumentMissing()
      {
         const string sourceFile = "Source.bmp";
         var stringArgs = ArrayHelper.Create( sourceFile );

         // Act

         var argumentAnalyzer = new ArgumentAnalyzer();

         var argumentClass = argumentAnalyzer.Analyze<FileCopyArguments>( stringArgs );

         // Assert

         argumentClass.ArgumentErrorType.Should().Be( ArgumentErrorType.ArgumentMissing );
      }

      public void FileCopyScenario_ContainsOptionalForceParameter_MatchesArguments()
      {
         const string sourceFile = @"C:\Temp\Source.txt";
         const string destinationFile = @"C:\Temp\Destination.txt";
         var stringArgs = ArrayHelper.Create( sourceFile, destinationFile, "/force" );

         // Act

         var argumentAnalyzer = new ArgumentAnalyzer();

         var argumentClass = argumentAnalyzer.Analyze<FileCopyArguments>( stringArgs );

         // Asserts

         argumentClass.SourceFile.Should().Be( sourceFile );
         argumentClass.DestinationFile.Should().Be( destinationFile );
         argumentClass.ForceCopy.Should().BeTrue();
      }
   }
}
