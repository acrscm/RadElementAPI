%~d1
cd "%~p1"
cd "ReportGenerator"
cd "RadElement.Service.Tests"
call cmd /K "Reportgenerator.exe -reports:"../../RadElement.Service.Tests/coverage.opencover.xml" -targetdir:"../Reports"" 
