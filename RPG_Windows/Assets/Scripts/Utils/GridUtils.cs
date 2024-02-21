using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class GridUtils
{
    public static Collider2D[] GetColliders(string gridName, string[] excludeCollsTag = null) {
        Collider2D[] result = null;
        var grid = GameObject.Find(gridName).GetComponent<Grid>() as Grid;
        result = grid.GetComponentsInChildren<Collider2D>().Where(c => c.GetType() != typeof(CompositeCollider2D)).ToArray();
        if(result != null) {
            if(excludeCollsTag != null && excludeCollsTag.Length > 0) {
                result = result.Where(c => !excludeCollsTag.Contains(c.tag)).ToArray();
            }
        }

        return result;
    }

    public static Collider2D[] GetColliders(string[] excludeCollsTag = null) {
        Collider2D[] result = null;
        var grids = GameObject.FindObjectsOfType(typeof(Grid)) as Grid[];
        foreach(Grid grid in grids) {
            var colls = grid.GetComponentsInChildren<Collider2D>().Where(c => c.GetType() != typeof(CompositeCollider2D)).ToArray();
            if(result == null) 
                result = colls;
            else {
                if(colls != null)
                    result.Concat(colls).ToArray();
            }
        }

        if(result != null) {
            if(excludeCollsTag != null && excludeCollsTag.Length > 0) {
                result = result.Where(c => !excludeCollsTag.Contains(c.tag)).ToArray();
            }
        }

        return result;
    }


    public static Collider2D[] GetColliders(string gridName, float selfHeight) {
        List<Collider2D> result = new List<Collider2D>();
        Collider2D[] colls = GetColliders(gridName);

        foreach(var collider2D in colls) {
            var heightObj = collider2D.GetComponent<HeightOfLevel>() as HeightOfLevel;
            if(heightObj != null && heightObj.GetSelfHeight() == selfHeight) {
                if(result == null) {
                    result = new List<Collider2D>();
                }

                result.Add(collider2D);
            }
        }

        return result.ToArray();
    }
}
