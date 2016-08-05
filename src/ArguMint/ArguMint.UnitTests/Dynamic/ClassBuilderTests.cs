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
   }
}
