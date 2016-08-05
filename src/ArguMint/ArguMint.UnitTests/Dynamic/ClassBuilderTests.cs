using System;
using FluentAssertions;

namespace ArguMint.UnitTests.Dynamic
{
   public class ClassBuilderTests
   {
      public void Create_DoesNoSetup_TypeIsNull()
      {
         var classBuilder = ClassBuilder.Create();

         classBuilder.Type.Should().BeNull();
      }

      public void Create_DoesNoSetupButGetsBuilt_TypeExists()
      {
         // Act

         var classBuilder = ClassBuilder.Create();

         classBuilder.Build();

         // Assert

         classBuilder.Type.Should().NotBeNull();
      }

      public void Create_PassesNullExpression_ThrowsArgumentException()
      {
         var classBuilder = ClassBuilder.Create();

         Action addAttribute = () => classBuilder.AddAttribute( "DoesNotMatter", null );

         addAttribute.ShouldThrow<ArgumentException>();
      }

      public void Create_ExpressionDoesNotCreateAnObject_ThrowsInvalidOperationException()
      {
         var classBuilder = ClassBuilder.Create();

         Action addAttribute = () => classBuilder.AddAttribute( "DoesNotExist", () => null );

         addAttribute.ShouldThrow<ArgumentException>();
      }

      public void Create_AddsAttributeToNonExistentProperty_ThrowsInvalidOperationException()
      {
         var classBuilder = ClassBuilder.Create();

         Action addAttribute = () => classBuilder.AddAttribute( "DoesNotExist", () => new ObsoleteAttribute() );

         addAttribute.ShouldThrow<InvalidOperationException>();
      }
   }
}
