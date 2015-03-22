using System;
using System.Linq;
using ArguMint.UnitTests.Dummies;
using Blargument;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArguMint.UnitTests
{
   [TestClass]
   public class TypeInspectorTest
   {
      [TestMethod]
      public void GetMarkedProperties_TypeHasNoProperties_ReturnsEmptyArrayOfMarkedProperties()
      {
         var typeInspector = new TypeInspector();

         var markedProperties = typeInspector.GetMarkedProperties<ClassWithNoAttributes, Attribute>();

         Assert.AreEqual<int>( 0, markedProperties.Length );
      }

      [TestMethod]
      public void GetMarkedProperties_TypeHasOnePropertyButHasNoAttributes_ReturnsEmptyArray()
      {
         var typeInspector = new TypeInspector();

         var markedProperties = typeInspector.GetMarkedProperties<ClassWithOneUnmarkedProperty, Attribute>();

         Assert.AreEqual<int>( 0, markedProperties.Length );
      }

      [TestMethod]
      public void GetMarkedProperties_TypeHasObsoletePropertyAndWeAskForIt_ReturnsThePropertyAndAttribute()
      {
         var typeInspector = new TypeInspector();

         var markedProperties = typeInspector.GetMarkedProperties<ClassWithOnePropertyMarkedAsObsolete, ObsoleteAttribute>();

         Assert.AreEqual<int>( 1, markedProperties.Length );
      }

      [TestMethod]
      public void GetMarkedProperties_TypeHasTwoPropertiesButOneIsMarkedWithObsolete_ReturnsTheObsoleteAttributeWithProperty()
      {
         var typeInspector = new TypeInspector();

         var markedProperties = typeInspector.GetMarkedProperties<ClassWithTwoAttributesOneMarkedAsObsolete, ObsoleteAttribute>();

         Assert.AreEqual<int>( 1, markedProperties.Length );
         Assert.AreEqual<string>( "TheInt", markedProperties[0].PropertyName );
      }

      [TestMethod]
      public void GetMarkedProperties_TypeHasTwoPropertiesBothAreMarkedWithObsolete_ReturnsBothObsoleteAttributesWithPropertiesAndAttributeParameter()
      {
         var typeInspector = new TypeInspector();

         var markedProperties = typeInspector.GetMarkedProperties<ClassWithTwoPropertiesMarkedObsolete, ObsoleteAttribute>();

         Assert.AreEqual<int>( 2, markedProperties.Length );
         Assert.IsTrue( markedProperties.Any<IMarkedProperty<ObsoleteAttribute>>( p => p.PropertyName == "X" && p.Attribute.Message == "Property X" ) );
         Assert.IsTrue( markedProperties.Any( p => p.PropertyName == "Y" && p.Attribute.Message == "Property Y" ) );
      }
   }
}
