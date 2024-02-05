using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 1;
    static Dictionary<string, float> degree = new Dictionary<string, float>(){{"u", 270}, {"d", 90}, {"r", 180}, {"l", 0}};
    public string[] forward_directions;
    public int id;
    public float p;
    public float pre_time;
    public bool isAppear = false;

    public void move(float cur_time){
        float delta = cur_time-pre_time;
        pre_time = cur_time;
        int cur_index = (int)Mathf.Floor(cur_time-p);
        if(cur_index < forward_directions.Length){
            Vector3 movement;
            float rot_Z = 0;
            if(forward_directions[cur_index] == "u")
                movement = new Vector3(0, 1, 0);
            else if(forward_directions[cur_index] == "d")
                movement = new Vector3(0, -1, 0);
            else if(forward_directions[cur_index] == "l")
                movement = new Vector3(-1, 0, 0);
            else if(forward_directions[cur_index] == "r")
                movement = new Vector3(1, 0, 0);
            else movement = new Vector3(0, 0, 0);
            if(cur_index+1 < forward_directions.Length){
                
                string next_direction = forward_directions[cur_index+1];
                string current_direction = forward_directions[cur_index];
                if(next_direction == "s" || current_direction == "s"){
                    rot_Z = 0;
                }
                else if(current_direction == "u" && next_direction == "l"){
                    rot_Z = 90;
                }
                else if(current_direction == "u" && next_direction == "r"){
                    rot_Z = -90;
                }
                else if(current_direction == "d" && next_direction == "l"){
                    rot_Z = -90;
                }
                else if(current_direction == "d" && next_direction == "r"){
                    rot_Z = 90;
                }
                else if(current_direction == "l" && next_direction == "u"){
                    rot_Z = -90;
                }
                else if(current_direction == "l" && next_direction == "d"){
                    rot_Z = 90;
                }
                else if(current_direction == "r" && next_direction == "u"){
                    rot_Z = 90;
                }
                else if(current_direction == "r" && next_direction == "d"){
                    rot_Z = -90;
                }
            }
            movement *= delta;
            rot_Z *= delta;
            transform.position += movement;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z+rot_Z));
        }
    }
}
