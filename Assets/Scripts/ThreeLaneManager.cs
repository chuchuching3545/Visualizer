using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ThreeLaneManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject carPrefab;
    public List<GameObject> cars = new List<GameObject>();
    Vector3[] iniPosition = {new Vector3(3.5f, -7.5f, 0), new Vector3(4.5f, -7.5f, 0), new Vector3(5.5f, -7.5f, 0), new Vector3(20.5f, 10.5f, 0), new Vector3(20.5f, 11.5f, 0), new Vector3(20.5f, 12.5f, 0), new Vector3(2.5f, 27.5f, 0), new Vector3(1.5f, 27.5f, 0), new Vector3(0.5f, 27.5f, 0), new Vector3(-14.5f, 9.5f, 0), new Vector3(-14.5f, 8.5f, 0), new Vector3(-14.5f, 7.5f, 0),};
    Quaternion[] iniRotation = {Quaternion.Euler(0, 0, 270), Quaternion.Euler(0, 0, 270), Quaternion.Euler(0, 0, 270), Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, 0),Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, 90), Quaternion.Euler(0, 0, 90),Quaternion.Euler(0, 0, 90), Quaternion.Euler(0, 0, 180), Quaternion.Euler(0, 0, 180), Quaternion.Euler(0, 0, 180)};
    public float speed = 1;
    public float cur_time = 0;
    float wall_time = 0;
    public float delta;
    public GameObject traffic_light_manager;   
    bool isStart = false;
    public ArrayList fileLines = new ArrayList();
    public void start_simulate(){
        isStart = false;
        reset();
        traffic_light_manager.GetComponent<TrafficLightManager>().reset();
        int i = 0;
        for(; i < fileLines.Count; i += 2){
            string[] carData = ((string)fileLines[i]).Split(' ');
            if(carData[0] == "T") break;
            int id = int.Parse(carData[0]);
            int p = int.Parse(carData[1]);
            int l = int.Parse(carData[2]);
            int isHV = int.Parse(carData[3]);
            cars.Add(Instantiate(carPrefab, new Vector3(0, 0, 0), Quaternion.identity));
            GameObject sprite = cars[cars.Count-1].transform.GetChild(0).gameObject;
            sprite.transform.position = iniPosition[l];
            sprite.transform.rotation = iniRotation[l];
            sprite.GetComponent<controller>().id = id;
            sprite.GetComponent<controller>().p = p;
            if(isHV == 1){
                sprite.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else{
                sprite.GetComponent<SpriteRenderer>().color = Color.blue;
            }
            sprite.GetComponent<controller>().forward_directions = ((string)fileLines[i+1]).Split(' ');
            // cars[cars.Count-1].SetActive(false);
        }
        for(int j = 0; j < 12; ++j){
            traffic_light_manager.GetComponent<TrafficLightManager>().states.Add(null);
        }
        for(i = i+1; i < fileLines.Count-1; i += 2){
            string[] ids = ((string)fileLines[i]).Split(' ');
            string[] data = ((string)fileLines[i+1]).Split(' ');
            for(int j = 0; j < ids.Length; ++j){
                traffic_light_manager.GetComponent<TrafficLightManager>().states[int.Parse(ids[j])] = data;
            }
        }
        isStart = true;
    }
    // Update is called once per frame
    void Update()
    {
        if(!isStart) return;
        delta = Time.deltaTime; 
        wall_time += delta;
        cur_time += delta*speed;
        foreach(GameObject car in cars){
            if(car.transform.GetChild(0).gameObject.GetComponent<controller>().p <= cur_time && !car.transform.GetChild(0).gameObject.GetComponent<controller>().isAppear){
                car.transform.GetChild(0).gameObject.GetComponent<controller>().isAppear = true;
                car.SetActive(true);
                car.transform.GetChild(0).gameObject.GetComponent<controller>().pre_time = car.transform.GetChild(0).gameObject.GetComponent<controller>().p;
            }
            if(car.transform.GetChild(0).gameObject.transform.position.x > 20.5f || car.transform.GetChild(0).gameObject.transform.position.x < -14.5f || car.transform.GetChild(0).gameObject.transform.position.y > 27.5f || car.transform.GetChild(0).gameObject.transform.position.y < -7.5f){
                car.SetActive(false);
            }
            if(car.activeSelf){
                car.transform.GetChild(0).gameObject.GetComponent<controller>().move(cur_time);
            }
        }
        traffic_light_manager.GetComponent<TrafficLightManager>().step(cur_time);

    }

    void reset(){
        foreach(GameObject car in cars){
            Destroy(car);
        }
        cars.Clear();
        cur_time = 0;
    }
}
