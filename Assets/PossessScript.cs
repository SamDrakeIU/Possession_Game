using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Hierarchy;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PossessScript : MonoBehaviour
{
    public TextMeshProUGUI possessPrompt;
    public TextMeshProUGUI depossessPrompt;
    
    public ParticleSystem poof;


    private List<GameObject> possessableObjects;

    private String possessedName;
    private int possessableIndex;
    private GameObject possessedObject;
    private bool possessing;

    private Vector3 positionDiff;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        possessing = false;
        possessableIndex = 0;
        possessPrompt.enabled = false;
        depossessPrompt.enabled = false;
        possessableObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Mouse.current != null && 
            Mouse.current.scroll.IsActuated() && 
            possessableObjects.Count() > 1)
        {
            int i = (int)MathF.Round(Input.GetAxis("Mouse ScrollWheel"), MidpointRounding.AwayFromZero);
            SwitchPossessedObject(i);
        }
        
        
        if (Keyboard.current != null && 
            Keyboard.current.eKey.wasPressedThisFrame && 
            possessableObjects.Count() > 0)
        {
            poof.Play();
            if(!possessing)
            {
                Possess();
            } else
            {
                Depossess();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Possessable"))
        {
            possessableObjects.Add(other.gameObject);
            if(!possessing)
            {
                possessPrompt.enabled = true;
                SetPossessedName(other.gameObject);
                possessPrompt.text = "Press E to Possess " + possessedName;
                depossessPrompt.text = "Press E to Depossess" + possessedName;
                possessedObject = possessableObjects[0];
            }
        }
        else if(other.CompareTag(Variables.Object(possessedObject).Get<string>("Zone"))
                && possessing)
        {
            float x = other.transform.position.x;
            float z = other.transform.position.z;
            float y = possessedObject.transform.position.y;
            Vector3 snapTo = new Vector3(x, y, z);
            possessedObject.transform.position = snapTo;
            Depossess();
        }
    }

    void OnTriggerExit(Collider possessable)
    {
        if(possessable.CompareTag("Possessable"))
        {
            if(possessableObjects.Remove(possessable.gameObject))
            {
                Console.WriteLine("Object Removed from list");
                Console.WriteLine("Possessable objects = " + possessableObjects.Count());
            }
            
            if(possessableObjects.Count == 0)
            {
                possessPrompt.enabled = false;
            }
            else
            {
                SwitchPossessedObject(0);
            }
        }
    }

    private void DisableRenderers()
    {
        foreach(Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.enabled = false; 
        }
    }
    
    private void EnableRenderers()
    {
        foreach(Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.enabled = true;
        }
    }

    private void SwitchPossessedObject(int i)
    {
        possessableIndex = (possessableIndex + i) % possessableObjects.Count();
        possessedObject = possessableObjects[possessableIndex];
        SetPossessedName(possessedObject);
        possessPrompt.text = "Press E to Possess " + possessedName;
    }

    private void SetPossessedName(GameObject possessable)
    {
        possessedName = Variables.Object(possessable).Get<string>("Name");
    }

    private void Possess()
    {
        positionDiff = transform.position - possessedObject.transform.position;
        transform.position = possessedObject.transform.position;
        possessPrompt.enabled = false;
        depossessPrompt.enabled = true;
        DisableRenderers();
        possessing = true;
        possessedObject.transform.SetParent(this.transform);
    }
    private void Depossess()
    {
        possessPrompt.enabled = true;
        depossessPrompt.enabled = false;
        EnableRenderers();
        possessing = false;
        possessedObject.transform.SetParent(null);
        transform.position += positionDiff;
    }
}
