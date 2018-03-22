#r @"tools/FAKE/tools/FakeLib.dll"

open Fake

let findNuget = @"tools/nuget"

let nugetRestoreDirectory = @"../packages"

"./SFA.DAS.PaymentComponents.sln" |> RestoreMSSolutionPackages(fun p -> 
    { p with OutputPath = nugetRestoreDirectory }
)

let nUnitToolPath = @"tools\NUnit.ConsoleRunner\tools\nunit3-console.exe"
let rootPublishDirectory = getBuildParamOrDefault "publishDirectory"  @"C:\CompiledSource"
let testDirectory = getBuildParamOrDefault "buildMode" "Debug"
let myBuildConfig = if testDirectory = "Release" then MSBuildRelease else MSBuildDebug
let userPath = getBuildParamOrDefault "userDirectory" @"C:\Users\buildguest\"

let nunitTestFormat = getBuildParamOrDefault "nunitTestFormat" "nunit2"
let publishNuget = getBuildParamOrDefault "publishNuget" "false"
let nugetOutputDirectory = getBuildParamOrDefault "nugetOutputDirectory" "bin/Release"
let nugetAccessKey = getBuildParamOrDefault "nugetAccessKey" ""

let isAutomationProject = getBuildParamOrDefault "AcceptanceTests" "false"

let devWebsitePort = getBuildParamOrDefault "devport" "7071"
let accWebsitePort = getBuildParamOrDefault "accport" "5051"

let mutable projectName = ""
let mutable folderPrecompiled = @"\"+ projectName + ".Release_precompiled "
let mutable publishDirectory = rootPublishDirectory @@ projectName
let mutable publishingProfile = projectName + "PublishProfile"
let mutable shouldPublishSite = false
let mutable shouldCreateDbProject = false
let mutable sqlPublishFile = ""

let mutable versionNumber = getBuildParamOrDefault "versionNumber" "1.0.0.0"

let mutable solutionFilePresent = true

Target "Set version number" (fun _ ->
    if publishNuget.ToLower() = "false" then
        let assemblyMajorNumber = environVarOrDefault "BUILD_MAJORNUMBER" "1" 
        let assemblyMinorNumber = environVarOrDefault "BUILD_MINORNUMBER" "0" 

        if testDirectory.ToLower() = "release" then
            versionNumber <- buildVersion
            if buildVersion.Contains(".") then
                versionNumber <- buildVersion
            else if versionNumber.ToLower() <> "localbuild" then
                versionNumber <- sprintf  @"%s.%s.0.%s" assemblyMajorNumber assemblyMinorNumber buildVersion
            else
                versionNumber <- "1.0.0.0"

    else
        trace "Skipping version number set"

    trace versionNumber
)

