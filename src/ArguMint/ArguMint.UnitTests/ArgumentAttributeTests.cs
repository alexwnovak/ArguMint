using System;
using FluentAssertions;
using Xunit;

namespace ArguMint.UnitTests
{
   public class ArgumentAttributeTests
   {
      [Fact]
      public void Ctor_SpacingParameterIsLessThanEnumBase_ThrowsArgumentOutOfRangeException()
      {
         Action ctor = () => new ArgumentAttribute( "DoesNotMatter", Spacing.None - 1 );

         ctor.ShouldThrow<ArgumentOutOfRangeException>();
      }

      [Fact]
      public void Ctor_SpacingParameterIsGreaterThanEnumBounds_ThrowsArgumentOutOfRangeException()
      {
         Action ctor = () => new ArgumentAttribute( "DoesNotMatter", Spacing.Postfix + 1 );

         ctor.ShouldThrow<ArgumentOutOfRangeException>();
      }
   }
}
