#!/bin/bash
cd "$(dirname "$0")"
docker compose -f Production.DockerCompose.yaml up --build --detach