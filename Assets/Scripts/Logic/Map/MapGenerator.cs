using UnityEngine;

namespace Logic.Map
{
    public class MapGenerator : MonoBehaviour
    {
        public static int AmountOfRows => 17;
    
        private int[] rowNodeAmount;
        //private int MinNodesPerRow = 2;
        private const int MaxNodesPerRow = 5;

        private const int HorizontalSpacing = 300;
        private const int VerticalSpacing = 350;
        private const int VerticalOffset = -2700;
        private const float NodeShiftAmount = 75;

        public void GenerateMap()
        {
            InitializeRows();
            GenerateRows();
        }

        private void InitializeRows()
        {
            rowNodeAmount = new int[AmountOfRows];
            rowNodeAmount[0] = MapManager.Instance.startNodes.Capacity; // Row 0 is starting nodes
            rowNodeAmount[16] = 1; // Last row

            for (var i = 1; i < rowNodeAmount.Length - 1; i++)
            {
                rowNodeAmount[i] = MaxNodesPerRow;
            }
        
        }

        private static void GenerateRows()
        {
            for (var y = 0; y < AmountOfRows; y++)
            {
                // Instantiate each node in the row
                for (var x = 0; x < MaxNodesPerRow; x++)
                {
                    var node = Instantiate(MapManager.Instance.nodePrefab, MapManager.Instance.mapNodesObject.transform);

                    // Position the node so the row is centered
                    var v = new Vector3((x * HorizontalSpacing) - ((MaxNodesPerRow-1) * HorizontalSpacing/2), 
                                        VerticalOffset + y * VerticalSpacing, 
                                        0);
                    node.transform.localPosition = v;

                    // Add to startNodes
                    if (y == 0)
                    {
                        MapManager.Instance.startNodes.Add(node);
                    }
                    // Add the last node
                    else if (y == AmountOfRows - 1)
                    {
                        MapManager.Instance.endNode = node;
                        node.transform.localPosition = new Vector3(0, 
                                                                    VerticalOffset + y * VerticalSpacing, 
                                                                    0);
                    }
                
                    // if this isn't the last row...
                    if (MapManager.Instance.endNode != node)
                    {
                        // ...Add some noise to make the map's appearance less uniform
                        var localPosition = node.transform.localPosition;
                        v = new Vector3(localPosition.x + MapManager.Instance.rng.Next((int)-NodeShiftAmount, (int)NodeShiftAmount),
                            localPosition.y + MapManager.Instance.rng.Next((int)-NodeShiftAmount, (int)NodeShiftAmount), 0);
                        node.transform.localPosition = v;
                    }

                    node.name = MapManager.Instance.mapNodes.Count.ToString();
                    node.nodeId = MapManager.Instance.mapNodes.Count;
                    node.rowLevel = y;
                    node.numInRow = x;
                    node.MapNodeType = MapNode.MapNodeTypes.Fight;
                    MapManager.Instance.mapNodes.Add(node);
                }
            }
        }

        public int GetRowNodeAmount(int row)
        {
            return rowNodeAmount[row];
        }

        public int GetMaxNodesPerRow()
        {
            return MaxNodesPerRow;
        }
    
    }
}
