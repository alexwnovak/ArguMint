using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArguMint.UnitTests
{
   [TestClass]
   public class MarkedPropertyTest
   {
      [TestMethod]
      [ExpectedException( typeof( ArgumentNullException ) )]
      public void Constructor_PropertyInfoIsNull_ThrowsArgumentNullException()
      {
         var markedProperty = new MarkedProperty<DontCareAttribute>( null, new DontCareAttribute() );
      }
   }
}
