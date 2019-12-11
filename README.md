# Robot Cleaner

A robot cleaner!

## Input and Output Criteria

- All input will be given on std in
- All output is expected on std out
- First input line: a single integer that represents the number of commands the robot should expect to execute before it knows it is done.
- Second input line: consists of two integer numbers (separated by space) that represents the starting coordinates `x y` of the robot.
- The third, and any subsequent line, will consist of two pieces of data. The first will be a single uppercase character (E, W, S, N), that represents the direction on the compass the robot should head. The second will be an integer representing the number of steps s that the robot should take in said direction.

## Special Notes

- The robot will never be sent outside the bounds of the plane
- Do not output any error messages.
- There will be no leading or trailing white space on any line of input
- There should be no leading or trailing whitespace on any line of output
- The robot cleans at every vertex it touches not just where it stops

## The output

The output of your program should be a number `u`, which represents the number of *unique* places in the office that where cleaned. The output of the number `u` should be prefixed by `=> Cleaned: `

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
