using System.Reflection;

namespace ArguMint.TestCommon.Helpers
{
   public static class ObjectExtensions
   {
      public static object Property( this object obj, string propertyName )
      {
         var propertyInfo = obj.GetType().GetProperty( propertyName, BindingFlags.Public | BindingFlags.Instance );
         return propertyInfo.GetValue( obj );
      }
   }
}
