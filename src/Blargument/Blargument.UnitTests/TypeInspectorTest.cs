using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Blargument.UnitTests.Dummies;

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
         Assert.AreEqual( "TheInt", markedProperties[0].PropertyInfo.Name );
      }

      [TestMethod]
      public void GetMarkedProperties_TypeHasTwoPropertiesBothAreMarkedWithObsolete_ReturnsBothObsoleteAttributesWithPropertiesAndAttributeParameter()
      {
         var typeInspector = new TypeInspector();

         var markedProperties = typeInspector.GetMarkedProperties<ClassWithTwoPropertiesMarkedObsolete, ObsoleteAttribute>();

         Assert.AreEqual( 2, markedProperties.Length );
         Assert.IsTrue( markedProperties.Any( p => p.PropertyInfo.Name == "X" && p.Attribute.Message == "Property X" ) );
         Assert.IsTrue( markedProperties.Any( p => p.PropertyInfo.Name == "Y" && p.Attribute.Message == "Property Y" ) );
      }
   }
}
