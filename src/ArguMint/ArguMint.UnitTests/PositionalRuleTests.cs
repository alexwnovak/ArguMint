using System;
using Moq;
using FluentAssertions;
using ArguMint.TestCommon.Helpers;

namespace ArguMint.UnitTests
{
   public class PositionalRuleTests
   {
      public void Match_DoesNotSpecifyPosition_DoesNotSetProperty()
      {
         object argumentClass = "ThisDoesNotMatter";

         // Arrange

         var markedPropertyMock = new Mock<IMarkedProperty<ArgumentAttribute>>();
         markedPropertyMock.SetupGet( mp => mp.Attribute ).Returns( new ArgumentAttribute() );

         // Act

         var positionalRule = new PositionalRule();

         positionalRule.Match( argumentClass, markedPropertyMock.Object, null );

         // Assert

         markedPropertyMock.Verify( mp => mp.SetPropertyValue( argumentClass, It.IsAny<object>() ), Times.Never() );
      }

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

         positionalRule.Match( argumentClass, markedPropertyMock.Object, ArrayHelper.Create( argument ) );

         // Assert

         markedPropertyMock.Verify( mp => mp.SetPropertyValue( argumentClass, argument ), Times.Once() );
      }

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

         Action match = () => positionalRule.Match( argumentClass, markedPropertyMock.Object, ArrayHelper.Create( argument ) );

         // Assert

         match.ShouldThrow<ArgumentErrorException>();
      }

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

         Action match = () => positionalRule.Match( argumentClass, markedPropertyMock.Object, ArrayHelper.Create( argument ) );

         // Assert

         match.ShouldThrow<ArgumentErrorException>();
      }
   }
}
