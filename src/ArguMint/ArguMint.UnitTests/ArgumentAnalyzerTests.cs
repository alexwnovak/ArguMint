using System;
using Moq;
using FluentAssertions;
using ArguMint.TestCommon.Dynamic;
using ArguMint.UnitTests.Dummies;
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
   }
}
