﻿using UnityEngine;
using System.Collections;
using Mirror;

/**
 * Manages trading between players.
 * Player who is currently having a turn may be able to ask for a certain resouce.
 * Other players may send offers in exchange for that resource (one at a time for networking ease)
 */
public class PlayerTradeManager : NetworkBehaviour
{
    [SyncVar]
    public bool trading = false;

    [SyncVar]
    public bool requesterAccept = false;
    [SyncVar]
    public bool offererAccept = false;

    [SyncVar]
    public string requesterId = "";
    [SyncVar]
    public int requestLumber = 0;
    [SyncVar]
    public int requestWool = 0;
    [SyncVar]
    public int requestGrain = 0;
    [SyncVar]
    public int requestBrick = 0;
    [SyncVar]
    public int requestOre = 0;

    [SyncVar]
    public string offererId = "";
    [SyncVar]
    public int offerLumber = 0;
    [SyncVar]
    public int offerWool = 0;
    [SyncVar]
    public int offerGrain = 0;
    [SyncVar]
    public int offerBrick = 0;
    [SyncVar]
    public int offerOre = 0;

    public void MakeTradeOffer(string offererId, int lumber, int wool, int grain, int brick, int ore)
    {
        this.offererId = offererId;
        this.offerLumber = lumber;
        this.offerWool = wool;
        this.offerGrain = grain;
        this.offerBrick = brick;
        this.offerOre = ore;
    }

    public void AcceptOffer()
    {

    }
}