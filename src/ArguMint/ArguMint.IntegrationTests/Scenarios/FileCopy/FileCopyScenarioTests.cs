using ArguMint.TestCommon.Helpers;
using FluentAssertions;
using Xunit;

namespace ArguMint.IntegrationTests.Scenarios.FileCopy
{
   public class FileCopyScenarioTests
   {
      [Fact]
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

      [Fact]
      public void FileCopyScenario_DestinationFileIsOmitted_NotifiesWithArgumentMissing()
      {
         const string sourceFile = "Source.bmp";
         var stringArgs = ArrayHelper.Create( sourceFile );

         // Act

         var argumentAnalyzer = new ArgumentAnalyzer();

         var argumentClass = argumentAnalyzer.Analyze<FileCopyArguments>( stringArgs );

         // Assert

         argumentClass.ArgumentError.ErrorType.Should().Be( ArgumentErrorType.ArgumentMissing );
         argumentClass.ArgumentError.Properties["PropertyName"].Should().Be( nameof( FileCopyArguments.DestinationFile ) );
      }

      [Fact]
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
