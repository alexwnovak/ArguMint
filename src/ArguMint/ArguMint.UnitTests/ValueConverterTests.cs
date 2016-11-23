using FluentAssertions;
using Xunit;

namespace ArguMint.UnitTests
{
   public class ValueConverterTests
   {
      [Fact]
      public void Convert_SourceObjectIsString_ReturnsStringWithSameValue()
      {
         string sourceObject = "";

         var convertedObject = ValueConverter.Convert( sourceObject, sourceObject.GetType() );

         convertedObject.Should().BeOfType<string>();
         convertedObject.Should().Be( "" );
      }

      [Fact]
      public void Convert_SourceObjectIsInteger_ReturnsIntegerWithSameValue()
      {
         int sourceObject = 123;

         var convertedObject = ValueConverter.Convert( sourceObject.ToString(), sourceObject.GetType() );

         convertedObject.Should().BeOfType<int>();
         convertedObject.Should().Be( sourceObject );
      }
   }
}
