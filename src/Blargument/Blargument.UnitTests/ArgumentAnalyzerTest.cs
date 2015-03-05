using System;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Blargument.UnitTests.Dummies;

namespace Blargument.UnitTests
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
         ArgumentAnalyzer.Analyze<Any>( null );
      }

      [TestMethod]
      [ExpectedException( typeof( MissingAttributesException ) )]
      public void Analyze_TypeNotDecoratedWithAnyAttributes_ThrowsMissingAttributesException()
      {
         var markedProperties = new MarkedProperty<ArgumentAttribute>[0];

         // Setup

         var typeInspectorMock = new Mock<ITypeInspector>();
         typeInspectorMock.Setup( ti => ti.GetMarkedProperties<ClassWithNoAttributes, ArgumentAttribute>() ).Returns( markedProperties );
         Dependency.UnityContainer.RegisterInstance( typeInspectorMock.Object );

         // Test

         ArgumentAnalyzer.Analyze<ClassWithNoAttributes>( new string[0] );
      }
   }
}
