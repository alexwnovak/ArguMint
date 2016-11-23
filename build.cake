#tool "nuget:?package=xunit.runner.console"

var target = Argument( "target", "Default" );
var configuration = Argument( "configuration", "Release" );

var buildDir = Directory( "./src/ArguMint/ArguMint/bin" ) + Directory( configuration );

//===========================================================================
// Clean Task
//===========================================================================

Task( "Clean" )
   .Does( () =>
{
   CleanDirectory( buildDir );
});

//===========================================================================
// Restore Task
//===========================================================================

Task( "RestoreNuGetPackages" )
   .IsDependentOn( "Clean" )
   .Does( () =>
{
   NuGetRestore( "./src/ArguMint/ArguMint.sln" );
} );

//===========================================================================
// Build Task
//===========================================================================

Task( "Build" )
   .IsDependentOn( "RestoreNuGetPackages")
   .Does( () =>
{
  MSBuild( "./src/ArguMint/ArguMint.sln", settings => settings.SetConfiguration( configuration ) );
} );

//===========================================================================
// Test Task
//===========================================================================

Task( "RunUnitTests" )
   .IsDependentOn( "Build" )
   .Does( () =>
{
   var testAssemblies = new[]
   {
      "./src/ArguMint/ArguMint.UnitTests/bin/" + Directory( configuration ) + "/ArguMint.UnitTests.dll",
      "./src/ArguMint/ArguMint.IntegrationTests/bin/" + Directory( configuration ) + "/ArguMint.IntegrationTests.dll",
      "./src/ArguMint/ArguMint.TestCommon/bin/" + Directory( configuration ) + "/ArguMint.TestCommon.dll"
   };

   XUnit2( testAssemblies );
} );

//===========================================================================
// Default Task
//===========================================================================

Task( "Default" )
   .IsDependentOn( "RunUnitTests" );

RunTarget( target );
