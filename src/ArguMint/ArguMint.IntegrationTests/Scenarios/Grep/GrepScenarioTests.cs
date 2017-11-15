using FluentAssertions;
using Xunit;
using ArguMint.TestCommon.Helpers;

namespace ArguMint.IntegrationTests.Scenarios.Grep
{
   public class GrepScenarioTests
   {
      [Fact]
      public void GrepScenario_HasFileName_MatchesFileName()
      {
         const string fileName = @"C:\Temp\Document.txt";
         var stringArgs = ArrayHelper.Create( fileName );

         // Act

         var argumentClass = ArgumentAnalyzer.Analyze<GrepArguments>( stringArgs );

         // Assert

         argumentClass.FileName.Should().Be( fileName );
         argumentClass.IgnoreCase.Should().BeFalse();
      }

      [Fact]
      public void GrepScenario_HasFileNameAndIgnoreCaseFlag_MatchesInOrder()
      {
         const string ignoreCase = "--ignore-case";
         const string fileName = @"C:\Temp\Document.txt";
         var stringArgs = ArrayHelper.Create( ignoreCase, fileName );

         // Act

         var argumentClass = ArgumentAnalyzer.Analyze<GrepArguments>( stringArgs );

         // Assert

         argumentClass.FileName.Should().Be( fileName );
         argumentClass.IgnoreCase.Should().BeTrue();
      }

      [Fact]
      public void GrepScenario_HasFileNameAndIgnoreCaseFlagInReverseOrder_()
      {
         const string ignoreCase = "--ignore-case";
         const string fileName = @"C:\Temp\Document.txt";
         var stringArgs = ArrayHelper.Create( fileName, ignoreCase );

         // Act

         var argumentClass = ArgumentAnalyzer.Analyze<GrepArguments>( stringArgs );

         // Assert

         argumentClass.FileName.Should().Be( ignoreCase );
         argumentClass.IgnoreCase.Should().BeFalse();
      }
   }
}
