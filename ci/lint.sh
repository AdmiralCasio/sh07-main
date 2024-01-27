#!/usr/bin/env bash

set -e
set -x

echo "Linting using $IMAGE_NAME in $PWD"
docker build -t monoapp .
docker run -it --rm monoapp
dotnet new tool-manifest
dotnet tool install csharpier
dotnet tool restore
dotnet csharpier --check