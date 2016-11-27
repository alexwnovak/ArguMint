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

      [Fact]
      public void TailScenario_SpecifiesFileNameButNotBytes_BytesAreNull()
      {
         const string fileName = @"C:\Temp\Document.txt";
         var stringArgs = ArrayHelper.Create( fileName );

         // Act

         var argumentClass = ArgumentAnalyzer.Analyze<TailArguments>( stringArgs );

         // Asserts

         argumentClass.Bytes.Should().Be( null );
      }

      [Fact]
      public void TailScenario_SpecifiesBytesArgument_MatchesBytes()
      {
         const string fileName = @"C:\Temp\Document.txt";
         const int bytes = 1234;
         var stringArgs = ArrayHelper.Create( $"--bytes={bytes}", fileName );

         // Act

         var argumentClass = ArgumentAnalyzer.Analyze<TailArguments>( stringArgs );

         // Asserts

         argumentClass.FileName.Should().Be( fileName );
         argumentClass.Bytes.Should().Be( bytes );
      }

      [Fact]
      public void TailScenario_OnlyHasBytesButNotFileName_MatchesBytesAsFileName()
      {
         const int bytes = 1234;
         string bytesArg = $"--bytes={bytes}";
         var stringArgs = ArrayHelper.Create( bytesArg );

         // Act

         var argumentClass = ArgumentAnalyzer.Analyze<TailArguments>( stringArgs );

         // Asserts

         argumentClass.FileName.Should().Be( bytesArg );
         argumentClass.Bytes.Should().Be( null );
      }
   }
}
