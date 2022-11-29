using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageRecog : MonoBehaviour
{



    [SerializeField]
    private GameObject[] arObjectsToPlace;
  
    private ARTrackedImageManager m_TrackedImageManager;

    public Dictionary<string, GameObject> arObjects = new Dictionary<string, GameObject>();

    

    void Awake()
    {

        m_TrackedImageManager = GetComponent<ARTrackedImageManager>();


        //  setup all game objects in dictionary
        foreach (GameObject arObject in arObjectsToPlace)
        {
            GameObject newARObject = Instantiate(arObject, Vector3.zero, Quaternion.identity);
            newARObject.name = arObject.name;
            arObjects.Add(arObject.name, newARObject);
            newARObject.SetActive(false);

           
            //newARObject.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);



        }

        m_TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;


       
    }

    private void OnEnable()
    {
    
    }

    private void Update()
    {

     
        
    }
    void OnDisable()
    {
        m_TrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;

    }


    private void OnDestroy()
    {
        m_TrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

     
    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
             

              UpdateARImage(trackedImage);


        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {

            

            if (trackedImage.trackingState == TrackingState.Tracking)
            {

                
                 UpdateARImage(trackedImage);

                

            }
            else
            {

               

                arObjects[trackedImage.referenceImage.name].SetActive(false);

               
            }

        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
           

            arObjects[trackedImage.referenceImage.name].SetActive(false);

          
 

            

        }
    }


   
    private void UpdateARImage(ARTrackedImage trackedImage)
    {

     
        // Assign and Place Game Object
      StartCoroutine(AssignGameObject(trackedImage.referenceImage.name, trackedImage.transform.position, trackedImage.transform.rotation));


    }

 
   

    void OnApplicationFocus(bool hasFocus)
    {
      
        if (hasFocus)
        {
          
           
        }

    }

    void OnApplicationPause(bool pauseStatus)
    {

    }


   
    IEnumerator AssignGameObject(string name, Vector3 newPosition, Quaternion rot)
    {

       yield return new WaitForSeconds(1.2f);

        if (arObjectsToPlace != null)
        {
           
            GameObject goARObject = arObjects[name];
          
            goARObject.SetActive(true);
            
            goARObject.transform.position = newPosition;
            goARObject.transform.rotation = rot;
            
          
         
            
            foreach (GameObject go in arObjects.Values)
            {

                if (go.name != name)
                {
                    go.SetActive(false);
                }
            }
        }
    }


    

}
