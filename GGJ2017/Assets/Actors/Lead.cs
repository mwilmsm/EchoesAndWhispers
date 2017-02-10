﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SpriteRenderer))]
public class Lead : MonoBehaviour
{
    public List<GameObject> investigationObjects;

    public List<InvestigationYields> unrevealedConnectedLeads;
    public List<InvestigationYields> revealedConnectedLeads;

    public int StarterInvestigationCost = 3;
    public int InvestigationCost { get { return workingInvestigationCost; } }
    private int workingInvestigationCost;

    public List<Sprite> boyPics;
    public List<Sprite> girlPics;
    public bool generateId = true;

    public GameObject PortraitTarget;

    public static List<string> boyNames;
    private static List<string> girlNames;

    public string name;
    private List<Link> connections;
    public bool alive { get; set; }

    private bool isBoy;

    public List<ManualConnection> peopleConnections;


    // Use this for initialization
	void Start ()
	{
	    workingInvestigationCost = StarterInvestigationCost;
        if(unrevealedConnectedLeads == null)
            unrevealedConnectedLeads = new List<InvestigationYields>();

	    foreach (var investigationObject in investigationObjects)
	    {
	        var investigationYield = investigationObject.GetComponent<InvestigationYields>();
            if(investigationYield != null)
                unrevealedConnectedLeads.Add(investigationYield);
	    }

        /********** I have use for this, but not yet **********/
//	    if (boyNames == null)
//	    {
//	        var rawBoyNames = Resources.Load<TextAsset>("boy_names");
//	        boyNames = rawBoyNames.text.Split('\n').ToList();
//            
//	    }
//
//        if (girlNames == null)
//        {
//            var rawGirlNames = Resources.Load<TextAsset>("girl_names");
//            girlNames = rawGirlNames.text.Split('\n').ToList();
//        }
//
//        if(generateId)
//        {
//            var portRenderer = PortraitTarget.GetComponent<SpriteRenderer>();
//
//            isBoy = Mathf.FloorToInt(Random.value * 100) > 50;
//
//            if (isBoy)
//            {
//                name = boyNames[Mathf.FloorToInt((Random.value * boyNames.Count))];
//                if (portRenderer != null)
//                {
//                    portRenderer.sprite = boyPics[Mathf.FloorToInt((Random.value * boyPics.Count))];
//                }
//
//            }
//            else
//            {
//                name = girlNames[Mathf.FloorToInt((Random.value * boyNames.Count))];
//                if (portRenderer != null)
//                {
//                    portRenderer.sprite = girlPics[Mathf.FloorToInt((Random.value * girlPics.Count))];
//                }
//            }
//        }
//
//	    alive = true;
//
        if (connections == null)
            connections = new List<Link>();
    }

    public InvestigationYields drawRandomEvent()
    {
        if (!unrevealedConnectedLeads.Any())
        {
            return null;
        }

        var randSlot = Mathf.FloorToInt(Random.value) % (unrevealedConnectedLeads.Count - 1);
        var selected = unrevealedConnectedLeads[randSlot];

        revealedConnectedLeads.Add(selected);
        unrevealedConnectedLeads.Remove(selected);

        return selected;    
    }

    public investigationSize GetInvestigationSize()
    {
        var remaining = unrevealedConnectedLeads.Count;
        if( remaining < 2)
            return investigationSize.SMALL;
        else if (remaining >= 2 && remaining < 5)
            return investigationSize.MEDIUM;
        else if (remaining >= 5 && remaining < 7)
            return investigationSize.LARGE;
        else 
            return investigationSize.BOTTOMLESS;
    }

    public void recieveLink(Link link)
    {
        if (connections == null)
            connections = new List<Link>();
        Lead other = this.getOtherLeadFromLink(link);
        if(!this.isLinkedTo(other))
            connections.Add(link);
    }

    public Lead getOtherLeadFromLink(Link link)
    {
        Lead otherLead;
        if (link.leadA == this)
            otherLead = link.leadB;
        else
            otherLead = link.leadA;
        return otherLead;
    }

    public bool isLinkedTo(Lead lead)
    {
        if (connections == null)
            return false;
        return connections.Exists(p => p.leadA == lead || p.leadB == lead);
    }

    public List<ManualConnection> showStarterLinks()
    {
        return peopleConnections;
    }

    public void createLinkWith(Lead lead)
    {
        Link newLink = new Link();
        newLink.recieveTargets(this, lead);
        this.recieveLink(newLink);
    }

    public void die()
    {
        this.alive = false;
    }

    public List<Link> getConnections()
    {
        return this.connections;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void move(Vector3 newloc)
    {
        this.transform.position = new Vector3(newloc.x, newloc.y, 0);
    }
}

[Serializable]
public class ManualConnection
{
    public bool visible;
    public GameObject person;
}


public enum investigationSize
{
    SMALL,
    MEDIUM,
    LARGE,
    UNTOUCHED,
    BOTTOMLESS
}
