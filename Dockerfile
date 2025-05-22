# Use Microsoft's ASP.NET 9.0 runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

# Copy the published output into the container
COPY myapp/ .

# Start the app using your published DLL
ENTRYPOINT ["dotnet", "IMS.dll"]
