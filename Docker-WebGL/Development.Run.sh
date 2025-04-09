#!/bin/bash
cd "$(dirname "$0")"
set -eux
cleanup() {
    docker compose -f Development.DockerCompose.yaml rm -fsv
}
trap cleanup EXIT
docker compose -f Development.DockerCompose.yaml up --build