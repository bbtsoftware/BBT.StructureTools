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

ToolSettings.SetToolSettings(
    context: Context,
    testCoverageFilter: "+[*]* -[xunit.*]* -[*.Tests]* -[FluentAssertions]* -[BBT.MaybePattern]* -[BBT.StrategyPattern]*",
    testCoverageExcludeByAttribute: "*.ExcludeFromCodeCoverage*",
    testCoverageExcludeByFile: "*/*Designer.cs;*/*.g.cs;*/*.g.i.cs");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

Build.RunDotNetCore();