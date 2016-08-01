using System;
using ArguMint.UnitTests.Dummies;
using FluentAssertions;

namespace ArguMint.UnitTests
{
   public class TypeInspectorTests
   {
      public void GetMarkedProperties_TypeHasNoProperties_ReturnsEmptyArrayOfMarkedProperties()
      {
         var typeInspector = new TypeInspector();

         var markedProperties = typeInspector.GetMarkedProperties<ClassWithNoAttributes, Attribute>();

         markedProperties.Should().BeEmpty();
      }

      public void GetMarkedProperties_TypeHasOnePropertyButHasNoAttributes_ReturnsEmptyArray()
      {
         var typeInspector = new TypeInspector();

         var markedProperties = typeInspector.GetMarkedProperties<ClassWithOneUnmarkedProperty, Attribute>();

         markedProperties.Should().BeEmpty();
      }

      public void GetMarkedProperties_TypeHasObsoletePropertyAndWeAskForIt_ReturnsThePropertyAndAttribute()
      {
         var typeInspector = new TypeInspector();

         var markedProperties = typeInspector.GetMarkedProperties<ClassWithOnePropertyMarkedAsObsolete, ObsoleteAttribute>();

         markedProperties.Should().HaveCount( 1 );
      }

      public void GetMarkedProperties_TypeHasTwoPropertiesButOneIsMarkedWithObsolete_ReturnsTheObsoleteAttributeWithProperty()
      {
         var typeInspector = new TypeInspector();

         var markedProperties = typeInspector.GetMarkedProperties<ClassWithTwoAttributesOneMarkedAsObsolete, ObsoleteAttribute>();

         markedProperties.Should().ContainSingle( p => p.PropertyName == "TheInt" );
      }

      public void GetMarkedProperties_TypeHasTwoPropertiesBothAreMarkedWithObsolete_ReturnsBothObsoleteAttributesWithPropertiesAndAttributeParameter()
      {
         var typeInspector = new TypeInspector();

         var markedProperties = typeInspector.GetMarkedProperties<ClassWithTwoPropertiesMarkedObsolete, ObsoleteAttribute>();

         markedProperties.Should().Contain( p => p.PropertyName == "X" && p.Attribute.Message == "Property X" );
         markedProperties.Should().Contain( p => p.PropertyName == "Y" && p.Attribute.Message == "Property Y" );
      }
   }
}
