using System;
using FluentAssertions;
using Xunit;

namespace ArguMint.UnitTests
{
   public class ArgumentPositionExtensionsTests
   {
      [Fact]
      public void ToIndex_PositionIsAny_ReturnsMinusOne()
      {
         var argumentPosition = ArgumentPosition.Any;

         int index = argumentPosition.ToIndex();

         index.Should().Be( -1 );
      }

      [Fact]
      public void ToIndex_PositionIsFirst_ReturnsZero()
      {
         var argumentPosition = ArgumentPosition.First;

         int index = argumentPosition.ToIndex();

         index.Should().Be( 0 );
      }

      [Fact]
      public void ToIndex_PositionIsSecond_ReturnsOne()
      {
         var argumentPosition = ArgumentPosition.Second;

         int index = argumentPosition.ToIndex();

         index.Should().Be( 1 );
      }

      [Fact]
      public void ToIndex_PositionIsBelowTheFirstOption_ThrowsArgumentOutOfRangeException()
      {
         var argumentPosition = ArgumentPosition.Any - 1;

         Action toIndex = () => argumentPosition.ToIndex();

         toIndex.ShouldThrow<ArgumentOutOfRangeException>();
      }

      [Fact]
      public void ToIndex_PositionIsAboveTheLastOption_ThrowsArgumentOutOfRangeException()
      {
         var argumentPosition = ArgumentPosition.Tenth + 1;

         Action toIndex = () => argumentPosition.ToIndex();

         toIndex.ShouldThrow<ArgumentOutOfRangeException>();
      }
   }
}
