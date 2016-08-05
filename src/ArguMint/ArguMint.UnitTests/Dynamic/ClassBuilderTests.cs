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
   }
}
