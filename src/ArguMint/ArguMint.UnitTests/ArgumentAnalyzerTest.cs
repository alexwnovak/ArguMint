using System;
using ArguMint.UnitTests.Dummies;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ArguMint;

namespace ArguMint.UnitTests
{
   [TestClass]
   public class ArgumentAnalyzerTest
   {
      [TestInitialize]
      public void Initialize()
      {
         Dependency.AutoInitialize = false;
      }

      [TestMethod]
      [ExpectedException( typeof( ArgumentException ) )]
      public void Analyze_ArgumentsAreNull_ThrowsArgumentException()
      {
         ArgumentAnalyzer.Analyze<DontCare>( null );
      }

      [TestMethod]
      public void Analyze_ArgumentArrayIsEmpty_DoesNotQueryForProperties()
      {
         // Setup

         var typeInspectorMock = new Mock<ITypeInspector>();
         Dependency.UnityContainer.RegisterInstance( typeInspectorMock.Object );

         // Test

         ArgumentAnalyzer.Analyze<DontCare>( new string[0] );

         // Assert

         typeInspectorMock.Verify( ti => ti.GetMarkedProperties<DontCare, ArgumentAttribute>(), Times.Never() );
      }

      [TestMethod]
      [ExpectedException( typeof( MissingAttributesException ) )]
      public void Analyze_TypeNotDecoratedWithAnyAttributes_ThrowsMissingAttributesException()
      {
         var arguments = ArrayHelper.Create( "/?" );
         var markedProperties = new MarkedProperty<ArgumentAttribute>[0];

         // Setup

         var typeInspectorMock = new Mock<ITypeInspector>();
         typeInspectorMock.Setup( ti => ti.GetMarkedProperties<ClassWithNoAttributes, ArgumentAttribute>() ).Returns( markedProperties );
         Dependency.UnityContainer.RegisterInstance( typeInspectorMock.Object );

         // Test

         ArgumentAnalyzer.Analyze<ClassWithNoAttributes>( arguments );
      }

      [TestMethod]
      public void Analyze_PassedArgumentThatMatchesAttribute_SetsTheDecoratedProperty()
      {
         var markedPropertyMock = MarkedPropertyHelper.Create( "/?" );
         var markedProperties = ArrayHelper.Create( markedPropertyMock.Object );
         var arguments = ArrayHelper.Create( "/?" );

         // Setup

         var typeInspectorMock = new Mock<ITypeInspector>();
         typeInspectorMock.Setup( ti => ti.GetMarkedProperties<ClassWithArgumentText, ArgumentAttribute>() ).Returns( markedProperties );
         Dependency.UnityContainer.RegisterInstance( typeInspectorMock.Object );

         // Test

         var argumentClass = ArgumentAnalyzer.Analyze<ClassWithArgumentText>( arguments );

         markedPropertyMock.Verify( mp => mp.SetPropertyValue( true ), Times.Once() );
      }

      [TestMethod]
      public void Analyze_PassedArgumentThatDoesNotMatchAttribute_DoesNotSetTheDecoratedProperty()
      {
         var markedPropertyMock = MarkedPropertyHelper.Create( "/?" );
         var markedProperties = ArrayHelper.Create( markedPropertyMock.Object );
         var arguments = ArrayHelper.Create( "someArgument" );

         // Setup

         var typeInspectorMock = new Mock<ITypeInspector>();
         typeInspectorMock.Setup( ti => ti.GetMarkedProperties<ClassWithArgumentText, ArgumentAttribute>() ).Returns( markedProperties );
         Dependency.UnityContainer.RegisterInstance( typeInspectorMock.Object );

         // Test

         var argumentClass = ArgumentAnalyzer.Analyze<ClassWithArgumentText>( arguments );

         markedPropertyMock.Verify( mp => mp.SetPropertyValue( true ), Times.Never() );
      }
   }
}
