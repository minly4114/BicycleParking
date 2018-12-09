using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIController : MonoBehaviour {

    public ElevatorSystem system;
    public GameObject bicyclePrefab;
    public GameObject spawnPoint;
    public GameObject Camera;
    public GameObject textStatus;
    public GameObject dropdown;

    private GameObject lastSpawnedBicycle;
    private GameObject pickedBicycle;
    private new Camera camera;   
    private bool isAttach = false;
    

    private void Start()
    {
        camera = Camera.GetComponent<Camera>();
    }

    private void Update()
    {
        if(isAttach)
        {
            if (lastSpawnedBicycle.GetComponent<FixedJoint>().connectedBody == null)
            {
                lastSpawnedBicycle.transform.position -= lastSpawnedBicycle.transform.right * 0.03f;
            } else
            {
                isAttach = false;
            }
        }
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            if(hit.collider.tag == "Bicycle")
            {
                pickedBicycle = hit.collider.gameObject;
            }
        }
    }

    public void SpawnPressed()
    {
        lastSpawnedBicycle = Instantiate(bicyclePrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
    }

    public void AttachPressed()
    {
        isAttach = true;
    }

    public void DeletePressed()
    {
        if(pickedBicycle != null)
        {
            Destroy(pickedBicycle);
        }
    }

    public void PromptStatus(string msg)
    {
        textStatus.GetComponent<TMPro.TextMeshProUGUI>().text = msg;
    }

    public void PromptBicycles(List<string> options)
    {
        var dd = dropdown.GetComponent<Dropdown>();
        dd.AddOptions(options);
    }

    public void PickedValue(int value)
    {
        if(value == 0)
        {
            int picked = int.Parse(dropdown.GetComponent<Dropdown>().captionText.text);
            system.SelectedParking(picked);
        }
        else
        {
            system.ParkingBicycle();
        }
        dropdown.GetComponent<Dropdown>().ClearOptions();
    }
}
