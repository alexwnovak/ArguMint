﻿using System;
using System.Runtime.Serialization;

namespace ArguMint
{
   [Serializable]
   internal class ArgumentErrorException : Exception
   {
      public ArgumentErrorException()
      {
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