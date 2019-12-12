#! /bin/bash
docker build -t robotcleanertests -f RobotCleaner.Test/Dockerfile .

if [ $? -eq 0 ]; then
    echo "Build OK"
else
    echo "Failed to build"
fi

docker run --rm -it robotcleanertests

if [ $? -eq 0 ]; then
    echo "All tests ran OK"
else
    echo "Some tests failed"
fi