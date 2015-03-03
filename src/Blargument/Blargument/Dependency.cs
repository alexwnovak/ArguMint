using System;
using Microsoft.Practices.Unity;

namespace Blargument
{
   internal static class Dependency
   {
      private static bool _autoInitialize = true;
      public static bool AutoInitialize
      {
         get
         {
            return _autoInitialize;
         }
         set
         {
            _autoInitialize = value;
         }
      }

      private static readonly Lazy<UnityContainer> _unityContainer = new Lazy<UnityContainer>( CreateUnityContainer, true );
      public static UnityContainer UnityContainer
      {
         get
         {
            return _unityContainer.Value;
         }
      }

      internal static UnityContainer CreateUnityContainer()
      {
         var unityContainer = new UnityContainer();

         if ( AutoInitialize )
         {
            // Real wire-ups go here
         }

         return unityContainer;
      }
   }
}
