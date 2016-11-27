using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace ArguMint.UnitTests
{
   public class ArgumentAttributeTests
   {
      [Fact]
      public void Ctor_SpacingParameterIsLessThanEnumBase_ThrowsArgumentOutOfRangeException()
      {
         var values = (Spacing[]) Enum.GetValues( typeof( Spacing ) );

         Action ctor = () => new ArgumentAttribute( "DoesNotMatter", values.First() - 1 );

         ctor.ShouldThrow<ArgumentOutOfRangeException>();
      }

      [Fact]
      public void Ctor_SpacingParameterIsGreaterThanEnumBounds_ThrowsArgumentOutOfRangeException()
      {
         var values = (Spacing[]) Enum.GetValues( typeof( Spacing ) );

         Action ctor = () => new ArgumentAttribute( "DoesNotMatter", values.Last() + 1 );

         ctor.ShouldThrow<ArgumentOutOfRangeException>();
      }
   }
}
