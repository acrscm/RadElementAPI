%~d1
cd "%~p1"
cd ".."
cd "RadElement.Service.Tests"
call cmd /K "dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover" 
