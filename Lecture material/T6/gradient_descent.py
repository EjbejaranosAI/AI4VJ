# Gradient Descent: A basic guide for understanding and experimenting with gradient descent

# Author: Edison Bejarano and Ramon Mateo

import numpy as np

# Step 1: Define the function we want to minimize
# This is a simple mathematical function: f(x, y) = (x-2)^2 + 2*(y-3)^2
# We want to find the values of x and y that minimize this function.

def function(X):
    """
    This function returns the value of the function we want to minimize.
    It takes in an array X where:
        X[0] is the 'x' value
        X[1] is the 'y' value
    """
    return (X[0] - 2)**2 + 2*(X[1] - 3)**2

# Step 2: Define the gradient of the function
# The gradient gives us the direction in which the function increases most steeply.
# We need the gradient to update our current point to move towards the minimum.

def gradient(X):
    """
    This function returns the gradient (partial derivatives) of the function.
    The gradient helps us find the direction to move in order to minimize the function.
    """
    return np.array([2*X[0] - 4, 4*X[1] - 12])

# Step 3: Set the starting point
# We'll start at an arbitrary point, X = [30, 20], and use gradient descent to find the minimum.

X = np.array([30, 20])

# Step 4: Set the step size (alpha)
# The step size determines how big of a step we take in the direction of the gradient.
# A smaller alpha means slower convergence, while a larger one can lead to overshooting the minimum.

alpha = 0.05  # Step size multiplier

# Step 5: Perform gradient descent (200 steps)
# In each iteration, we calculate the gradient at our current position, 
# then update our position by subtracting the gradient, scaled by the step size.

for i in range(200):
    X = X - alpha * gradient(X)

# Step 6: Print the results
# After 200 iterations, we print the result. The expected result should be close to the minimum values:
# X = [2.00000002, 3.00000000]

print(f'Optimal values of X: {tuple(X)} = {function(X)}')

# Expected output (approximately):
# X = [2.00000002, 3.00000000]
# f(2.00000002, 3.00000000) = 3.902292974211357e-16

# How to Experiment:
# 1. Change the values of 'X' to start from different points.
#    - Modify the starting point by updating `X = np.array([30, 20])` to any other values.
#    - See how it affects the path taken by gradient descent.
#
# 2. Adjust the 'alpha' (step size) to see how it affects the convergence.
#    - Try changing `alpha = 0.05` to a smaller or larger value.
#    - A smaller alpha will make the descent slower, while a larger alpha might cause overshooting.
#
# 3. Modify the number of iterations and observe how many steps it takes to get close to the minimum.
#    - Try changing `for i in range(200)` to a different number, like 1000 or 500.
#    - Observe if the algorithm gets closer to the minimum value with more iterations or if it takes too long.
#
# Experiment with different parameters and see how the algorithm behaves! This will help you understand the mechanics of gradient descent.
