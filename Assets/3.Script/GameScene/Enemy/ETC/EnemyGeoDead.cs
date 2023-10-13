using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Geo
{
    SmallGio = 0, // 1
    MiddleGio, // 5
    LargeGio, // 25
}

public class EnemyGeoDead : MonoBehaviour
{
    [SerializeField] private GameObject[] geo;
    [SerializeField] private int enemyGeo = 0;

    private int[] geoNumber = { 1, 5, 25 };
    private int[] geoCount;

    private void Awake()
    {
        geoCount = new int[3];
        GeoCount();
    }

    private void GeoCount()
    {
        for (int i = geoNumber.Length - 1; i >= 0; i--)
        {
            geoCount[i] = enemyGeo / geoNumber[i];
            enemyGeo %= geoNumber[i];
        }
    }

    public void GeoCreate()
    {
        for (int i = 0; i < geo.Length; i++)
        {
            for (int j = 0; j < geoCount[i]; j++)
            {
                // geo count 개수 만큼 생성
                Instantiate(geo[i], transform.position, Quaternion.identity);
            }
        }
    }
}
