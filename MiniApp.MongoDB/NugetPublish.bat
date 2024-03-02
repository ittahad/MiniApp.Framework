del *.nupkg

dotnet pack -o . -c release

dotnet nuget push *.nupkg --api-key oy2hrfvghxjkzhgqjx5axjldwkvazxcenltx6557foet6i --source https://api.nuget.org/v3/index.json

PAUSE