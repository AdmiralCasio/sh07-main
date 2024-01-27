#!/usr/bin/env bash

set -e
set -x

echo "Linting using $image in $PWD"
dotnet new tool-manifest
dotnet tool install csharpier
dotnet tool restore
dotnet csharpier --check

