using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using ArguMint.UnitTests.Helpers;

namespace ArguMint.UnitTests.Dynamic
{
   public class ClassBuilder
   {
      private readonly TypeBuilder _typeBuilder;
      private readonly Dictionary<string, PropertyBuilder> _propertyBuilders = new Dictionary<string, PropertyBuilder>();

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

      public void AddProperty<T>( string name )
      {
         var fieldBuilder = _typeBuilder.DefineField( $"_{name}", typeof( T ), FieldAttributes.Private );
         var propertyBuilder = _typeBuilder.DefineProperty( name, PropertyAttributes.HasDefault, typeof( T ), null );

         _propertyBuilders[name] = propertyBuilder;

         var getterSetterAttributes = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

         // Build the property "getter"

         var getterBuilder = _typeBuilder.DefineMethod( $"get_{name}", getterSetterAttributes, typeof( T ), Type.EmptyTypes );
         ILGenerator getter = getterBuilder.GetILGenerator();

         getter.Emit( OpCodes.Ldarg_0 );
         getter.Emit( OpCodes.Ldfld, fieldBuilder );
         getter.Emit( OpCodes.Ret );

         // Build the property "setter"

         var typeArray = ArrayHelper.Create( typeof ( T ) );
         var setterBuilder = _typeBuilder.DefineMethod( $"set_{name}", getterSetterAttributes, null, typeArray );
         ILGenerator setter = setterBuilder.GetILGenerator();

         setter.Emit( OpCodes.Ldarg_0 );
         setter.Emit( OpCodes.Ldarg_1 );
         setter.Emit( OpCodes.Stfld, fieldBuilder );
         setter.Emit( OpCodes.Ret );

         // Wire up the getter and setter to the property itself

         propertyBuilder.SetGetMethod( getterBuilder );
         propertyBuilder.SetSetMethod( setterBuilder );
      }
   }
}
