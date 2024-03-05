import json
import tkinter as tk
from tkinter import END, messagebox

from FindBuildingPolygon import getVectors
from shapely.affinity import scale
from shapely.geometry import Polygon


def read():
    try:
        title = str(buildingEntry.get())
        clue = str(clueEntry.get("1.0", "end-1c"))
        info = str(infoEntry.get("1.0", "end-1c"))
        url = str(osmUrlEntry.get())
        scale_factor = float(scaleEntry.get())
        inner = json.loads(getVectors(url))

        with open("Locations.txt", "w") as f:
            f.write(
                f"""
            {{
            \"name\" : \"{title}\",
            \"clue\" : \"{clue}\",
            \"info\" : \"{info}\",
            \"centre\" : [{Polygon(inner).centroid.x}, {Polygon(inner).centroid.y}],
            \"inner\" : [{inner}],
            \"outer\" : [{get_outer(inner, scale_factor)}]
            }}"""
            )
        messagebox.showinfo(
            "Success!", "Location added successfully, proceed to Locations.txt"
        )

        buildingEntry.delete(0, END)
        clueEntry.delete("1.0", END)
        infoEntry.delete("1.0", END)
        osmUrlEntry.delete(0, END)
        scaleEntry.delete(0, END)

    except Exception as e:
        print(e)
        messagebox.showerror("Error!", "An Error occurred, please try again")


def get_outer(vertices, factor):
    poly = Polygon(vertices)
    centroid = poly.centroid
    scaled = scale(poly, xfact=factor, yfact=factor, origin=centroid)
    scaled = list(scaled.exterior.coords)
    scaled_list = [list(ele) for ele in list(scaled)]
    return scaled_list


window = tk.Tk()
window.geometry("500x500")
window.title("Add Location")

buildingLabel = tk.Label(window, text="Building Name")
buildingLabel.pack(padx=10, pady=5)

buildingEntry = tk.Entry(window, width=40)
buildingEntry.pack(padx=10, pady=5)

clueLabel = tk.Label(window, text="Clue")
clueLabel.pack(padx=10, pady=5)

clueEntry = tk.Text(window, width=20, height=5)
clueEntry.pack(padx=10, pady=5)

infoLabel = tk.Label(window, text="Information")
infoLabel.pack(padx=10, pady=5)

infoEntry = tk.Text(window, width=20, height=5)
infoEntry.pack(padx=10, pady=5)

osmUrlLabel = tk.Label(window, text="OSM Location link")
osmUrlLabel.pack(padx=10, pady=5)

osmUrlEntry = tk.Entry(window, width=40)
osmUrlEntry.pack(padx=10, pady=5)

scaleLabel = tk.Label(window, text="Outer Boundary Box Scale")
scaleLabel.pack(padx=10, pady=5)

scaleEntry = tk.Entry(
    window,
    width=5,
)
scaleEntry.pack(padx=10, pady=5)

button = tk.Button(window, text="submit", command=read)
button.pack(padx=10, pady=5)

window.mainloop()
