FROM microsoft/aspnetcore:2.0.5
COPY deploy /app
WORKDIR /app
# EXPOSE 80
EXPOSE 8085
# ENV ASPNETCORE_URLS http://*:5000
ENTRYPOINT ["dotnet", "Cheetoh.dll"]
