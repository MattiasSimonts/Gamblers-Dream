using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class SlotMachineManager : MonoBehaviour
{
   [SerializeField] private string _slotMachineTag = "SlotMachine";
   [SerializeField] private float _minDistance = 10f;
   
   private Transform _player;
    private List<SlotMachine> _slotMachines = new List<SlotMachine>();

    private void Awake()
    {
        print("test");
        SlotMachine.Manager = this;
        GameObject player = GameObject.FindWithTag("Player");
        if(player is not null)
            _player = player.transform;

        var slotMachineObjects = GameObject.FindGameObjectsWithTag(_slotMachineTag);
        foreach (var slotMachineObject in slotMachineObjects)
            if(slotMachineObject.TryGetComponent(out SlotMachine slotMachine))
                if(slotMachine.isTutorialMachine)
                    _slotMachines.Add(slotMachine);
    }

    public void SwitchMachine()
    {
        
        List<SlotMachine> availableSpawns = _slotMachines.Where(slotMachine =>
            Vector3.Distance(_player.position, slotMachine.transform.position) > _minDistance && 
            Vector3.Distance(GetActiveSlotMachineTransform().position, slotMachine.transform.position) < _minDistance  &&
            slotMachine.IsEnabled ).ToList() ?? new List<SlotMachine>();
        
        if(availableSpawns.Count == 0) return;
        print("test");
        availableSpawns[UnityEngine.Random.Range(0, availableSpawns.Count)].IsEnabled = true;
    }

    private Transform GetActiveSlotMachineTransform()
    {
        foreach (SlotMachine slotMachine in _slotMachines)
            if(slotMachine.IsEnabled) 
                return slotMachine.transform;
        return _player;
    }
}