using System;
using Moq;
using FluentAssertions;
using Xunit;
using ArguMint.UnitTests.Helpers;

namespace ArguMint.UnitTests
{
   public class PositionalRuleTests
   {
      [Fact]
      public void Match_DoesNotSpecifyPosition_DoesNotSetProperty()
      {
         object argumentClass = "ThisDoesNotMatter";
         var tokens = new ArgumentToken[0];

         // Arrange

         var markedPropertyMock = new Mock<IMarkedProperty<ArgumentAttribute>>();
         markedPropertyMock.SetupGet( mp => mp.Attribute ).Returns( new ArgumentAttribute() );

         // Act

         var positionalRule = new PositionalRule();

         positionalRule.Match( argumentClass, markedPropertyMock.Object, tokens );

         // Assert

         markedPropertyMock.Verify( mp => mp.SetPropertyValue( argumentClass, It.IsAny<object>() ), Times.Never() );
      }

      [Fact]
      public void Match_StringArgumentInFirstPosition_ReceivesValue()
      {
         object argumentClass = "ThisDoesNotMatter";
         var argumentAttribute = new ArgumentAttribute
         {
            Position = ArgumentPosition.First
         };

         // Arrange

         var markedPropertyMock = new Mock<IMarkedProperty<ArgumentAttribute>>();
         markedPropertyMock.SetupGet( mp => mp.Attribute ).Returns( argumentAttribute );
         markedPropertyMock.SetupGet( mp => mp.PropertyType ).Returns( typeof( string ) );

         // Act

         string argument = "OneParameterButNotTwo";

         var positionalRule = new PositionalRule();

         positionalRule.Match( argumentClass, markedPropertyMock.Object, TokenHelper.CreateArray( argument ) );

         // Assert

         markedPropertyMock.Verify( mp => mp.SetPropertyValue( argumentClass, argument ), Times.Once() );
      }

      [Fact]
      public void Match_ArgumentDoesNotMatchPropertyType_ThrowsArgumentErrorException()
      {
         object argumentClass = "ThisDoesNotMatter";
         var argumentAttribute = new ArgumentAttribute
         {
            Position = ArgumentPosition.First
         };

         // Arrange

         var markedPropertyMock = new Mock<IMarkedProperty<ArgumentAttribute>>();
         markedPropertyMock.SetupGet( mp => mp.Attribute ).Returns( argumentAttribute );
         markedPropertyMock.SetupGet( mp => mp.PropertyType ).Returns( typeof( int ) );

         // Act

         string argument = "DoesNotConvertToAnInteger";

         var positionalRule = new PositionalRule();

         Action match = () => positionalRule.Match( argumentClass, markedPropertyMock.Object, TokenHelper.CreateArray( argument ) );

         // Assert

         match.ShouldThrow<ArgumentErrorException>().Where( e => e.ErrorType == ArgumentErrorType.TypeMismatch );
      }

      [Fact]
      public void Match_HasOneArgumentAndTriesToMatchInSecondPosition_ThrowsArgumentErrorException()
      {
         object argumentClass = "ThisDoesNotMatter";
         var argumentAttribute = new ArgumentAttribute
         {
            Position = ArgumentPosition.Second
         };

         // Arrange

         var markedPropertyMock = new Mock<IMarkedProperty<ArgumentAttribute>>();
         markedPropertyMock.SetupGet( mp => mp.Attribute ).Returns( argumentAttribute );
         markedPropertyMock.SetupGet( mp => mp.PropertyType ).Returns( typeof( int ) );

         // Act

         string argument = "OneArgument";

         var positionalRule = new PositionalRule();

         Action match = () => positionalRule.Match( argumentClass, markedPropertyMock.Object, TokenHelper.CreateArray( argument ) );

         // Assert

         match.ShouldThrow<ArgumentErrorException>();
      }

      [Fact]
      public void Match_HasOneArgumentInLastPosition_MatchesLastArgument()
      {
         object argumentClass = "ThisDoesNotMatter";
         var argumentAttribute = new ArgumentAttribute
         {
            Position = ArgumentPosition.Last
         };

         // Arrange

         var markedPropertyMock = new Mock<IMarkedProperty<ArgumentAttribute>>();
         markedPropertyMock.SetupGet( mp => mp.Attribute ).Returns( argumentAttribute );

         // Act

         string argument = "OneArgument";

         var positionalRule = new PositionalRule();

         positionalRule.Match( argumentClass, markedPropertyMock.Object, TokenHelper.CreateArray( argument ) );

         // Assert

         markedPropertyMock.Verify( mp => mp.SetPropertyValue( argumentClass, argument ), Times.Once() );
      }

      [Fact]
      public void Match_LookingForLastArgumentButAcceptsThree_MatchesLastArgument()
      {
         object argumentClass = "ThisDoesNotMatter";
         var argumentAttribute = new ArgumentAttribute
         {
            Position = ArgumentPosition.Last
         };

         // Arrange

         var markedPropertyMock = new Mock<IMarkedProperty<ArgumentAttribute>>();
         markedPropertyMock.SetupGet( mp => mp.Attribute ).Returns( argumentAttribute );

         // Act

         string argument1 = "First argument";
         string argument2 = "Middle argument";
         string argument3 = "Last argument";
         var stringArgs = TokenHelper.CreateArray( argument1, argument2, argument3 );

         var positionalRule = new PositionalRule();

         positionalRule.Match( argumentClass, markedPropertyMock.Object, stringArgs );

         // Assert

         markedPropertyMock.Verify( mp => mp.SetPropertyValue( argumentClass, argument3 ), Times.Once() );
      }
   }
}
