
# Difference between Dijkstra and A*:
#
# Dijkstra's Algorithm:
# - It is an algorithm for finding the shortest paths.
# - Works by assigning an infinite weight to all nodes, except the start node which is assigned a weight of 0.
# - Uses a priority queue to explore nodes with the least accumulated cost.
# - Does not take any heuristic into account, which means it explores more nodes compared to A*.
# - It is used when we do not have additional information about the environment and want to find optimal paths in directed or undirected graphs.
#
# A* Algorithm:
# - It is an improvement over Dijkstra's algorithm for both directed and undirected graphs.
# - Uses a heuristic function (usually the estimated distance from the current node to the target).
# - This heuristic is added to the accumulated cost of the node to prioritize nodes that seem more "promising".
# - Thanks to the heuristic, A* can be more efficient by exploring fewer nodes.
# - It is widely used in games and robotics applications, where there is knowledge of the target position.

import heapq
import math
import matplotlib.pyplot as plt
import networkx as nx
import random
import time
import matplotlib.animation as animation


# TODO: Test the implementation with different values of NUM_NODES: 30, 120, 250, 300, 400 and take the cost and the time 
# Number of nodes (modifiable)
NUM_NODES = 400

def dijkstra(graph, start, end):
    start_time = time.time()
    queue = [(0, start)]
    distances = {node: float('inf') for node in graph}
    distances[start] = 0
    previous = {node: None for node in graph}

    while queue:
        current_distance, current_node = heapq.heappop(queue)

        if current_node == end:
            break

        if current_distance > distances[current_node]:
            continue

        for neighbor, weight in graph[current_node].items():
            distance = current_distance + weight

            if distance < distances[neighbor]:
                distances[neighbor] = distance
                previous[neighbor] = current_node
                heapq.heappush(queue, (distance, neighbor))

    path = []
    while end is not None:
        path.append(end)
        end = previous[end]
    path.reverse()
    end_time = time.time()
    return path, distances[path[-1]], end_time - start_time

def a_star(graph, start, end, heuristic):
    start_time = time.time()
    queue = [(0, start)]
    distances = {node: float('inf') for node in graph}
    distances[start] = 0
    previous = {node: None for node in graph}

    while queue:
        current_distance, current_node = heapq.heappop(queue)

        if current_node == end:
            break

        for neighbor, weight in graph[current_node].items():
            g_cost = distances[current_node] + weight
            h_cost = heuristic(neighbor, end)
            f_cost = g_cost + h_cost

            if g_cost < distances[neighbor]:
                distances[neighbor] = g_cost
                previous[neighbor] = current_node
                heapq.heappush(queue, (f_cost, neighbor))

    path = []
    while end is not None:
        path.append(end)
        end = previous[end]
    path.reverse()
    end_time = time.time()
    return path, distances[path[-1]], end_time - start_time

def heuristic(node, end):
    # Using Manhattan distance as a heuristic (since nodes are integers)
    return abs(node - end)

# TODO: Implement cosine similarity as a heuristic function for your A* algorithm
#       Make sure to comment out the Manhattan heuristic above when you implement cosine similarity.



# Create a random graph with NetworkX
g = nx.gnp_random_graph(NUM_NODES, 0.09, seed=42)  # Random graph with NUM_NODES nodes and a connection probability of 0.09

graph = {}
for edge in g.edges():
    weight = round(random.uniform(1, 10), 2)
    if edge[0] not in graph:
        graph[edge[0]] = {}
    if edge[1] not in graph:
        graph[edge[1]] = {}
    graph[edge[0]][edge[1]] = weight
    graph[edge[1]][edge[0]] = weight

start = random.choice(list(graph.keys()))
end = random.choice(list(graph.keys()))

# Compare Dijkstra and A*
path_dijkstra, cost_dijkstra, time_dijkstra = dijkstra(graph, start, end)
path_a_star, cost_a_star, time_a_star = a_star(graph, start, end, heuristic)

print("Dijkstra Path:", path_dijkstra, "with Cost:", cost_dijkstra, "Time Taken:", time_dijkstra, "seconds")
print("A* Path:", path_a_star, "with Cost:", cost_a_star, "Time Taken:", time_a_star, "seconds")

