using System;
using System.Reflection;
using FluentAssertions;
using ArguMint.TestCommon.Dynamic;
using ArguMint.TestCommon.Helpers;
using Xunit;

namespace ArguMint.IntegrationTests
{
   public class ArgumentAnalyzerTests
   {
      [Fact]
      public void Analyze_ClassHasFirstArgumentAttribute_MapsArgument()
      {
         const string propertyName = "FileName";
         const string fileName = "SomeFileName.txt";

         // Arrange

         var argumentClass = ClassBuilder.Create();
         argumentClass.AddProperty<string>( propertyName );
         argumentClass.MarkProperty( propertyName, () => new ArgumentAttribute
         {
            Position = ArgumentPosition.First
         } );
         argumentClass.Build();

         // Act

         var stringArgs = ArrayHelper.Create( fileName );

         var arguments = ArgumentAnalyzerHelper.Analyze<ArgumentAnalyzer>( argumentClass.Type, stringArgs );

         // Assert

         arguments.Property( propertyName ).Should().Be( fileName );
      }

      [Fact]
      public void Analyze_ClassHasFirstArgumentAttributeButNotTheArgument_DoesNotSet()
      {
         const string propertyNameOne = "SourceFileName";
         const string propertyNameTwo = "DestinationFileName";
         const string firstArgument = "Source.txt";
         const string secondArgument = "Destination.txt";

         // Arrange

         var argumentClass = ClassBuilder.Create();
         argumentClass.AddProperty<string>( propertyNameOne );
         argumentClass.MarkProperty( propertyNameOne, () => new ArgumentAttribute
         {
            Position = ArgumentPosition.First
         } );
         argumentClass.AddProperty<string>( propertyNameTwo );
         argumentClass.MarkProperty( propertyNameTwo, () => new ArgumentAttribute
         {
            Position = ArgumentPosition.Second
         } );
         argumentClass.Build();

         // Act

         var stringArgs = ArrayHelper.Create( firstArgument, secondArgument );

         var arguments = ArgumentAnalyzerHelper.Analyze<ArgumentAnalyzer>( argumentClass.Type, stringArgs );

         // Assert

         arguments.Property( propertyNameOne ).Should().Be( firstArgument );
         arguments.Property( propertyNameTwo ).Should().Be( secondArgument );
      }

      [Fact]
      public void Analyze_HasSecondPositionAttributeAndOneArgument_DoesNotSet()
      {
         const string propertyName = "SomeArgument";

         // Arrange

         var argumentClass = ClassBuilder.Create();
         argumentClass.AddProperty<string>( propertyName );
         argumentClass.MarkProperty( propertyName, () => new ArgumentAttribute
         {
            Position = ArgumentPosition.Second
         } );
         argumentClass.Build();

         // Act

         var stringArgs = ArrayHelper.Create( "oneargumentbutnottwo" );

         var arguments = ArgumentAnalyzerHelper.Analyze<ArgumentAnalyzer>( argumentClass.Type, stringArgs );

         // Assert

         arguments.Property( propertyName ).Should().BeNull();
      }

      [Fact]
      public void Analyze_HasPrefixPropertyAndOneMatch_SetsValue()
      {
         const string propertyName = "FileName";
         const string fileName = "FileName.txt";

         // Arrange

         var argumentClass = ClassBuilder.Create();
         argumentClass.AddProperty<string>( propertyName );
         argumentClass.MarkProperty( propertyName, () => new ArgumentAttribute( "/f:", Spacing.None ) );
         argumentClass.Build();

         // Act

         var stringArgs = ArrayHelper.Create( $"/f:{fileName}" );

         var arguments = ArgumentAnalyzerHelper.Analyze<ArgumentAnalyzer>( argumentClass.Type, stringArgs );

         // Assert

         arguments.Property( propertyName ).Should().Be( fileName );
      }

      [Fact]
      public void Analyze_HasPrefixPropertyWithSpaceAndOneMatch_SetsValue()
      {
         const string propertyName = "FileName";
         const string fileName = "Video.mp4";

         // Arrange

         var argumentClass = ClassBuilder.Create();
         argumentClass.AddProperty<string>( propertyName );
         argumentClass.MarkProperty( propertyName, () => new ArgumentAttribute( "-filename", Spacing.Postfix ) );
         argumentClass.Build();

         // Act

         var stringArgs = ArrayHelper.Create( "-filename", fileName );

         var arguments = ArgumentAnalyzerHelper.Analyze<ArgumentAnalyzer>( argumentClass.Type, stringArgs );

         // Assert

         arguments.Property( propertyName ).Should().Be( fileName );
      }

      [Fact]
      public void Analyze_MatchesIntegerArgument_ArgumentTypeAndValueAreConverted()
      {
         const string propertyName = "MaxSize";
         const int maxSize = 123456;

         // Arrange

         var argumentClass = ClassBuilder.Create();
         argumentClass.AddProperty<int>( propertyName );
         argumentClass.MarkProperty( propertyName, () => new ArgumentAttribute
         {
            Position = ArgumentPosition.First
         } );
         argumentClass.Build();

         // Act

         var stringArgs = ArrayHelper.Create( maxSize.ToString() );

         var arguments = ArgumentAnalyzerHelper.Analyze<ArgumentAnalyzer>( argumentClass.Type, stringArgs );

         // Assert

         arguments.Property( propertyName ).Should().Be( maxSize );
      }

      [Fact]
      public void Analyze_MatchesCharArgument_ArgumentTypeAndValueAreConverted()
      {
         const string propertyName = "CharValue";
         const char charValue = 'C';

         // Arrange

         var argumentClass = ClassBuilder.Create();
         argumentClass.AddProperty<char>( propertyName );
         argumentClass.MarkProperty( propertyName, () => new ArgumentAttribute
         {
            Position = ArgumentPosition.First
         } );
         argumentClass.Build();

         // Act

         var stringArgs = ArrayHelper.Create( charValue.ToString() );

         var arguments = ArgumentAnalyzerHelper.Analyze<ArgumentAnalyzer>( argumentClass.Type, stringArgs );

         // Assert

         arguments.Property( propertyName ).Should().Be( charValue );
      }

      [Fact]
      public void Analyze_MethodHasOmittedHandlerAndNoArguments_HandlerIsCalled()
      {
         const string methodName = "ArgumentsOmittedHandler";
         bool wasCalled = false;

         // Arrange

         var argumentsClass = ClassBuilder.Create();
         argumentsClass.AddMethod( methodName,
            MethodAttributes.Public,
            typeof( void ),
            Type.EmptyTypes,
            () => { wasCalled = true; } );
         argumentsClass.MarkMethod( methodName, () => new ArgumentsOmittedHandlerAttribute() );
         argumentsClass.Build();

         // Act

         ArgumentAnalyzerHelper.Analyze<ArgumentAnalyzer>( argumentsClass.Type, new string[0] );

         // Assert

         wasCalled.Should().BeTrue();
      }
   }
}
