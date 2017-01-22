﻿using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Link : MonoBehaviour
{
    public Person personA;
    public Person personB;
    public bool Visible = true;
    public GameObject drawLinkPiece;
    public float baseScale = 1f;

    public int numPieces = 10;

    private bool mouseLinked;
    private float LinkWidth = .5f;

    private bool wasVisible = true;
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < numPieces; i++)
        {
            var temp = GameObject.Instantiate(drawLinkPiece);

            temp.transform.position = transform.position;
            temp.transform.localPosition = new Vector3(LinkWidth, 0) * i;
            temp.transform.parent = this.gameObject.transform;

        }
    }

    public void recieveTargets(Person a, Person b)
    {        personA = a;
        personB = b;
    }

    public void recieveTargets(Person a)
    {
        personA = a;
        mouseLinked = true;        Visible = true;
    }

    public void passToPersonTarget(Person b)
    {
        personB = b;
        mouseLinked = false;    }

    public bool contains(Person person)
    {
        return (personA == person || personB == person);
    }

    // Update is called once per frame
    void Update()
    {        if (personA != null && (personB != null || mouseLinked))
        {
            var placea = personA.transform.position;
            Vector3 placeb;
            if(!mouseLinked)
            {
                 placeb = personB.transform.position;
            }
            else
            {
                placeb = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            this.transform.position = placea;
            var dist = placeb - placea;


            for (int i = 0; i < transform.childCount; i++)

            {
                var temp = transform.GetChild(i);
                temp.transform.localPosition = new Vector3(dist.magnitude/numPieces, 0)*i;
            }

            dist.Normalize();
            float rot_z = Mathf.Atan2(dist.y, dist.x)*Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(rot_z, Vector3.forward);

        }
        if (Visible && !wasVisible)        {
            for (int i = 0; i < transform.childCount; i++)
            {                var temp = transform.GetChild(i);
                temp.gameObject.SetActive(true);            }
        }
        if (!Visible && wasVisible)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var temp = transform.GetChild(i);
                temp.gameObject.SetActive(false);
            }
        }

        wasVisible = Visible;

    }
}