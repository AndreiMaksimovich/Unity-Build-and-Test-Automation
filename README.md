# Unity Build and Test Automation

## This repository contains:
- A demo Unity project featuring editor scripts and tools for automating Android, iOS, and WebGL Desktop/Mobile builds, as well as light baking and testing automation.
- Docker configuration files and scripts for the Dockerization of Unity WebGL builds, supporting both universal Mobile and Desktop builds.
- Example Jenkins configurations with support for automated building, testing, and deploying Unity projects (Android, iOS, WebGL).

Notice: This is a demo and is not intended for production use. It can serve as a foundation for your own CI/CD pipeline and automation. Please note that this demo was prepared on macOS, and experiences may vary on different operating systems.


# Unity Build Automatization

The Unity project includes an automated build system that supports light baking, addressables building, and building the project for iOS, Android and WebGL Mobile/Desktop platforms. Build scripts can be triggered externally.

## Important files:

### Light baking
- /Assets/Editor/Scripts/LightBaker.cs
- /Assets/Editor/Scripts/LightBakerConfiguration.cs

### Build Automatization
- /Assets/Editor/Scripts/BuildBehaviour.cs
- /Assets/Editor/Scripts/BuildConfiguration.cs
- /Assets/Editor/Scripts/BuildManager.cs
- /Assets/Editor/Scripts/BuildManagerConfiguration.cs

### Configurations
- /Assets/Configurations/BuildManagerConfiguration.asset
- /Assets/ Configurations/BuildConfigurations/…

# WebGL Build Dockerization

 The scripts, Docker files, and configurations provide a foundational setup for Dockerizing Unity WebGL builds for both production and development purposes. These containers utilize the nginx base image and support universal double builds Mobile/Desktop, gzip (.gz) and brotli (.br) compression formats.

## Important files:
- /Docker-WebGL/nginx/default.conf.template – nginx configuration template for Unity WebGL build that supports gunzip (.gz) and brotli (.br) compressions formats.
- /Docker-WebGL/src/index.html – index.html with support for WebGL double build that using user-agent sends users to correct build (Please note that this is just an example; in a production environment, a customized WebGL Unity template and a merged build are more appropriate.).

Notice: Development containers will use build folders as live-source/volumes.

# Jenkins build & testing automatization

This is an example of Jenkins configurations that can be used on a local machine or a machine dedicated to tests and builds, to automate testing and building.

How to install: copy folders into your Jenkins projects/configuration folder and restart Jenkins. Adjust project configurations for your local path (Unity, Android SDK, …).

Notice: CFGUTIL is used to deploy iOS builds on locally connected test devices.  Configurations do not contain instructions for production builds