# Visualize the paths in a combined graph
G = nx.Graph()
for node in graph:
    for neighbor, weight in graph[node].items():
        G.add_edge(node, neighbor, weight=weight)

# Use spring_layout for better node separation
pos = nx.spring_layout(G, seed=42, k=1.5, iterations=300)  # Adjusted k and iterations for better spacing

fig, ax = plt.subplots(figsize=(20, 15))  # Increased figure size for better clarity
ax.set_facecolor('#F0F0F0')  # Set background color to a darker shade for better contrast

# Draw the complete graph with color and width based on weight
weights = nx.get_edge_attributes(G, 'weight')
max_weight = max(weights.values())
min_weight = min(weights.values())

# Normalize edge weights to control edge width and transparency
edge_widths = [(weights.get(edge, weights.get((edge[1], edge[0]))) - min_weight) / (max_weight - min_weight) * 3 + 0.5 for edge in G.edges()]
edge_colors = ['#B0B0B0' for edge in G.edges()]  # Light gray edges for background

# Draw nodes with a consistent color and label sizes for better visualization
nx.draw(G, pos, with_labels=True, node_size=600, node_color='#4682B4', font_size=10, font_color='#000000', font_weight="bold", edge_color=edge_colors, width=edge_widths, alpha=0.5, ax=ax)

# Highlight the target and start nodes with distinct colors
nx.draw_networkx_nodes(G, pos, nodelist=[start], node_size=800, node_color='#FF8C00', label='Start Node', ax=ax)
nx.draw_networkx_nodes(G, pos, nodelist=[end], node_size=800, node_color='#DC143C', label='Target Node', ax=ax)

# Highlight edges only for the paths
path_edges_dijkstra = list(zip(path_dijkstra, path_dijkstra[1:]))
path_edges_a_star = list(zip(path_a_star, path_a_star[1:]))
path_edges = set(path_edges_dijkstra + path_edges_a_star)

# Ensure all edges have weight labels visible
edge_labels = {edge: f'{weights.get(edge, weights.get((edge[1], edge[0]))):.2f}' for edge in path_edges}
nx.draw_networkx_edge_labels(G, pos, edge_labels=edge_labels, font_size=12, font_color='black', ax=ax)  # Set font color to white for better contrast

# Highlight the Dijkstra path with a distinct color
nx.draw_networkx_edges(G, pos, edgelist=path_edges_dijkstra, edge_color='#FF6347', width=4, label='Dijkstra Path', ax=ax)  # Set width and color for Dijkstra path

# Highlight the A* path with a different color
nx.draw_networkx_edges(G, pos, edgelist=path_edges_a_star, edge_color='#32CD32', width=4, style='dashed', label='A* Path', ax=ax)  # Set width and style for A* path

# Add time taken to the plot title
plt.title(f"Path Comparison: Dijkstra vs A*  ------ Dijkstra Time: {time_dijkstra:.4f} s | A* Time: {time_a_star:.4f} s --------- Dijkstra Cost: {cost_dijkstra:.2f} | A* Cost: {cost_a_star:.2f}", fontsize=20, color='black')
plt.legend(fontsize=12, loc='upper right', facecolor='#F0F0F0', edgecolor='black')

# Animation function
paths = {'dijkstra': path_dijkstra, 'a_star': path_a_star}
colors_path = {'dijkstra': '#FF6347', 'a_star': '#32CD32'}
styles_path = {'dijkstra': 'solid', 'a_star': 'dashed'}

def update(num, paths, G, pos, ax):
    # Instead of clearing the entire axis, we only draw the specific edge being animated
    for algo, path in paths.items():
        if num < len(path) - 1:
            edge = (path[num], path[num + 1])
            nx.draw_networkx_edges(G, pos, edgelist=[edge], edge_color=colors_path[algo], width=4, style=styles_path[algo], ax=ax)

ani = animation.FuncAnimation(fig, update, frames=max(len(path_dijkstra), len(path_a_star)), fargs=(paths, G, pos, ax), interval=500, repeat=False)

plt.show()
