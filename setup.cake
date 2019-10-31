#load nuget:?package=Cake.Recipe&version=1.0.0

//////////////////////////////////////////////////////////////////////
// PARAMETERS
//////////////////////////////////////////////////////////////////////

Environment.SetVariableNames();

BuildParameters.SetParameters(
    context: Context, 
    buildSystem: BuildSystem,
    sourceDirectoryPath: "./src",
    title: "BBT.StructureTools",
    repositoryOwner: "bbtsoftware",
    repositoryName: "BBT.StructureTools",
    appVeyorAccountName: "BBTSoftwareAG",
    shouldPublishMyGet: false,
    shouldRunCodecov: true,
    shouldDeployGraphDocumentation: false,
    shouldRunDupFinder: false);

BuildParameters.PrintParameters(Context);

// ToDo https://github.com/bbtsoftware/BBT.StructureTools/issues/4
// ToolSettings.SetToolSettings(
    // context: Context,
    // dupFinderExcludePattern: new string[] { BuildParameters.RootDirectoryPath + "/src/BBT.StructureTools.Tests/*.cs" },
    // testCoverageFilter: "+[*]* -[xunit.*]* -[*.Tests]* -[Shouldly]*",
    // testCoverageExcludeByAttribute: "*.ExcludeFromCodeCoverage*",
    // testCoverageExcludeByFile: "*/*Designer.cs;*/*.g.cs;*/*.g.i.cs");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

Build.RunDotNetCore();