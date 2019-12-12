#! /bin/bash
docker build -t robotcleaner -f RobotCleaner/Dockerfile .

if [ $? -eq 0 ]; then
    echo "Build OK"
else
    echo "Failed to build"
fi

docker run --rm -it robotcleaner