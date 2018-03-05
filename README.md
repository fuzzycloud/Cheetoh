# Cheetoh - Blogging Platform

## Chapter 0 - Setup

Let's start with [Saturn Framework](https://github.com/SaturnFramework/Saturn)

Follow the instruction 1 - 4 from **How to start in 60 seconds**

1. Install `dotnet` template with `dotnet new -i Saturn.Template`
2. Create new folder and move into it - `mkdir Cheetoh && cd Cheetoh`
3. Create new Saturn application `dotnet new saturn -lang F#`
4. Run build process to ensure everything was scaffolded correctly and restore dependencies - `build.cmd / build.sh`

Everything is good to go. As build command showing it all green.

## Chapter 1 - Docker

Docker normally comes in the picture at the time of deployment. But I like it in start. It is very good constraint to have.

There are normally two options either you build with docker (good option if you are having separate deployment pipeline) or run with docker. Currently we gonna use second option. It is just a personal preference nothing else.