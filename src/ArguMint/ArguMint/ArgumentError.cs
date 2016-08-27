using System.Collections.Generic;

namespace ArguMint
{
   public class ArgumentError
   {
      public ArgumentErrorType ErrorType
      {
         get;
      }

      public Dictionary<string, object> Properties
      {
         get;
      }

      public ArgumentError( ArgumentErrorType errorType )
      {
         ErrorType = errorType;
         Properties = new Dictionary<string, object>();
      }

      public ArgumentError( ArgumentErrorType errorType, Dictionary<string, object> properties )
      {
         ErrorType = errorType;
         Properties = properties;
      }

      internal static void ThrowForTypeMismatch( string propertyName, string propertyType )
      {
         var properties = new Dictionary<string, object>
         {
            ["PropertyName"] = propertyName,
            ["PropertyType"] = propertyType
         };

         throw new ArgumentErrorException( ArgumentErrorType.TypeMismatch, properties );
      }

      internal static void ThrowForArgumentMissing( string propertyName )
      {
         var properties = new Dictionary<string, object>
         {
            ["PropertyName"] = propertyName,
         };

         throw new ArgumentErrorException( ArgumentErrorType.ArgumentMissing, properties );
      }

      public static void ThrowForPrefixArgumentHasNoValue()
      {
         throw new ArgumentErrorException( ArgumentErrorType.PrefixArgumentHasNoValue, null );
      }
   }
}
