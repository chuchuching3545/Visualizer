using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class TrafficLightManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Tilemap> traffic_lights = new List<Tilemap>();
    public List<string[]> states = new List<string[]>();
    void Start()
    {
        // for(int i = 0; i < 12; ++i){
        //     traffic_lights[i].color = Color.red;
        // }
    }

    // Update is called once per frame
    public void step(float cur_time){
        int index = (int)Mathf.Floor(cur_time);
        for(int i = 0; i < 12; ++i){
            if(states[i] == null) continue;
            if(index < states[i].Length){
                if(states[i][index] == "R") traffic_lights[i].color = Color.red;
                if(states[i][index] == "G") traffic_lights[i].color = Color.green;
                if(states[i][index] == "Y") traffic_lights[i].color = Color.yellow;
            }
        }
    }

    public void reset(){
        states.Clear();
    }
}
