using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArguMint
{
   [Serializable]
   internal class ArgumentErrorException : Exception
   {
      public ArgumentErrorType ErrorType
      {
         get;
      }

      public Dictionary<string, object> Properties
      {
         get;
      }

      public ArgumentErrorException( ArgumentErrorType errorType, Dictionary<string, object> properties )
      {
         ErrorType = errorType;
         Properties = properties;
      }

      public ArgumentErrorException( string message )
         : base( message )
      {
      }

      public ArgumentErrorException( string message, Exception inner )
         : base( message, inner )
      {
      }

      protected ArgumentErrorException( SerializationInfo info, StreamingContext context )
         : base( info, context )
      {
      }
   }
}
