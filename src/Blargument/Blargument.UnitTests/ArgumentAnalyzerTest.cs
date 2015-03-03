using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Blargument.UnitTests
{
   [TestClass]
   public class ArgumentAnalyzerTest
   {
      [TestMethod]
      [ExpectedException( typeof( ArgumentException ) )]
      public void Analyze_ArgumentsAreNull_ThrowsArgumentException()
      {
         ArgumentAnalyzer.Analyze<Any>( null );
      }
   }
}
