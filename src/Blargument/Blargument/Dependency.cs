using System;
using Microsoft.Practices.Unity;

namespace Blargument
{
   internal static class Dependency
   {
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

         return unityContainer;
      }
   }
}
