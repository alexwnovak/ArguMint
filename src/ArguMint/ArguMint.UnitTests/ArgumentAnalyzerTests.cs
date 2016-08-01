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

         argumentAnalyzer.Invoking( aa => aa.Analyze<DontCare>( null ) ).ShouldThrow<ArgumentException>();
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
         argumentAnalyzer.Invoking( aa => aa.Analyze<ClassWithNoAttributes>( arguments ) ).ShouldThrow<MissingAttributesException>();
      }

      public void Analyze_PassedArgumentThatMatchesAttribute_SetsTheDecoratedProperty()
      {
         var markedPropertyMock = MarkedPropertyHelper.Create( "/?" );
         var markedProperties = ArrayHelper.Create( markedPropertyMock.Object );
         var arguments = ArrayHelper.Create( "/?" );

         // Setup

         var typeInspectorMock = new Mock<ITypeInspector>();
         typeInspectorMock.Setup( ti => ti.GetMarkedProperties<ClassWithArgumentText, ArgumentAttribute>() ).Returns( markedProperties );

         // Test

         var argumentAnalyzer = new ArgumentAnalyzer( typeInspectorMock.Object );

         argumentAnalyzer.Analyze<ClassWithArgumentText>( arguments );

         markedPropertyMock.Verify( mp => mp.SetPropertyValue( true ), Times.Once() );
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

         markedPropertyMock.Verify( mp => mp.SetPropertyValue( true ), Times.Never() );
      }
   }
}
