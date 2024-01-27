#!/usr/bin/env bash

set -e

docker run \
  -w /project/ \
  -v $UNITY_DIR:/project/ \
  $IMAGE_NAME \
  /bin/bash -c "/project/ci/lint.sh && /project/ci/lint.sh"
