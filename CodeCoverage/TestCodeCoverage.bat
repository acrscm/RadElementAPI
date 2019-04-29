%~d1
cd "%~p1"
cd ".."
cd "RadElement.API.IntegrationTests"
call cmd /K "dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover" 
