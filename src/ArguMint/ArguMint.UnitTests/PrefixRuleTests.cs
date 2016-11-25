using System;
using FluentAssertions;
using Moq;
using Xunit;
using ArguMint.UnitTests.Helpers;

namespace ArguMint.UnitTests
{
   public class PrefixRuleTests
   {
      [Fact]
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
         var tokens = TokenHelper.CreateArray( argument );

         var prefixRule = new PrefixRule();

         prefixRule.Match( argumentClass, markedPropertyMock.Object, tokens );

         // Assert

         markedPropertyMock.Verify( mp => mp.SetPropertyValue( argumentClass, value ), Times.Once() );
      }

      [Fact]
      public void Match_HasOnlyPrefixWithoutArgumentValue_ThrowsArgumentErrorException()
      {
         object argumentClass = "DoesNotMatter";

         const string propertyName = "TheProperty";
         const string prefix = "/prefixNoSpace:";
         var argumentAttribute = new ArgumentAttribute( prefix );

         // Arrange

         var markedPropertyMock = new Mock<IMarkedProperty<ArgumentAttribute>>();
         markedPropertyMock.SetupGet( mp => mp.Attribute ).Returns( argumentAttribute );
         markedPropertyMock.SetupGet( mp => mp.PropertyName ).Returns( propertyName );
         markedPropertyMock.SetupGet( mp => mp.PropertyType ).Returns( typeof( string ) );

         // Act

         var prefixRule = new PrefixRule();

         Action match = () => prefixRule.Match( argumentClass, markedPropertyMock.Object, TokenHelper.CreateArray( prefix ) );

         // Assert

         match.ShouldThrow<ArgumentErrorException>()
            .Where( e => e.ErrorType == ArgumentErrorType.PrefixArgumentHasNoValue &&
                    e.Properties["PropertyName"].ToString() == propertyName );
      }

      [Fact]
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

         prefixRule.Match( argumentClass, markedPropertyMock.Object, TokenHelper.CreateArray( prefix, value ) );

         // Assert

         markedPropertyMock.Verify( mp => mp.SetPropertyValue( argumentClass, value ), Times.Once() );
      }

      [Fact]
      public void Match_HasStringArgumentPrefixWithSpaceButNoValue_ThrowsArgumentErrorException()
      {
         object argumentClass = "DoesNotMatter";

         const string prefix = "/prefixWithSpace:";
         const string propertyName = "PrefixPropertyWithSpace";

         var argumentAttribute = new ArgumentAttribute( prefix, Spacing.Postfix );

         // Arrange

         var markedPropertyMock = new Mock<IMarkedProperty<ArgumentAttribute>>();
         markedPropertyMock.SetupGet( mp => mp.Attribute ).Returns( argumentAttribute );
         markedPropertyMock.SetupGet( mp => mp.PropertyName ).Returns( propertyName );
         markedPropertyMock.SetupGet( mp => mp.PropertyType ).Returns( typeof( string ) );

         // Act

         var prefixRule = new PrefixRule();

         Action match = () => prefixRule.Match( argumentClass, markedPropertyMock.Object, TokenHelper.CreateArray( prefix ) );

         // Assert

         match.ShouldThrow<ArgumentErrorException>()
            .Where( e => e.ErrorType == ArgumentErrorType.PrefixArgumentHasNoValue &&
                    e.Properties["PropertyName"].ToString() == propertyName );
      }

      [Fact]
      public void Match_HasBoolArgumentPrefixWithNoValue_SetsBooleanValue()
      {
         object argumentClass = "DoesNotMatter";

         const string prefix = "--force";

         var argumentAttribute = new ArgumentAttribute( prefix );

         // Arrange

         var markedPropertyMock = new Mock<IMarkedProperty<ArgumentAttribute>>();
         markedPropertyMock.SetupGet( mp => mp.Attribute ).Returns( argumentAttribute );
         markedPropertyMock.SetupGet( mp => mp.PropertyType ).Returns( typeof( bool ) );

         // Act

         var prefixRule = new PrefixRule();

         prefixRule.Match( argumentClass, markedPropertyMock.Object, TokenHelper.CreateArray( prefix ) );

         // Assert

         markedPropertyMock.Verify( mp => mp.SetPropertyValue( argumentClass, true ), Times.Once() );
      }

      [Fact]
      public void Match_PrefixNoSpaceHasInteger_ArgumentIsConvertedAndSet()
      {
         object argumentClass = "DoesNotMatter";

         const string prefix = "/prefixWithNoSpace:";
         const int value = 1234;

         var argumentAttribute = new ArgumentAttribute( prefix );

         // Arrange

         var markedPropertyMock = new Mock<IMarkedProperty<ArgumentAttribute>>();
         markedPropertyMock.SetupGet( mp => mp.Attribute ).Returns( argumentAttribute );
         markedPropertyMock.SetupGet( mp => mp.PropertyType ).Returns( typeof( int ) );

         // Act

         string argument = $"{prefix}{value}";

         var prefixRule = new PrefixRule();

         prefixRule.Match( argumentClass, markedPropertyMock.Object, TokenHelper.CreateArray( argument ) );

         // Assert

         markedPropertyMock.Verify( mp => mp.SetPropertyValue( argumentClass, value ), Times.Once() );
      }

      [Fact]
      public void Match_PrefixNoSpaceMatchesAgainstNonInteger_ThrowsArgumentErrorException()
      {
         object argumentClass = "DoesNotMatter";

         const string prefix = "/prefixWithNoSpace:";
         const string value = "NotAnInteger";

         var argumentAttribute = new ArgumentAttribute( prefix );

         // Arrange

         var markedPropertyMock = new Mock<IMarkedProperty<ArgumentAttribute>>();
         markedPropertyMock.SetupGet( mp => mp.Attribute ).Returns( argumentAttribute );
         markedPropertyMock.SetupGet( mp => mp.PropertyType ).Returns( typeof( int ) );

         // Act

         string argument = $"{prefix}{value}";

         var prefixRule = new PrefixRule();

         Action match = () => prefixRule.Match( argumentClass, markedPropertyMock.Object, TokenHelper.CreateArray( argument ) );

         // Assert

         match.ShouldThrow<ArgumentErrorException>().Where( e => e.ErrorType == ArgumentErrorType.TypeMismatch );
      }
   }
}
