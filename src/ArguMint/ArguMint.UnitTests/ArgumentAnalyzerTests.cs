using System;
using Moq;
using FluentAssertions;
using Xunit;
using ArguMint.TestCommon.Dynamic;
using ArguMint.TestCommon.Helpers;
using ArguMint.UnitTests.Dummies;
using ArguMint.UnitTests.Helpers;

namespace ArguMint.UnitTests
{
   public class ArgumentAnalyzerTests
   {
      [Fact]
      public void Analyze_ArgumentsAreNull_ThrowsArgumentException()
      {
         // Arrange

         var argumentClass = ClassBuilder.Create();
         argumentClass.Build();

         // Act

         var argumentAnalyzer = new ArgumentAnalyzer();

         Action analyze = () => ArgumentAnalyzerHelper.Analyze( argumentAnalyzer, argumentClass.Type, null );

         analyze.ShouldThrow<ArgumentException>();
      }

      [Fact]
      public void Analyze_ArgumentArrayIsEmpty_CallsDispatchHandlerForArgumentsOmitted()
      {
         // Arrange

         var handlerDispatcherMock = new Mock<IHandlerDispatcher>();

         // Act

         var argumentAnalyzer = new ArgumentAnalyzer( handlerDispatcherMock.Object, null );

         argumentAnalyzer.Analyze<DontCare>( new string[0] );

         // Assert

         handlerDispatcherMock.Verify( hd => hd.DispatchArgumentsOmitted( It.IsAny<object>() ), Times.Once() );
      }

      [Fact]
      public void Analyze_HasArguments_CallsRuleMatcher()
      {
         var stringArgs = ArrayHelper.Create( "OneArg" );

         // Arrange

         var ruleMatcherMock = new Mock<IRuleMatcher>();

         // Act

         var argumentAnalyzer = new ArgumentAnalyzer( null, ruleMatcherMock.Object );

         argumentAnalyzer.Analyze<DontCare>( stringArgs );

         // Assert

         ruleMatcherMock.Verify( rm => rm.Match( It.IsAny<object>(), stringArgs ), Times.Once() );
      }

      [Fact]
      public void Analyze_RuleMatcherThrowsArgumentErrorException_CallsArgumentHandler()
      {
         var stringArgs = ArrayHelper.Create( "OneArg" );
         const ArgumentErrorType errorType = ArgumentErrorType.Unspecified;

         // Arrange

         var handlerDispatcherMock = new Mock<IHandlerDispatcher>();

         var ruleMatcherMock = new Mock<IRuleMatcher>();
         ruleMatcherMock.Setup( rm => rm.Match( It.IsAny<object>(), stringArgs ) ).Throws( new ArgumentErrorException( errorType, null ) );

         // Act

         var argumentAnalyzer = new ArgumentAnalyzer( handlerDispatcherMock.Object, ruleMatcherMock.Object );

         argumentAnalyzer.Analyze<DontCare>( stringArgs );

         // Assert

         handlerDispatcherMock.Verify( hd => hd.DispatchArgumentError( It.IsAny<object>(), It.Is<ArgumentError>( ae => ae.ErrorType == errorType ) ), Times.Once() );
      }
   }
}
