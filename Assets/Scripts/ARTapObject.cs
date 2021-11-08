using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]

public class ARTapObject : MonoBehaviour
{
    public GameObject object_prefab;

    private GameObject spawned_obj;
    private ARRaycastManager _arRycastManager;
    private bool SpawnOn = false;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // Start is called before the first frame update
    void Awake()
    {
        _arRycastManager = GetComponent<ARRaycastManager>(); 
    }


    bool GetTouchPos(out Vector2 touchPos)
    {
        if(Input.touchCount > 0)
        {
            touchPos = Input.GetTouch(index: 0).position;
            return true;
        }

        touchPos = default;
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        //if no touch, just go to return
        if (!GetTouchPos(out Vector2 touchPosition))
        {
            SpawnOn = true;
            return;
        }

        if(_arRycastManager.Raycast(touchPosition, hits, trackableTypes: TrackableType.PlaneWithinPolygon))
        {
            var hit_pose = hits[0].pose;

            if (SpawnOn)
            {
                spawned_obj = Instantiate(object_prefab, hit_pose.position, Quaternion.identity);
                //Animation to make this good
                Invoke("CallWaveAnim", 1f);
                Invoke("CallPickupAnim", 0.5f);
                
                SpawnOn = false;
            }
        }
    }

    void CallPickupAnim()
    {
        Animator animTrigger = spawned_obj.GetComponent<Animator>();
        animTrigger.SetBool("Pickup", true);
    }

    void CallWaveAnim()
    {
        Animator animTrigger = spawned_obj.GetComponent<Animator>();
        animTrigger.SetBool("Wave", true);
    }
}
