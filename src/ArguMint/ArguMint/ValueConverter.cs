using System;
using System.ComponentModel;

namespace ArguMint
{
   internal static class ValueConverter
   {
      public static object Convert( string stringValue, Type destinationType )
      {
         var typeConverter = TypeDescriptor.GetConverter( destinationType );

         return typeConverter.ConvertFromString( stringValue );
      }
   }
}
