using System;
using System.Runtime.Serialization;

namespace Blargument
{
   [Serializable]
   public class MissingAttributesException : Exception
   {
      public MissingAttributesException()
      {
      }

      public MissingAttributesException( string message )
         : base( message )
      {
      }

      public MissingAttributesException( string message, Exception inner )
         : base( message, inner )
      {
      }

      protected MissingAttributesException( SerializationInfo info, StreamingContext context )
         : base( info, context )
      {
      }
   }
}
