using System;
using Moq;
using FluentAssertions;
using ArguMint.TestCommon.Helpers;
using ArguMint.UnitTests.Helpers;
using Xunit;

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
   }
}
