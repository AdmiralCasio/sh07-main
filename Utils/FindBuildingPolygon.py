from OSMPythonTools.api import Api
from itertools import chain
import sys

sys.tracebacklimit = 100


def extractNodesFromWay(WayID):
    api = Api()
    way = api.query("way/{}".format(WayID))
    wayNodes = []
    for i in way.nodes():
        node = (" [{lat} , {lon}], ".format(lat=i.lat(), lon=i.lon()))
        wayNodes.append(node)
    return wayNodes

def getVectors(url):
    try:
        api = Api()
        building = url
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
    
        out = "[ \n [ \n "
        for i in nodes:
            out += (str(i) + '\n')
        out = out[:-3] if out[-3] == ',' else out
        out += "\n]\n]"
        return out
    
    except Exception as error:
        print(type(error).__name__, error)
