#! /bin/bash
docker build -f RobotCleaner.Test/Dockerfile .
if [ $? -eq 0 ]; then
    echo "All tests ran OK"
else
    echo "Some tests failed"
fi