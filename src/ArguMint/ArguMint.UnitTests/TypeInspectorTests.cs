using System;
using FluentAssertions;
using ArguMint.UnitTests.Dummies;
using Xunit;

namespace ArguMint.UnitTests
{
   public class TypeInspectorTests
   {
      [Fact]
      public void GetMarkedProperties_TypeHasNoProperties_ReturnsEmptyArrayOfMarkedProperties()
      {
         var typeInspector = new TypeInspector();

         var markedProperties = typeInspector.GetMarkedProperties<Attribute>( typeof( ClassWithNoAttributes ) );

         markedProperties.Should().BeEmpty();
      }

      [Fact]
      public void GetMarkedProperties_TypeHasOnePropertyButHasNoAttributes_ReturnsEmptyArray()
      {
         var typeInspector = new TypeInspector();

         var markedProperties = typeInspector.GetMarkedProperties<Attribute>( typeof( ClassWithOneUnmarkedProperty ) );

         markedProperties.Should().BeEmpty();
      }

      [Fact]
      public void GetMarkedProperties_TypeHasObsoletePropertyAndWeAskForIt_ReturnsThePropertyAndAttribute()
      {
         var typeInspector = new TypeInspector();

         var markedProperties = typeInspector.GetMarkedProperties<ObsoleteAttribute>( typeof( ClassWithOnePropertyMarkedAsObsolete ) );

         markedProperties.Should().HaveCount( 1 );
      }

      [Fact]
      public void GetMarkedProperties_TypeHasTwoPropertiesButOneIsMarkedWithObsolete_ReturnsTheObsoleteAttributeWithProperty()
      {
         var typeInspector = new TypeInspector();

         var markedProperties = typeInspector.GetMarkedProperties<ObsoleteAttribute>( typeof( ClassWithTwoAttributesOneMarkedAsObsolete ) );

         markedProperties.Should().ContainSingle( p => p.PropertyName == "TheInt" );
      }

      [Fact]
      public void GetMarkedProperties_TypeHasTwoPropertiesBothAreMarkedWithObsolete_ReturnsBothObsoleteAttributesWithPropertiesAndAttributeParameter()
      {
         var typeInspector = new TypeInspector();

         var markedProperties = typeInspector.GetMarkedProperties<ObsoleteAttribute>( typeof( ClassWithTwoPropertiesMarkedObsolete ) );

         markedProperties.Should().Contain( p => p.PropertyName == "X" && p.Attribute.Message == "Property X" );
         markedProperties.Should().Contain( p => p.PropertyName == "Y" && p.Attribute.Message == "Property Y" );
      }

      [Fact]
      public void GetMarkedMethods_TypeHasOneMatchingMethod_GetsTheMethod()
      {
         var typeInspector = new TypeInspector();

         var markedMethods = typeInspector.GetMarkedMethods<ObsoleteAttribute>( typeof( ClassWithOneMarkedPublicInstanceMethod ) );

         markedMethods.Should().ContainSingle( m => m.Name == "InstanceMethod" );
      }

      [Fact]
      public void GetMarkedMethods_TypeHasTwoMatchingMethods_GetsBothMethods()
      {
         var typeInspector = new TypeInspector();

         var markedMethods = typeInspector.GetMarkedMethods<ObsoleteAttribute>( typeof( ClassWithTwoMarkedPublicInstanceMethods ) );

         markedMethods.Should().HaveCount( 2 );
         markedMethods.Should().Contain( m => m.Name == "MethodOne" );
         markedMethods.Should().Contain( m => m.Name == "MethodTwo" );
      }

      [Fact]
      public void GetMarkedMethods_TypeHasOneMatchingStaticMethod_GetsMethod()
      {
         var typeInspector = new TypeInspector();

         var markedMethods = typeInspector.GetMarkedMethods<ObsoleteAttribute>( typeof( ClassWithOneMarkedPublicStaticMethod ) );

         markedMethods.Should().ContainSingle( m => m.Name == nameof( ClassWithOneMarkedPublicStaticMethod.StaticMethod ) );
      }

      [Fact]
      public void GetMarkedMethods_TypeHasOneMatchingPrivateInstanceMethod_GetsMethod()
      {
         var typeInspector = new TypeInspector();

         var markedMethods = typeInspector.GetMarkedMethods<ObsoleteAttribute>( typeof( ClassWithOneMarkedPrivateInstanceMethod ) );

         markedMethods.Should().ContainSingle( m => m.Name == "PrivateInstanceMethod" );
      }

      [Fact]
      public void GetMarkedMethods_TypeHasOneMatchingPrivateStaticMethod_GetsMethod()
      {
         var typeInspector = new TypeInspector();

         var markedMethods = typeInspector.GetMarkedMethods<ObsoleteAttribute>( typeof( ClassWithOneMarkedPrivateStaticMethod ) );

         markedMethods.Should().ContainSingle( m => m.Name == "PrivateStaticMethod" );
      }
   }
}
