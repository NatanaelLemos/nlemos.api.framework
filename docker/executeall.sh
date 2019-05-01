#!/bin/bash

cd ./scripts/

for f in *.sh; do
  bash "$f" -H
done
