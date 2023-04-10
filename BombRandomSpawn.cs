
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class BombRandomSpawn : UdonSharpBehaviour
{
    //GameObject (for item list)
    public GameObject[] item_list;
    //GameObject (for item spawn point)
    public GameObject[] item_spawn_point;
    [UdonSynced] public int item_index;
    //int (for GameObject point index sync)
    [UdonSynced] public int point_index;
    public void ItemSpawn()
    {
        //instance owner only
        if(!Networking.IsOwner(gameObject)) {
            return;
        }
        //Item Spawn but item_list and Point is sync
        //and item_list GameObject is active then another item spawn

        GameObject item = item_list[Random.Range(0, item_list.Length)];
        GameObject point = item_spawn_point[Random.Range(0, item_spawn_point.Length)];

        //find item index
        for(int i = 0; i < item_list.Length; i++) {
            if(item_list[i] == item) {
                item_index = i;
            }
        }
        //find point index
        for(int i = 0; i < item_spawn_point.Length; i++) {
            if(item_spawn_point[i] == point) {
                point_index = i;
            }
        }
        if(item.activeSelf) {
            ItemSpawn();
            return;
        }
        else {
            RequestSerialization();
            syncItemSpawn();
        }
    }
    public override void OnDeserialization() 
    {
        syncItemSpawn();
    }
    public void syncItemSpawn() 
    {
        GameObject item = item_list[item_index];
        GameObject point = item_spawn_point[point_index];

        item.transform.position = point.transform.position;
        item.SetActive(true);
    }
}
