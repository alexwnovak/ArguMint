using System;
using Blargument.UnitTests.Dummies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Blargument.UnitTests
{
   [TestClass]
   public class TypeInspectorTest
   {
      [TestMethod]
      public void GetMarkedProperties_TypeHasNoProperties_ReturnsEmptyArrayOfMarkedProperties()
      {
         var typeInspector = new TypeInspector();

         var markedProperties = typeInspector.GetMarkedProperties<ClassWithNoAttributes, Attribute>();

         Assert.AreEqual( 0, markedProperties.Length );
      }
   }
}
