using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Random = System.Random;

namespace Logic.Map
{
    [System.Serializable]
    public class MapManager : MonoBehaviour
    {
        public MapNode endNode, nodePrefab;
    
        public GameObject mapNodesObject;
    
        public List<MapNode> mapNodes, startNodes, eliteNodes, shopNodes, 
                                friendNodes, restNodes, completedNodes, highlightedNodes = new List<MapNode>();

        public MapGenerator gen;

        public static MapManager Instance;

        public Random rng;

        private void Awake()
        {
            rng = new Random(RunManager.Instance.seed);
            
            if (Instance != null)
                Destroy(this);
            else
                Instance = this;
        
            gen = transform.gameObject.AddComponent<MapGenerator>();
            gen.GenerateMap();
            FindNodeNeighbors();
            RandomizeNodeConnections();
            AssignNodeTypes();
            HighlightAvailableNodes(startNodes);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                foreach (Transform child in mapNodesObject.transform) 
                {
                    GameObject.Destroy(child.gameObject);
                }
            
                mapNodes.Clear();
                startNodes.Clear();
                gen.GenerateMap();
                FindNodeNeighbors();
                RandomizeNodeConnections();
                AssignNodeTypes();
            }
        }

        private void FindNodeNeighbors()
        {
            foreach (var node in mapNodes)
            {
                node.FindNeighbors();
            }
        }
    
        private void RandomizeNodeConnections()
        {
            foreach (var node in startNodes)
            {
                node.GenerateConnections();
            }
            ClearUnusedNodes();
            DrawAllLines();
        }

        private void DrawAllLines()
        {
            foreach (var n in mapNodes)
            {
                if (n.rowLevel == MapGenerator.AmountOfRows-1) return;
            
                DrawLines(n);
            }
        }

        private static void DrawLines(MapNode node)
        {
            var itr = 0;
            foreach (var n in node.connectedNodes)
            {
                node.lineRenderers[itr].SetPosition(1,n.transform.localPosition - node.transform.localPosition);
                itr++;
            }
        }

        public static MapNode GetNodeById(int id)
        {
            var t = from node in MapManager.Instance.mapNodes
                where node.nodeId == id
                select node;
            return t.First();
        }

        private void ClearUnusedNodes()
        {
            foreach (var node in mapNodes.Where(node => !node.hasConnections && node != endNode))
            {
                node.gameObject.SetActive(false);
            }
        }

        private void HighlightAvailableNodes(List<MapNode> nodeList)
        {
            foreach (var node in highlightedNodes)
            {
                node.IsHighlighted = false;
            }
            
            foreach (var node in nodeList)
            {
                node.IsHighlighted = true;
                highlightedNodes.Add(node);
            }
        }
        
        private void AssignNodeTypes()
        {
            // start nodes always fights
            foreach (var node in startNodes)
            {
                node.MapNodeType = MapNode.MapNodeTypes.Fight;
            }

            // end node always boss
            endNode.MapNodeType = MapNode.MapNodeTypes.Boss;
            
            // entire row before boss fight are rest areas
            foreach (var node in mapNodes.Where(node => node.rowLevel == endNode.rowLevel-1))
            {
                node.MapNodeType = MapNode.MapNodeTypes.Rest;
                restNodes.Add(node);
            }
            // randomly change 4-6 nodes for rest areas
            var rand = 0;
            var randRestAmount  = rng.Next(3, 7);
            for (var i = 0; i < randRestAmount; i++)
            {
                rand = rng.Next(5, endNode.nodeId);
                if (mapNodes[rand].MapNodeType == MapNode.MapNodeTypes.Fight && mapNodes[rand].gameObject.activeSelf)
                {
                    mapNodes[rand].MapNodeType = MapNode.MapNodeTypes.Rest;
                    restNodes.Add(mapNodes[rand]);
                }
                else
                {
                    i--;
                }
            }
            
            // choose an entire row(7-9) and 3-5 other nodes(row>3) for elite fights
            rand = rng.Next(7, 10);
            foreach (var node in mapNodes.Where(node => node.rowLevel == rand))
            {
                node.MapNodeType = MapNode.MapNodeTypes.Elite;
                eliteNodes.Add(node);
            }
            var randEliteAmount  = rng.Next(3, 6);
            for (var i = 0; i < randEliteAmount; i++)
            {
                rand = rng.Next(15, endNode.nodeId);
                if (mapNodes[rand].MapNodeType == MapNode.MapNodeTypes.Fight && mapNodes[rand].gameObject.activeSelf)
                {
                    mapNodes[rand].MapNodeType = MapNode.MapNodeTypes.Elite;
                    eliteNodes.Add(mapNodes[rand]);
                }
                else
                    i--;
            }
            
            // randomly change 3-5 nodes for shops
            var randShopAmount  = rng.Next(3, 6);
            for (var i = 0; i < randShopAmount; i++)
            {
                rand = rng.Next(5, endNode.nodeId);
                if (mapNodes[rand].MapNodeType == MapNode.MapNodeTypes.Fight && mapNodes[rand].gameObject.activeSelf)
                {
                    mapNodes[rand].MapNodeType = MapNode.MapNodeTypes.Shop;
                    shopNodes.Add(mapNodes[rand]);
                }
                else
                {
                    i--;
                }
                
            }
            
            // randomly change 4-6 nodes for friends
            var randFriendAmount = rng.Next(3, 6);
            for (var i = 0; i < randFriendAmount; i++)
            {
                rand = rng.Next(5, endNode.nodeId);
                if (mapNodes[rand].MapNodeType == MapNode.MapNodeTypes.Fight && mapNodes[rand].gameObject.activeSelf)
                {
                    mapNodes[rand].MapNodeType = MapNode.MapNodeTypes.Friend;
                    friendNodes.Add(mapNodes[rand]);
                }
                else
                    i--;
            }
        }

        public void NodeTouched(MapNode node)
        {
            switch (node.MapNodeType)
            {
                case MapNode.MapNodeTypes.Fight:
                    SceneManager.LoadScene("Arena");
                    break;
                case MapNode.MapNodeTypes.Friend:
                    break;
                case MapNode.MapNodeTypes.Event:
                    break;
                case MapNode.MapNodeTypes.Shop:
                    break;
                case MapNode.MapNodeTypes.Elite:
                    break;
                case MapNode.MapNodeTypes.Boss:
                    break;
                case MapNode.MapNodeTypes.Rest:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
                
        }

    }
}
