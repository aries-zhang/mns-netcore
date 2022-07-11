VERSION = 1.1.${BUILD_NUMBER}
NUGET_KEY = ${NUGET_API_KEY}

.PHONY: build test pack

build:
	dotnet build .

test:
	dotnet test .

pack:
	dotnet pack src/mns-netcore/mns-netcore.csproj -c Release -o ./packages -p:PackageVersion=${VERSION}
	dotnet nuget push ./packages/*.nupkg --skip-duplicate --api-key ${NUGET_KEY} --source https://www.nuget.org/
