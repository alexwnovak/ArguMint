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
   }
}
