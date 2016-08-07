using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using FluentAssertions;
using ArguMint.TestCommon.Helpers;

namespace ArguMint.TestCommon.Dynamic
{
   public class ClassBuilder
   {
      private readonly TypeBuilder _typeBuilder;
      private readonly Dictionary<string, PropertyBuilder> _propertyBuilders = new Dictionary<string, PropertyBuilder>();

      public Type Type
      {
         get;
         private set;
      }

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
         if ( string.IsNullOrEmpty( name ) )
         {
            throw new ArgumentException( "Property name must not be null or empty", nameof( name ) );
         }

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

      public void AddAttribute( string propertyName, Expression<Func<Attribute>> expr )
      {
         if ( expr == null )
         {
            throw new ArgumentException( "Expression must not be null", nameof( expr ) );
         }

         var propertyBuilder = GetPropertyBuilderOrThrow( propertyName );

         if ( expr.Body is NewExpression )
         {
            var newExpression = (NewExpression) expr.Body;

            var argumentList = new List<object>();

            foreach ( var argument in newExpression.Arguments )
            {
               var constantExpression = argument as ConstantExpression;
               argumentList.Add( constantExpression.Value );
            }

            var arguments = argumentList.ToArray();

            var attributeBuilder = new CustomAttributeBuilder( newExpression.Constructor, arguments );

            propertyBuilder.SetCustomAttribute( attributeBuilder );
         }
         else if ( expr.Body is MemberInitExpression )
         {
            var memberInitExpression = (MemberInitExpression) expr.Body;
            var attributeInstance = expr.Compile()();

            //memberInitExpression.NewExpression.Constructor

            var argumentList = new List<object>();

            foreach ( var argument in memberInitExpression.NewExpression.Arguments )
            {
               var constantExpression = argument as ConstantExpression;
               argumentList.Add( constantExpression.Value );
            }

            var arguments = argumentList.ToArray();

            var propertyNames = memberInitExpression.Bindings.Select( b => b.Member.As<PropertyInfo>() ).ToArray();
            var propertyValues = memberInitExpression.Bindings.Select( b => b.Member.As<PropertyInfo>().GetValue( attributeInstance ) ).ToArray();

            var attributeBuilder = new CustomAttributeBuilder( memberInitExpression.NewExpression.Constructor, arguments, propertyNames, propertyValues );

            propertyBuilder.SetCustomAttribute( attributeBuilder );

         }
      }

      public void Build()
      {
         if ( Type != null )
         {
            throw new InvalidOperationException( "Type has already been built" );
         }

         Type = _typeBuilder.CreateType();
      }

      private PropertyBuilder GetPropertyBuilderOrThrow( string propertyName )
      {
         PropertyBuilder propertyBuilder;

         if ( !_propertyBuilders.TryGetValue( propertyName, out propertyBuilder ) )
         {
            throw new InvalidOperationException( $"Property must be defined first: {propertyName}" );
         }

         return propertyBuilder;
      }
   }
}
