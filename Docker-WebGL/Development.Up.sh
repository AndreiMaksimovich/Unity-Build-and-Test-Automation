#!/bin/bash
cd "$(dirname "$0")"
/usr/local/bin/docker compose -f Development.DockerCompose.yaml up --build --detach