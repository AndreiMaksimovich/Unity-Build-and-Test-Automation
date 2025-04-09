#!/bin/bash
cd "$(dirname "$0")"
set -eux
cleanup() {
    docker compose -f Production.DockerCompose.yaml rm -fsv
}
trap cleanup EXIT
docker compose -f Production.DockerCompose.yaml up --build