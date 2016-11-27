using FluentAssertions;
using Xunit;
using ArguMint.TestCommon.Helpers;

namespace ArguMint.IntegrationTests.Scenarios.Tail
{
   public class TailScenarioTests
   {
      [Fact]
      public void TailScenario_SpecifiesFileNameInLastPosition_MatchesLastArgumentWithNoOtherSwitches()
      {
         const string fileName = @"C:\Temp\Document.txt";
         var stringArgs = ArrayHelper.Create( fileName );

         // Act

         var argumentClass = ArgumentAnalyzer.Analyze<TailArguments>( stringArgs );

         // Assert

         argumentClass.FileName.Should().Be( fileName );
      }
   }
}
