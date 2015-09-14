param([bool]$force = $true,[string]$configuration = "release",$output = "..\nuget\",[string]$repo = "lzy\")

$projects = @(
    @{Path = '.\Framework\'; Project = 'LazyFramework'}
   ,@{Path = '.\Lazyframework.Data\'; Project = 'Lazyframework.Data'}
   ,@{Path = '.\SqlServer\'; Project = 'LazyFramework.MSSqlServer'}
   ,@{Path = '.\LazyFramework.CQRS\'; Project = 'LazyFramework.CQRS'}
)

$output = $output + $repo



if(!(Test-Path $output)){ New-Item $output -ItemType Directory}

$saveHash = $output+"lastbuild.log"
#Skal vi slette alle versionene inne i denne katalogen før vi bygger????

$remove = $output + "*.nupkg"
del $remove

$lastRev = ""
$currRev = git log -1 --format=%H

if(Test-Path $saveHash) {
    $lastRev = Get-Content($saveHash)
}



$projects | % {
    
    $_.Project
    
    $outstanding = (git status $_.Path --porcelain) | Out-String
    $msg = ((git log $lastRev`.`.$currRev --format=%B $_.Path) | Out-String )

    if(!($force)) {
        if (!($outstanding -eq "")){ 
            Write-Host $outstanding
            Write-Host "Commit all changes before building"
            return 
        } 


        if (($currRev -eq $lastRev) -or ($msg -eq ""))   {
            ": nothing to build "
            return
        }
    }
            
    #Updating spec file with release notes. 
     $specFile = $_.Path + $_.Project +".nuspec" 
     [xml]$xml = Get-Content $specFile
     $xml.package.metadata.releaseNotes = $msg.ToString()
     $xml.Save($specFile)
    #End

    $p = $_.Path+$_.Project+".vbproj"

    .\nuget pack $p  -OutputDirectory $output -Build -IncludeReferencedProjects -Symbols  -Properties Configuration=$configuration

    git checkout $specFile
    "Release notes:"
    $msg
}

$currRev | Set-Content $saveHash

