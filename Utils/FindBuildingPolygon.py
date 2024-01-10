from OSMPythonTools.api import Api
from itertools import chain
import sys

sys.tracebacklimit = 0


def extractNodesFromWay(WayID):
    way = api.query("way/{}".format(WayID))
    wayNodes = []
    for i in way.nodes():
        node = (i.lat(), i.lon())
        wayNodes.append(node)
    return wayNodes


try:
    api = Api()
    building = sys.argv[1]
    form = building.split('/')[-2]
    nodes = []
    if form == "relation":
        relation = api.query("/".join(building.split('/')[-2:]))
        ways = []
        for i in relation.members():
            ways.append(i.id())
        waysInRelation = relation.members()[0].id()
        for i in ways:
            nodes.append(extractNodesFromWay(i))
        nodes = list(chain.from_iterable(nodes))
    elif form == "way":
        nodes = extractNodesFromWay(building.split('/')[-1])
    else:
        print("Please enter a Way or Relation link")
    with open("nodes.txt", 'w') as f:
        for i in nodes:
            f.write(str(i)+'\n')
except:
    print("OSM link not found, Try again")
