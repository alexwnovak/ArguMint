using System;
using System.Runtime.Serialization;

namespace ArguMint
{
   [Serializable]
   public class ArgumentConfigurationException : Exception
   {
      public ArgumentConfigurationException()
      {
      }

      public ArgumentConfigurationException( string message )
         : base( message )
      {
      }

      public ArgumentConfigurationException( string message, Exception inner )
         : base( message, inner )
      {
      }

      protected ArgumentConfigurationException( SerializationInfo info, StreamingContext context )
         : base( info, context )
      {
      }
   }
}
