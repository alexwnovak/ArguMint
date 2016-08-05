using System;
using Moq;
using FluentAssertions;
using ArguMint.UnitTests.Dummies;
using ArguMint.UnitTests.Helpers;

namespace ArguMint.UnitTests
{
   public class ArgumentAnalyzerTests
   {
      public void Analyze_ArgumentsAreNull_ThrowsArgumentException()
      {
         var argumentAnalyzer = new ArgumentAnalyzer();

         Action analyze = () => argumentAnalyzer.Analyze<DontCare>( null );

         analyze.ShouldThrow<ArgumentException>();
      }

      public void Analyze_ArgumentArrayIsEmpty_DoesNotQueryForProperties()
      {
         // Setup

         var typeInspectorMock = new Mock<ITypeInspector>();

         // Test

         var argumentAnalyzer = new ArgumentAnalyzer( typeInspectorMock.Object );

         argumentAnalyzer.Analyze<DontCare>( new string[0] );

         // Assert

         typeInspectorMock.Verify( ti => ti.GetMarkedProperties<DontCare, ArgumentAttribute>(), Times.Never() );
      }

      public void Analyze_TypeNotDecoratedWithAnyAttributes_ThrowsMissingAttributesException()
      {
         var arguments = ArrayHelper.Create( "/?" );
         var markedProperties = new MarkedProperty<ArgumentAttribute>[0];

         // Setup

         var typeInspectorMock = new Mock<ITypeInspector>();
         typeInspectorMock.Setup( ti => ti.GetMarkedProperties<ClassWithNoAttributes, ArgumentAttribute>() ).Returns( markedProperties );

         // Test

         var argumentAnalyzer = new ArgumentAnalyzer( typeInspectorMock.Object );

         Action analyze = () => argumentAnalyzer.Analyze<ClassWithNoAttributes>( arguments );

         analyze.ShouldThrow<MissingAttributesException>();
      }

      public void Analyze_PassedArgumentThatDoesNotMatchAttribute_DoesNotSetTheDecoratedProperty()
      {
         var markedPropertyMock = MarkedPropertyHelper.Create( "/?" );
         var markedProperties = ArrayHelper.Create( markedPropertyMock.Object );
         var arguments = ArrayHelper.Create( "someArgument" );

         // Setup

         var typeInspectorMock = new Mock<ITypeInspector>();
         typeInspectorMock.Setup( ti => ti.GetMarkedProperties<ClassWithArgumentText, ArgumentAttribute>() ).Returns( markedProperties );

         // Test

         var argumentAnalyzer = new ArgumentAnalyzer( typeInspectorMock.Object );

         argumentAnalyzer.Analyze<ClassWithArgumentText>( arguments );

         markedPropertyMock.Verify( mp => mp.SetPropertyValue( It.IsAny<object>(), true ), Times.Never() );
      }

      public void Analyze_ClassHasFirstArgumentAttribute_MapsArgument()
      {
         const string fileName = "SomeFileName.txt";

         // Arrange

         var stringArgs = ArrayHelper.Create( fileName );

         // Act

         var argumentAnalyzer = new ArgumentAnalyzer();

         var arguments = argumentAnalyzer.Analyze<ClassWithStringArgumentFirst>( stringArgs );

         // Assert

         arguments.FileName.Should().Be( fileName );
      }

      public void Analyze_ClassHasFirstArgumentAttributeButNotTheArgument_DoesNotSet()
      {
         const string firstArgument = "Source.txt";
         const string secondArgument = "Destination.txt";

         // Arrange

         var stringArgs = ArrayHelper.Create( firstArgument, secondArgument );

         // Act

         var argumentAnalyzer = new ArgumentAnalyzer();

         var arguments = argumentAnalyzer.Analyze<ClassWithTwoPositionalArguments>( stringArgs );

         // Assert

         arguments.SourceFileName.Should().Be( firstArgument );
         arguments.DestinationFileName.Should().Be( secondArgument );
      }

      public void Analyze_HasSecondPositionAttributeAndOneArgument_DoesNotSet()
      {
         // Arrange

         var stringArgs = ArrayHelper.Create( "someargument" );

         // Act

         var argumentAnalyzer = new ArgumentAnalyzer();

         var arguments = argumentAnalyzer.Analyze<SecondPositionArgumentOnly>( stringArgs );

         // Assert

         arguments.SomeArgument.Should().BeNull();
      }

      public void Analyze_HasPrefixPropertyAndOneMatch_SetsValue()
      {
         const string fileName = "FileName.txt";

         // Arrange

         var stringArgs = ArrayHelper.Create( $"/f:{fileName}" );

         // Act

         var argumentAnalyzer = new ArgumentAnalyzer();

         var arguments = argumentAnalyzer.Analyze<PrefixStringArgumentWithoutSpace>( stringArgs );

         // Assert

         arguments.FileName.Should().Be( fileName );
      }
   }
}
