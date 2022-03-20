using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GridTests
{
    [UnityTest]
    public IEnumerator WhenGridGetsPositions_AndGrid2x2_ThenReturnCorrectCellPositions()
    {
        // Arrange
        Grid grid = new GameObject().AddComponent<Grid>();
        yield return null;
        grid.ChangeSize(new Vector2Int(2, 2));

        // Act
        Vector3 cell1 = grid.PositionToCell(new Vector3(-0.9f, 0, 0.1f));
        Vector3 cell2 = grid.PositionToCell(new Vector3(0.1f, 0, 0.5f));
        Vector3 cell3 = grid.PositionToCell(new Vector3(-0.5f, 0, -0.5f));
        Vector3 cell4 = grid.PositionToCell(new Vector3(0.9f, 0, -0.9f));

        // Assert
        Assert.AreEqual(new Vector3(-0.5f, 0, 0.5f), cell1);
        Assert.AreEqual(new Vector3(0.5f, 0, 0.5f), cell2);
        Assert.AreEqual(new Vector3(-0.5f, 0, -0.5f), cell3);
        Assert.AreEqual(new Vector3(0.5f, 0, -0.5f), cell4);
    }
}
