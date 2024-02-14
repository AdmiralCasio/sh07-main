import tkinter as tk
from tkinter import *
from FindBuildingPolygon import *

window = tk.Tk()


def read():
    title = (str(buildingEntry.get()))
    clue = (str(clueEntry.get()))
    info = (str(infoEntry.get()))
    url = (str(osmUrlEntry.get()))
    print(getVectors(url))

window.title("Add Location")

# Buildine Name
buildingLabel = tk.Label(window, text="Building Name")
buildingLabel.grid(row=0, column=0)

buildingEntry = tk.Entry(window)
buildingEntry.grid(row=0, column=1)

# Clue
clueLabel = tk.Label(window, text="Clue")
clueLabel.grid(row=1, column=0)

clueEntry = tk.Entry(window)
clueEntry.grid(row=1, column=1)

# Info
infoLabel = tk.Label(window, text="Information")
infoLabel.grid(row=2, column=0)

infoEntry = tk.Entry(window)
infoEntry.grid(row=2, column=1)

# Building URL
osmUrlLabel = tk.Label(window, text="OSM Location link")
osmUrlLabel.grid(row=3,column=0)

osmUrlEntry = tk.Entry(window)
osmUrlEntry.grid(row=3,column=1)

button = tk.Button(window, text="submit", command=read).grid(row=4, column=1)

window.mainloop()
