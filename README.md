# Robot Cleaner

## Example
**Input:**

    2
    10 22
    E 2
    N 1

**Output:**

    => Cleaned: 4

With a resulting position of `(12, 21)`

## Run via Docker

1. Build
```
docker build -t robotcleaner -f RobotCleaner/Dockerfile .
```
2. Run
```
docker run --rm -it robotcleaner
```

## Run tests

1. Build
```
docker build -t robotcleanertests -f RobotCleaner.Test/Dockerfile .
```
2. Run
```
docker run --rm -it robotcleanertests
```
