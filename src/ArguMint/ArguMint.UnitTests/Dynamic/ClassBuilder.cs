using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ArguMint.UnitTests.Dynamic
{
   public class ClassBuilder
   {
      private readonly TypeBuilder _typeBuilder;

      private ClassBuilder( TypeBuilder typeBuilder )
      {
         _typeBuilder = typeBuilder;
      }

      public static ClassBuilder Create()
      {
         var assemblyName = new AssemblyName( $"ClassBuilderProxyAssembly_{Guid.NewGuid()}" );

         AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly( assemblyName, AssemblyBuilderAccess.Run );
         ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule( "MainModule" );

         var typeName = $"DynamicClass_{Guid.NewGuid()}";

         TypeBuilder typeBuilder = moduleBuilder.DefineType( typeName,
            TypeAttributes.Public |
            TypeAttributes.Class |
            TypeAttributes.AutoClass |
            TypeAttributes.AnsiClass |
            TypeAttributes.BeforeFieldInit |
            TypeAttributes.AutoLayout,
            typeof( object ) );

         return new ClassBuilder( typeBuilder );
      }
   }
}
