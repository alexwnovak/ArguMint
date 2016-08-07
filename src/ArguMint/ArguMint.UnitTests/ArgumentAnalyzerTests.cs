using System;
using Moq;
using FluentAssertions;
using ArguMint.UnitTests.Dummies;
using ArguMint.UnitTests.Dynamic;
using ArguMint.UnitTests.Helpers;

namespace ArguMint.UnitTests
{
   public class ArgumentAnalyzerTests
   {
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
         const string propertyName = "FileName";
         const string fileName = "SomeFileName.txt";

         // Arrange

         var argumentClass = ClassBuilder.Create();
         argumentClass.AddProperty<string>( propertyName );
         argumentClass.AddAttribute( propertyName, () => new ArgumentAttribute
         {
            Position = ArgumentPosition.First
         } );
         argumentClass.Build();

         // Act

         var stringArgs = ArrayHelper.Create( fileName );

         var argumentAnalyzer = new ArgumentAnalyzer();

         var arguments = ArgumentAnalyzerHelper.Analyze( argumentAnalyzer, argumentClass.Type, stringArgs );

         // Assert

         arguments.Property( propertyName ).Should().Be( fileName );
      }

      public void Analyze_ClassHasFirstArgumentAttributeButNotTheArgument_DoesNotSet()
      {
         const string propertyNameOne = "SourceFileName";
         const string propertyNameTwo = "DestinationFileName";
         const string firstArgument = "Source.txt";
         const string secondArgument = "Destination.txt";

         // Arrange

         var argumentClass = ClassBuilder.Create();
         argumentClass.AddProperty<string>( propertyNameOne );
         argumentClass.AddAttribute( propertyNameOne, () => new ArgumentAttribute
         {
            Position = ArgumentPosition.First
         } );
         argumentClass.AddProperty<string>( propertyNameTwo );
         argumentClass.AddAttribute( propertyNameTwo, () => new ArgumentAttribute
         {
            Position = ArgumentPosition.Second
         } );
         argumentClass.Build();

         // Act

         var stringArgs = ArrayHelper.Create( firstArgument, secondArgument );

         var argumentAnalyzer = new ArgumentAnalyzer();

         var arguments = ArgumentAnalyzerHelper.Analyze( argumentAnalyzer, argumentClass.Type, stringArgs );

         // Assert

         arguments.Property( propertyNameOne ).Should().Be( firstArgument );
         arguments.Property( propertyNameTwo ).Should().Be( secondArgument );
      }

      public void Analyze_HasSecondPositionAttributeAndOneArgument_DoesNotSet()
      {
         const string propertyName = "SomeArgument";

         // Arrange

         var argumentClass = ClassBuilder.Create();
         argumentClass.AddProperty<string>( propertyName );
         argumentClass.AddAttribute( propertyName, () => new ArgumentAttribute
         {
            Position = ArgumentPosition.Second
         } );
         argumentClass.Build();

         // Act

         var stringArgs = ArrayHelper.Create( "oneargumentbutnottwo" );

         var argumentAnalyzer = new ArgumentAnalyzer();

         var arguments = ArgumentAnalyzerHelper.Analyze( argumentAnalyzer, argumentClass.Type, stringArgs );

         // Assert

         arguments.Property( propertyName ).Should().BeNull();
      }

      public void Analyze_HasPrefixPropertyAndOneMatch_SetsValue()
      {
         const string propertyName = "FileName";
         const string fileName = "FileName.txt";

         // Arrange

         var argumentClass = ClassBuilder.Create();
         argumentClass.AddProperty<string>( propertyName );
         argumentClass.AddAttribute( propertyName, () => new ArgumentAttribute( "/f:", Spacing.None ) );
         argumentClass.Build();

         // Act

         var stringArgs = ArrayHelper.Create( $"/f:{fileName}" );

         var argumentAnalyzer = new ArgumentAnalyzer();

         var arguments = ArgumentAnalyzerHelper.Analyze( argumentAnalyzer, argumentClass.Type, stringArgs );

         // Assert

         arguments.Property( propertyName ).Should().Be( fileName );
      }

      public void Analyze_HasPrefixPropertyWithSpaceAndOneMatch_SetsValue()
      {
         const string propertyName = "FileName";
         const string fileName = "Video.mp4";

         // Arrange

         var argumentClass = ClassBuilder.Create();
         argumentClass.AddProperty<string>( propertyName );
         argumentClass.AddAttribute( propertyName, () => new ArgumentAttribute( "-filename", Spacing.Postfix ) );
         argumentClass.Build();

         // Act

         var stringArgs = ArrayHelper.Create( "-filename", fileName );

         var argumentAnalyzer = new ArgumentAnalyzer();

         var arguments = ArgumentAnalyzerHelper.Analyze( argumentAnalyzer, argumentClass.Type, stringArgs );

         // Assert

         arguments.Property( propertyName ).Should().Be( fileName );
      }
   }
}
