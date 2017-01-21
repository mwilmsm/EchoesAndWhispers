﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Person : MonoBehaviour
{

    public string name;
    private List<Link> connections;
    public bool alive { get; set; }
    public List<Rumor> rumors;

    public List<GameObject> peopleConnections; 

	// Use this for initialization
	void Start ()
	{
	    name = (Random.value*100).ToString();
        if(connections == null)
            connections = new List<Link>();
    }

    public void recieveLink(Link link)
    {
        if (connections == null)
            connections = new List<Link>();
        Person other = this.getOtherPersonFromLink(link);
        if(!this.isLinkedTo(other))
            connections.Add(link);
    }

    public Person getOtherPersonFromLink(Link link)
    {
        Person otherPerson;
        if (link.personA == this)
            otherPerson = link.personB;
        else
            otherPerson = link.personA;
        return otherPerson;
    }

    public bool isLinkedTo(Person person)
    {
        if (connections == null)
            return false;
        return connections.Exists(p => p.personA == person || p.personB == person);
    }

    public List<GameObject> showStarterLinks()
    {
        return peopleConnections;
    }

    public void breakLinkWith(Person person)
    {
        connections.ForEach((Link linkToPerson) => {
            if (linkToPerson.personA == person || linkToPerson.personB == person)
            {
                this.connections.Remove(linkToPerson);
            }
        });
    }

    public void breakLink(Link link)
    {
        connections.Remove(link);
    }

    public void createLinkWith(Person person)
    {
        Link newLink = new Link();
        newLink.recieveTargets(this, person);
        this.recieveLink(newLink);
    }

    public void die()
    {
        this.alive = false;
    }

    public void infect(Rumor rumor)
    {
        this.rumors.Add(rumor);
        // TODO: Any other rumor actions.
    }

    public bool isInfectedByRumor(Rumor rumor)
    {
        return this.rumors.Contains(rumor);
    }

    public List<Link> getConnections()
    {
        return this.connections;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
