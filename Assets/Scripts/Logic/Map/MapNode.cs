using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Logic.Map
{
    public class MapNode : MonoBehaviour
    {
        public Image mapNodeImage;
        public Image highlightImage;
        
        public bool hasConnections;

        private bool isHighlighted = false;
        public bool IsHighlighted
        {
            get => isHighlighted;
            set
            {
                isHighlighted = value;
                highlightImage.gameObject.SetActive(value);
            } 
                
        }
        public int highestConnection, nodeId, rowLevel, numInRow;
        public int lowestConnection = 5;
        
        public List<MapNode> neighborsAbove, connectedNodes = new List<MapNode>();
        public LineRenderer[] lineRenderers = new LineRenderer[5];
    
        public enum MapNodeTypes
        {
            Fight = 0,
            Friend = 1,
            Event = 2,
            Shop = 3,
            Elite = 4,
            Boss = 5,
            Rest = 6
        }
        private MapNodeTypes nodeType;
        public MapNodeTypes MapNodeType
        {
            get => nodeType; 
            set
            {
                switch (value)
                {
                    case MapNodeTypes.Fight:
                        nodeType = MapNodeTypes.Fight;
                        mapNodeImage.color = Color.red;
                        break;
                    case MapNodeTypes.Friend:
                        nodeType = MapNodeTypes.Friend;
                        mapNodeImage.color = Color.blue;
                        break;
                    case MapNodeTypes.Event:
                        nodeType = MapNodeTypes.Event;
                        mapNodeImage.color = Color.yellow;
                        break;
                    case MapNodeTypes.Shop:
                        nodeType = MapNodeTypes.Shop;
                        mapNodeImage.color = Color.green;
                        break;
                    case MapNodeTypes.Elite:
                        nodeType = MapNodeTypes.Elite;
                        mapNodeImage.color = Color.grey;
                        break;
                    case MapNodeTypes.Boss:
                        nodeType = MapNodeTypes.Boss;
                        mapNodeImage.color = Color.white;
                        break;
                    case MapNodeTypes.Rest:
                        nodeType = MapNodeTypes.Rest;
                        mapNodeImage.color = Color.cyan;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, "Bad Node Type");
                }
            }
        }

        public void FindNeighbors()
        {
            // Return if on last row
            if (rowLevel == MapGenerator.AmountOfRows-1) return;
        
            var tempList = from node in MapManager.Instance.mapNodes
                where node.rowLevel == rowLevel+1
                select node;
            neighborsAbove = tempList.ToList();
        }

        public void GenerateConnections()
        {
            hasConnections = true;
            // Return if on last row
            if (rowLevel == MapGenerator.AmountOfRows - 2)
            {
                
                connectedNodes.Add(MapManager.Instance.endNode);
                return;
            }
        
            // We check if we're a startNode...
            if (rowLevel == 0)
            {
                if (IsSplitting(50))
                {
                    ChooseRandomNodeAbove();
                }
            
                ChooseRandomNodeAbove();
            
            }
            // This is not the start or end rows
            else
            {
                if (IsSplitting(33))
                {
                    ChooseRandomNodeAbove();
                }
                ChooseRandomNodeAbove();
                
            }
            
        }

        private void ChooseRandomNodeAbove()
        {
            var rowAmount = MapManager.Instance.gen.GetMaxNodesPerRow();
            int rand;

            // The lowest possible connection is the left neighbors highest connection. This is to prevent intersects
            var lowestPossibleConnection = 0;
        
            // The highest possible connection is the right neighbors lowest connection if it has connections
            var highestPossibleConnection = MapManager.GetNodeById(nodeId + 1).lowestConnection;

            // TODO: WHY DOES THIS REACH 5 SOMETIMES??!?!?!?!?
            if (highestPossibleConnection == 5)
            {
                highestPossibleConnection = 4;
            }
            
            // This is the first node in the row...
            if (numInRow == 0)
            {
                rand = MapManager.Instance.rng.Next(0, 2);
            
                // Store if lower/higher connection was made
                IsHighestOrLowestConnection(rand);
                //IsLowestConnection(rand);
            }
            else
            {
                lowestPossibleConnection = MapManager.GetNodeById(nodeId - 1).highestConnection;
            }
        
            // This is the last node in the row...
            if (numInRow == rowAmount - 1)
            {
                rand = MapManager.Instance.rng.Next(numInRow - 1, numInRow + 1);
                highestPossibleConnection = numInRow;
            }
            else
            {
                rand = MapManager.Instance.rng.Next(numInRow - 1, numInRow + 2);
            }
            

            if (rand > rowAmount - 1)
                rand = rowAmount - 1;
            if (rand > highestPossibleConnection)
                rand = highestPossibleConnection;
            if (rand < lowestPossibleConnection)
                rand = lowestPossibleConnection;

            IsHighestOrLowestConnection(rand);
            //IsLowestConnection(rand);
        
            var node = neighborsAbove[rand];
            if (connectedNodes.Contains(node)) return;
            connectedNodes.Add(node);
        
            node.GenerateConnections();
        }

        private static bool IsSplitting(int percent)
        {
            return MapManager.Instance.rng.Next(100) < percent;
        }

        private void IsHighestOrLowestConnection(int r)
        {
            if (r > highestConnection)
            {
                highestConnection = r;
            }
            if (r < lowestConnection)
            {
                lowestConnection = r;
            }
        }

        private void OnMouseUp()
        {
            if (isHighlighted)
                MapManager.Instance.NodeTouched(this);
        }

    }
}
