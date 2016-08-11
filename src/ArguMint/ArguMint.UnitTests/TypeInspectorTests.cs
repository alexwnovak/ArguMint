using System;
using FluentAssertions;
using ArguMint.UnitTests.Dummies;

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

      public void GetMarkedMethods_TypeHasOneMatchingMethod_GetsTheMethod()
      {
         var typeInspector = new TypeInspector();

         var markedMethods = typeInspector.GetMarkedMethods<ClassWithOneMarkedPublicInstanceMethod, ObsoleteAttribute>();

         markedMethods.Should().ContainSingle( m => m.Name == "InstanceMethod" );
      }

      public void GetMarkedMethods_TypeHasTwoMatchingMethods_GetsBothMethods()
      {
         var typeInspector = new TypeInspector();

         var markedMethods = typeInspector.GetMarkedMethods<ClassWithTwoMarkedPublicInstanceMethods, ObsoleteAttribute>();

         markedMethods.Should().HaveCount( 2 );
         markedMethods.Should().Contain( m => m.Name == "MethodOne" );
         markedMethods.Should().Contain( m => m.Name == "MethodTwo" );
      }

      public void GetMarkedMethods_TypeHasOneMatchingStaticMethod_GetsMethod()
      {
         var typeInspector = new TypeInspector();

         var markedMethods = typeInspector.GetMarkedMethods<ClassWithOneMarkedPublicStaticMethod, ObsoleteAttribute>();

         markedMethods.Should().ContainSingle( m => m.Name == nameof( ClassWithOneMarkedPublicStaticMethod.StaticMethod ) );
      }

      public void GetMarkedMethods_TypeHasOneMatchingPrivateInstanceMethod_GetsMethod()
      {
         var typeInspector = new TypeInspector();

         var markedMethods = typeInspector.GetMarkedMethods<ClassWithOneMarkedPrivateInstanceMethod, ObsoleteAttribute>();

         markedMethods.Should().ContainSingle( m => m.Name == "PrivateInstanceMethod" );
      }

      public void GetMarkedMethods_TypeHasOneMatchingPrivateStaticMethod_GetsMethod()
      {
         var typeInspector = new TypeInspector();

         var markedMethods = typeInspector.GetMarkedMethods<ClassWithOneMarkedPrivateStaticMethod, ObsoleteAttribute>();

         markedMethods.Should().ContainSingle( m => m.Name == "PrivateStaticMethod" );
      }
   }
}
