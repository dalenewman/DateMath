nuget pack DateMath.nuspec -OutputDirectory "c:\temp\modules"
nuget push "c:\temp\modules\DateMath.1.2.0.nupkg" -source https://api.nuget.org/v3/index.json