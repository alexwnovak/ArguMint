$testAssemblies = gci *Test*.dll -r | ? { $_.FullName.ToLower().Contains( "bin" ) } | Sort-Object Name -Unique
.\src\ArguMint\packages\Fixie.1.0.2\lib\net45\Fixie.Console.exe $testAssemblies