using System;
using FluentAssertions;
using Moq;
using ArguMint.TestCommon.Helpers;

namespace ArguMint.UnitTests
{
   public class PrefixRuleTests
   {
      public void Match_HasPrefixWithNoSpaceArgument_MatchesArgument()
      {
         object argumentClass = "DoesNotMatter";

         const string prefix = "/prefixNoSpace:";
         const string value = "ArgumentValue";
         var argumentAttribute = new ArgumentAttribute( prefix );

         // Arrange

         var markedPropertyMock = new Mock<IMarkedProperty<ArgumentAttribute>>();
         markedPropertyMock.SetupGet( mp => mp.Attribute ).Returns( argumentAttribute );
         markedPropertyMock.SetupGet( mp => mp.PropertyType ).Returns( typeof( string ) );

         // Act

         string argument = $"{prefix}{value}";

         var prefixRule = new PrefixRule();

         prefixRule.Match( argumentClass, markedPropertyMock.Object, ArrayHelper.Create( argument ) );

         // Assert

         markedPropertyMock.Verify( mp => mp.SetPropertyValue( argumentClass, value ), Times.Once() );
      }

      public void Match_HasOnlyPrefixWithoutArgumentValue_ThrowsArgumentErrorException()
      {
         object argumentClass = "DoesNotMatter";

         const string prefix = "/prefixNoSpace:";
         var argumentAttribute = new ArgumentAttribute( prefix );

         // Arrange

         var markedPropertyMock = new Mock<IMarkedProperty<ArgumentAttribute>>();
         markedPropertyMock.SetupGet( mp => mp.Attribute ).Returns( argumentAttribute );
         markedPropertyMock.SetupGet( mp => mp.PropertyType ).Returns( typeof( string ) );

         // Act

         var prefixRule = new PrefixRule();

         Action match = () => prefixRule.Match( argumentClass, markedPropertyMock.Object, ArrayHelper.Create( prefix ) );

         // Assert

         match.ShouldThrow<ArgumentErrorException>().Where( e => e.ErrorType == ArgumentErrorType.PrefixArgumentHasNoValue );
      }

      public void Match_PrefixHasSpace_SpaceIsIgnoredAndArgumentIsMatchedAnyway()
      {
         object argumentClass = "DoesNotMatter";

         const string prefix = "/prefixWithSpace: ";
         const string value = "ArgumentValue";

         var argumentAttribute = new ArgumentAttribute( prefix, Spacing.Postfix );

         // Arrange

         var markedPropertyMock = new Mock<IMarkedProperty<ArgumentAttribute>>();
         markedPropertyMock.SetupGet( mp => mp.Attribute ).Returns( argumentAttribute );
         markedPropertyMock.SetupGet( mp => mp.PropertyType ).Returns( typeof( string ) );

         // Act

         var prefixRule = new PrefixRule();

         prefixRule.Match( argumentClass, markedPropertyMock.Object, ArrayHelper.Create( prefix, value ) );

         // Assert

         markedPropertyMock.Verify( mp => mp.SetPropertyValue( argumentClass, value ), Times.Once() );
      }

      public void Match_HasPrefixWithSpaceButNoValue_ThrowsArgumentErrorException()
      {
         object argumentClass = "DoesNotMatter";

         const string prefix = "/prefixWithSpace:";

         var argumentAttribute = new ArgumentAttribute( prefix, Spacing.Postfix );

         // Arrange

         var markedPropertyMock = new Mock<IMarkedProperty<ArgumentAttribute>>();
         markedPropertyMock.SetupGet( mp => mp.Attribute ).Returns( argumentAttribute );
         markedPropertyMock.SetupGet( mp => mp.PropertyType ).Returns( typeof( string ) );

         // Act

         var prefixRule = new PrefixRule();

         Action match = () => prefixRule.Match( argumentClass, markedPropertyMock.Object, ArrayHelper.Create( prefix ) );

         // Assert

         match.ShouldThrow<ArgumentErrorException>().Where( e => e.ErrorType == ArgumentErrorType.PrefixArgumentHasNoValue );
      }
   }
}
