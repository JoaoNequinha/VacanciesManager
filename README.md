# Project Dashboard - .NET Backend Services

## Prerequisites 📝
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)
- [Docker for Desktop](https://www.docker.com/products/docker-desktop)

## Install PostgreSQL 🐘
- Make sure Docker is running: on a terminal/cmd, run: `docker -v`
- Install PostgreSQL: `docker run --name postgres-dashboard -p 5432:5432 -e POSTGRES_PASSWORD=password -d postgres:alpine`
 
**Docker for Desktop will:**
- Download the [postgres](https://hub.docker.com/_/postgres) image to your local machine
- Create a new container named *postgres-dashboard*
- Map the container's port 5432 to your local machine's port 5432
- Set 'password' as password for default user *postgres*

## Run the App 🚀
- Hit `Ctrl+F5` to run the app
- Alternatively, if you don't want to run the app on Docker then switch the run profile from *Docker* to *Dashboard.API*