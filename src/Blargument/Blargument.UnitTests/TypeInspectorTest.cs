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

      [TestMethod]
      public void GetMarkedProperties_TypeHasOnePropertyButHasNoAttributes_ReturnsEmptyArray()
      {
         var typeInspector = new TypeInspector();

         var markedProperties = typeInspector.GetMarkedProperties<ClassWithOneUnmarkedProperty, Attribute>();

         Assert.AreEqual( 0, markedProperties.Length );
      }

      [TestMethod]
      public void GetMarkedProperties_TypeHasObsoletePropertyAndWeAskForIt_ReturnsThePropertyAndAttribute()
      {
         var typeInspector = new TypeInspector();

         var markedProperties = typeInspector.GetMarkedProperties<ClassWithOnePropertyMarkedAsObsolete, ObsoleteAttribute>();

         Assert.AreEqual( 1, markedProperties.Length );
      }

      [TestMethod]
      public void GetMarkedProperties_TypeHasTwoPropertiesButOneIsMarkedWithObsolete_ReturnsTheObsoleteAttributeWithProperty()
      {
         var typeInspector = new TypeInspector();

         var markedProperties = typeInspector.GetMarkedProperties<ClassWithTwoAttributesOneMarkedAsObsolete, ObsoleteAttribute>();

         Assert.AreEqual( 1, markedProperties.Length );
         Assert.AreEqual( "TheInt", markedProperties[0].PropertyInfo.Name  );
      }
   }
}
