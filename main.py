"""
>>> Ax + By = C

A = y2-y1; B = x1-x2; C = Ax1+By1 

float delta = A1 * B2 - A2 * B1;

if (delta == 0) 
    throw new ArgumentException("Lines are parallel");

float x = (B2 * C1 - B1 * C2) / delta;
float y = (A1 * C2 - A2 * C1) / delta;
"""


lineA = ((0,0), (100000,0))
lineB = ((50000,0), (50000, 75000))

def inside(Y, X, y, x):
    if Y[0] > Y[1]:
        inY = Y[1] <= y or y <= Y[0]
    else:
        inY = Y[0] <= y or y <= Y[1]

    if X[0] > X[1]:
        inX = X[1] <= x or x <= X[0]
    else:
        inX = X[0] <= x or x <= X[1]

    return inY and inX

def intersect(line_a, line_b):
    Y1 = line_a[0][1], line_a[1][1]
    X1 = line_a[0][0], line_a[1][0]

    Y2 = line_b[0][1], line_b[1][1]
    X2 = line_b[0][0], line_b[1][0]

    A1 = Y1[1] - Y1[0]
    B1 = X1[0] - X1[1]
    C1 = A1 * X1[0] + B1 * Y1[0]

    A2 = Y2[1] - Y2[0]
    B2 = X2[0] - X2[1]
    C2 = A2 * X2[0] + B2 * Y2[0]

    delta = A1 * B2 - A2 * B1

    if delta == 0:
        print("Lines are parallel")
        return False
    print("delta:", delta)
    x = (B2 * C1 - B1 * C2) / delta
    y = (A1 * C2 - A2 * C1) / delta
    print("x", x, "y", y)

    return inside(Y1, X1, y, x) and inside(Y2, X2, y, x)

def overlap(min1, max1, min2, max2):
    return max(0, min(max1, max2) - max(min1, min2))

if __name__ == "__main__":
    print(intersect(lineA, lineB))
    #print(overlap(2, -1, 1, 3))