open System.IO
open Fake.Git.Staging
#r @"packages/build/FAKE/tools/FakeLib.dll"

open System

open Fake
open Fake.ReleaseNotesHelper

let appPath = "./src/Cheetoh/" |> FullName


let dockerUser = getBuildParam "DockerUser"
let dockerPassword = getBuildParam "DockerPassword"
let dockerLoginServer = getBuildParam "DockerLoginServer"
let dockerImageName = getBuildParam "DockerImageName"

let deployDir = "./deploy"

let releaseNotes = File.ReadAllLines "RELEASE_NOTES.md"
let releaseNotesData =
    releaseNotes
    |> parseAllReleaseNotes

let release = List.head releaseNotesData


let dotnetcliVersion = DotNetCli.GetDotNetSDKVersionFromGlobalJson()

Target "Clean" (fun _ ->
    !!"src/**/bin"
    |> CleanDirs

    !! "src/**/obj/*.nuspec"
    |> DeleteFiles

    CleanDirs ["bin"; "temp"; "docs/output"; deployDir]
)

Target "InstallDotNetCore" (fun _ ->
  DotNetCli.InstallDotNetSDK dotnetcliVersion |> ignore
)


Target "Restore" (fun _ ->
    DotNetCli.Restore (fun p -> {p with WorkingDir = appPath})
)

Target "Build" (fun _ ->
    DotNetCli.Build(fun p -> {p with WorkingDir = appPath})
)

Target "BuildRelease" (fun _ ->
  DotNetCli.Publish(fun p -> {p with WorkingDir = appPath; Output = FullName deployDir})
)

Target "CreateDockerImage" (fun _ ->
    if String.IsNullOrEmpty dockerUser then
        failwithf "docker username not given."
    if String.IsNullOrEmpty dockerImageName then
        failwithf "docker image Name not given."
    let result =
        ExecProcess (fun info ->
            info.FileName <- "docker"
            info.UseShellExecute <- false
            info.Arguments <- sprintf "build -t %s/%s ." dockerUser dockerImageName) TimeSpan.MaxValue
    if result <> 0 then failwith "Docker build failed"
)


Target "SetReleaseNotes" (fun _ ->
    let lines = [
            "module internal ReleaseNotes"
            ""
            (sprintf "let Version = \"%s\"" release.NugetVersion)
            ""
            (sprintf "let IsPrerelease = %b" (release.SemVer.PreRelease <> None))
            ""
            "let Notes = \"\"\""] @ Array.toList releaseNotes @ ["\"\"\""]
    File.WriteAllLines("src/Cheetoh/ReleaseNotes.fs",lines)
)


Target "PrepareRelease" (fun _ ->
    Git.Branches.checkout "" false "master"
    Git.CommandHelper.directRunGitCommand "" "fetch origin" |> ignore
    Git.CommandHelper.directRunGitCommand "" "fetch origin --tags" |> ignore

    StageAll ""
    Git.Commit.Commit "" (sprintf "Bumping version to %O" release.NugetVersion)
    Git.Branches.pushBranch "" "origin" "master"

    let tagName = string release.NugetVersion
    Git.Branches.tag "" tagName
    Git.Branches.pushTag "" "origin" tagName

    let result =
        ExecProcess (fun info ->
            info.FileName <- "docker"
            info.Arguments <- sprintf "tag %s/%s %s/%s:%s" dockerUser dockerImageName dockerUser dockerImageName release.NugetVersion) TimeSpan.MaxValue
    if result <> 0 then failwith "Docker tag failed"
)

Target "Deploy" (fun _ ->
    let result =
        ExecProcess (fun info ->
            info.FileName <- "docker"
            info.WorkingDirectory <- deployDir
            info.Arguments <- sprintf "login %s --username \"%s\" --password \"%s\"" dockerLoginServer dockerUser dockerPassword) TimeSpan.MaxValue
    if result <> 0 then failwith "Docker login failed"

    let result =
        ExecProcess (fun info ->
            info.FileName <- "docker"
            info.WorkingDirectory <- deployDir
            info.Arguments <- sprintf "push %s/%s" dockerUser dockerImageName) TimeSpan.MaxValue
    if result <> 0 then failwith "Docker push failed"
)

Target "Run" (fun () ->
  let server = async {
    DotNetCli.RunCommand (fun p -> {p with WorkingDir = appPath}) "watch run"
  }
  let browser = async {
    Threading.Thread.Sleep 5000
    Diagnostics.Process.Start "http://localhost:8085" |> ignore
  }

  [ server; browser]
  |> Async.Parallel
  |> Async.RunSynchronously
  |> ignore
)

"Clean"
  ==> "InstallDotNetCore"
  ==> "Build"

"Clean"
  ==> "Restore"
  ==> "Run"

"Clean"
  ==> "InstallDotNetCore"
  ==> "SetReleaseNotes"
  ==> "BuildRelease"
  ==> "CreateDockerImage"
  ==> "PrepareRelease"
  ==> "Deploy"

RunTargetOrDefault "Build"