Target "Set Solution Name" (fun _ ->
    let directoryHelper = FileSystemHelper.directoryInfo(currentDirectory).Name

    let mutable solutionNameToMatch = ""
    if isAutomationProject.ToLower() = "false" then 
        solutionNameToMatch <- "*.sln" 
    else 
        solutionNameToMatch <- "*Automation.sln"

    let findSolutionFile = TryFindFirstMatchingFile "*.sln" currentDirectory
    
    if findSolutionFile.IsSome then
        
        let solutionFileHelper = FileSystemHelper.fileInfo(findSolutionFile.Value)
            
        projectName <- solutionFileHelper.Name.Replace(solutionFileHelper.Extension, "")
        folderPrecompiled <- sprintf @"\%s.%s_precompiled " projectName testDirectory
        publishDirectory <- rootPublishDirectory @@  projectName
        publishingProfile <- projectName + "PublishProfile"

        let subDirectories = directoryInfo(currentDirectory).GetDirectories()
    
        if subDirectories.Length > 0 then 
            for directory in subDirectories do
                if shouldPublishSite = false then 
                    shouldPublishSite <- fileExists((directory.FullName @@ @"Properties\PublishProfiles\" @@ publishingProfile + ".pubxml"))
                
        else
            shouldPublishSite <- false

        let subdirs = FileSystemHelper.directoryInfo(currentDirectory).EnumerateDirectories("*.Database")
        
        let mutable databaseDir = ""

        for directs in subdirs do
            trace directs.Name
            let dirExists = directs.Name.Contains("Database")
            if(dirExists) then
                databaseDir <- directs.Name

        let sqlPublishFile = (@"./"+ databaseDir + "/Database.Publish.xml")
        shouldCreateDbProject <- fileExists(sqlPublishFile)

        trace ("Will publish: " + (shouldPublishSite.ToString()))
        trace ("Will build db project: " + (shouldCreateDbProject.ToString()))
        trace ("PublishingProfile: " + publishingProfile)
        trace ("PublishDirectory: " + publishDirectory)
        trace ("PrecompiledFolder: " + folderPrecompiled)
        trace ("Project Name has been set to: " + projectName)
    else
        solutionFilePresent <- false
)

Target "Update Assembly Info Version Numbers"(fun _ ->
    if testDirectory.ToLower() = "release" then
        trace "Update Assembly Info Version Numbers"
        BulkReplaceAssemblyInfoVersions(currentDirectory) (fun p ->
                {p with
                    AssemblyFileVersion = versionNumber 
                    AssemblyVersion = versionNumber 
                    })
)

Target "Clean Publish Directory" (fun _ ->
    trace "Clean Publish Directory"

    if FileHelper.TestDir(rootPublishDirectory) then
        FileHelper.CleanDir(rootPublishDirectory)
    else
        FileHelper.CreateDir(rootPublishDirectory)


    if FileHelper.TestDir(publishDirectory) then
        FileHelper.CleanDir(publishDirectory)
    else
        FileHelper.CreateDir(publishDirectory)
    

    if FileHelper.TestDir(publishDirectory) then
        FileHelper.CleanDir(publishDirectory)
    else
        FileHelper.CreateDir(publishDirectory)

    let directoryinfo = FileSystemHelper.directoryInfo(EnvironmentHelper.combinePaths publishDirectory @"\..\" + folderPrecompiled)
    let directory = directoryinfo.FullName
    trace directory
    if FileHelper.TestDir(directory) then
        FileHelper.CleanDir(directory)
    else
        FileHelper.CreateDir(directory)
)

Target "Build DNX Project"(fun _ ->
    trace "Build DNX PRoject"
    let dnuDir = userPath @@ @"\.dnx\runtimes\dnx-clr-win-x86.1.0.0-rc1-update1\bin\dnu.cmd"
    trace dnuDir
        
    let result =
        ExecProcess (fun info ->
            info.FileName <- dnuDir
            info.Arguments <- @"publish .\src\MvcPOC --out .\WR\Publish --configuration Release --runtime dnx-clr-win-x86.1.0.0-rc1-update1"
            info.WorkingDirectory <- rootPublishDirectory @@ @"..\..\"
        ) (System.TimeSpan.FromMinutes 10.)
        
    if result <> 0 then failwith "Failed to build DNX project"
)

let buildSolution() = 
    if solutionFilePresent then
        let buildMode = getBuildParamOrDefault "buildMode" "Debug"

        let properties = 
                        [
                            ("TargetProfile","cloud")
                        ]

        !! (@"./" + projectName + ".sln")
            |> MSBuildReleaseExt null properties "Publish"
            |> Log "Build-Output: "

Target "Build Acceptance Solution"(fun _ ->
    buildSolution()
)

Target "Build Solution"(fun _ ->
    buildSolution()
)

Target "Publish Solution"(fun _ ->
    if shouldPublishSite then
        let buildMode = getBuildParamOrDefault "buildMode" "Debug"

        let directoryinfo = FileSystemHelper.directoryInfo(@".\" @@ publishDirectory)
        let directory = directoryinfo.FullName
        trace directory

        let properties = 
                        [
                            ("DebugSymbols", "False");
                            ("Configuration", buildMode);
                            ("PublishProfile", @".\" + publishingProfile + ".pubxml");
                            ("PublishUrl", directory);
                            ("DeployOnBuild","True");
                            ("ToolsVersion","14");
                        ]

        !! (@"./" + projectName + ".sln")
            |> MSBuildReleaseExt null properties "Build"
            |> Log "Build-Output: "
    else
        trace "Skipping publish"
)

Target "Build Database project"(fun _ ->
    if shouldCreateDbProject then
        trace "Publish Database project"

        trace (@".\" + projectName + ".Database.Publish.xml")

        let buildMode = getBuildParamOrDefault "buildMode" "Debug"
        let directoryinfo = FileSystemHelper.directoryInfo(@".\" @@ publishDirectory)
        let directory = directoryinfo.FullName
        

        let properties = 
                        [
                            ("DebugSymbols", "False");
                            ("SqlPublishProfilePath", @".\Database.Publish.xml");
                            ("ToolsVersion","14");
                        ]

        !! (@".\**\*.sqlproj")
            |> MSBuildReleaseExt null properties "Build"
            |> Log "Build-Output: "
    else
        trace "Skipping Build Database project"
)

Target "Publish Database project"(fun _ ->
    if shouldCreateDbProject then
        trace "Publish Database project"

        trace (@".\" + projectName + ".Database.Publish.xml")

        let buildMode = getBuildParamOrDefault "buildMode" "Debug"
        let directoryinfo = FileSystemHelper.directoryInfo(@".\" @@ publishDirectory)
        let directory = directoryinfo.FullName
        trace directory

        let properties = 
                        [
                            ("DebugSymbols", "False");
                            ("TargetDatabaseName", "Database");
                            ("SqlPublishProfilePath", @".\Database.Publish.xml");
                            ("TargetConnectionString", "Data Source=.;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True");
                            ("PublishScriptFileName","Database.sql");
                            ("ToolsVersion","14");
                            ("PublishToDatabase","true");
                        ]

        !! (@".\**\*.sqlproj")
            |> MSBuildReleaseExt null properties "Publish"
            |> Log "Build-Output: "
    else
        trace "Skipping Publish Database project"
)

Target "Clean Projects" (fun _ ->
    trace "Clean Projects"
    !! (@".\**\*.csproj")
        -- @".\**\ProviderPayments.TestStack.UI.csproj"
        -- @".\**\SFA.DAS.Payments.Automation.WebUI.UnitTests.csproj"
        -- @".\**\SFA.DAS.Payments.Automation.WebUI.csproj"
        -- @".\**\ProviderPayments.TestStack.Domain.csproj"
        -- @".\**\ProviderPayments.TestStack.Engine.ExecutionProxy.csproj"
        |> myBuildConfig "" "Clean"
        |> Log "AppBuild-Output: "
)

Target "Build Projects" (fun _ ->
    trace "Build Projects"
    !! (@".\**\*.csproj") 
        -- @".\**\SFA.DAS.Payments.Automation.WebUI.csproj"
        -- @".\**\ProviderPayments.TestStack.UI.csproj"
        -- @".\**\ProviderPayments.TestStack.Infrastructure.csproj"
        -- @".\**\SFA.DAS.Payments.Automation.WebUI.UnitTests.csproj"
        -- @".\AcceptanceTesting\IlrSubmissionPortal\**\*"
        -- @".\**\ExampleDCFSTask.csproj"
        -- @".\**\ConsoleEngine.csproj"
        -- @".\**\CoreTestApp.csproj"
        -- @".\**\IlrGeneratorApp.csproj"
        -- @".\**\ProviderPayments.TestStack.Engine.csproj"
        -- @".\**\ProviderPayments.TestStack.Application.csproj"
        -- @".\**\ProviderPayments.TestStack.Domain.csproj"
        -- @".\**\ManualTaskRunner.csproj"
        -- @".\**\ProviderPayments.TestStack.Engine.ExecutionProxy.csproj"
        |> myBuildConfig "" "Rebuild"
        |> Log "AppBuild-Output: "
)

Target "Cleaning Unit Tests" (fun _ ->
    trace "Cleaning Unit Tests"
    !! (@".\**\*.UnitTests.csproj")
      |> myBuildConfig "" "Clean"
      |> Log "AppBuild-Output: "
)

Target "Building Unit Tests" (fun _ ->
    trace "Building Unit Tests"
    !! (@".\**\*.UnitTests.csproj")
      |> myBuildConfig "" "Rebuild"
      |> Log "AppBuild-Output: "
)

Target "Run NUnit Tests" (fun _ ->
    trace "Run NUnit Tests"

    let mutable shouldRunTests = false

    let testDlls = !! (@".\**\*.UnitTests.dll") -- @".\**\obj\**\*"
    
    for testDll in testDlls do
        shouldRunTests <- true

    for testDll in testDlls do 
        let idx1 = testDll.LastIndexOf("\\") + 1
        let idx2 = testDll.IndexOf(".UnitTests.dll") - 1
        let testResultFileName = "TestResult." + testDll.[idx1..idx2] + ".xml"
        [testDll] |> Fake.Testing.NUnit3.NUnit3 (fun p -> 
            {p with
                ToolPath = nUnitToolPath;
                ShadowCopy = false;
                Framework = Testing.NUnit3.NUnit3Runtime.Net45;
                ResultSpecs = [(testResultFileName + ";format=" + nunitTestFormat)];
                })
)

Target "Cleaning Integration Tests" (fun _ ->
    trace "Cleaning Integration Tests"
    !! (".\**\*.IntegrationTests.csproj")
      |> myBuildConfig "" "Clean"
      |> Log "AppBuild-Output: "
)

Target "Building Integration Tests" (fun _ ->
    trace "Building Integration Tests"
    !! (".\**\*.IntegrationTests.csproj")
      |> myBuildConfig "" "Rebuild"
      |> Log "AppBuild-Output: "
)

Target "Run Integration Tests" (fun _ ->
    trace "Run Integration Tests"
    
    let mutable shouldRunTests = false

    let testDlls = !! (".\**\*.IntegrationTests.dll") -- @".\**\obj\**\*"
    
    for testDll in testDlls do
        shouldRunTests <- true
    
    if shouldRunTests then
        for testDll in testDlls do 
            let idx1 = testDll.LastIndexOf("\\") + 1
            let idx2 = testDll.IndexOf(".IntegrationTests.dll") - 1
            let testResultFileName = "TestResult-Integration." + testDll.[idx1..idx2] + ".xml"
            [testDll] |> Fake.Testing.NUnit3.NUnit3 (fun p -> 
            {p with
                ToolPath = nUnitToolPath;
                StopOnError = false;
                ResultSpecs = [(testResultFileName + ";format=" + nunitTestFormat)];
                })
)

Target "Clean Project" (fun _ ->
    trace "Clean Project"
    
    !! (@".\" + projectName + "\*.csproj")
      |> myBuildConfig "" "Clean"
      |> Log "AppBuild-Output: "
)

Target "Build Project" (fun _ ->
    trace "Building Project"
    
    !! (@".\" + projectName + "\*.csproj")
      |> myBuildConfig "" "Rebuild"
      |> Log "AppBuild-Output: "
)

"Set version number"
   ==>"Set Solution Name"
   ==>"Update Assembly Info Version Numbers"
   ==>"Clean Publish Directory"
   ==>"Clean Projects"
   ==>"Build Projects"
   ==>"Build Solution"
   ==>"Build Database project"
   ==>"Publish Solution"
   ==>"Run NUnit Tests"

"Set Solution Name"
    ==> "Build Database project"
    ==> "Publish Database project"
   
RunTargetOrDefault  "Run NUnit Tests"