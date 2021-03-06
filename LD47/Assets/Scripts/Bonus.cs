﻿using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    [SerializeField] private const float DefaultMiningModifier = 1.5f;

    [SerializeField] private const float DefaultMiningTimingModifier = 5.0f;

    [SerializeField] private const float DefaultImmortalityTimingModifier = 5.0f;

    [SerializeField] private const bool DefaultImmortalityModifier = true;
    [SerializeField] private Cell cell;

    [SerializeField] private GlobalController globalController;

    public enum BonusType
    {
        Immortality,
        DamageBonus,
        Swap,
        CrossBomb,
        SplashBomb
    }

    public BonusType myBonusType;

    public void EnableBonus(
        Character character,
        bool immortalityModifier = DefaultImmortalityModifier,
        float miningModifier = DefaultMiningModifier, 
        float timingModifier = DefaultImmortalityTimingModifier)
    {
        cell = character.curCell;
        switch (myBonusType)
        {
            case BonusType.Immortality:
                EnableImmortality(character);
                break;
            case BonusType.DamageBonus:
                EnableDamageBonus(character);
                break;
            case BonusType.Swap:
                //
                break;
            default:
                break;
        }
    }

    void EnableDamageBonus(
        Character character, 
        float miningModifier = DefaultMiningModifier, 
        float timingModifier = DefaultMiningTimingModifier)
    {
        character.curDamage *= miningModifier;
        StartCoroutine(DisableDamageBonus(character, miningModifier, timingModifier));
    }

    IEnumerator DisableDamageBonus(
        Character character,
        float miningModifier = DefaultMiningModifier,
        float timingModifier = DefaultMiningTimingModifier)
    {
        yield return new WaitForSeconds(timingModifier);
        character.curDamage /= miningModifier;
    }
    
    void EnableImmortality(
        Character character,
        bool immortalityModifier = DefaultImmortalityModifier,
        float timingModifier = DefaultImmortalityTimingModifier)
    {
        character.immortal = DefaultImmortalityModifier;
        StartCoroutine(DisableImmortality(character, immortalityModifier));
    }

    IEnumerator DisableImmortality(
        Character character, 
        bool immortalityModifier = DefaultImmortalityModifier,
        float timingModifier = DefaultImmortalityTimingModifier)
    {
        yield return new WaitForSeconds(timingModifier);
        character.immortal = !immortalityModifier;
    }

    void EnableSplashBomb(
        Character character, 
        float miningModifier = DefaultMiningModifier, 
        float timingModifier = DefaultMiningTimingModifier)
    {
        StartCoroutine(DisableSplashBomb(character, miningModifier, 3f));
    }

    IEnumerator DisableSplashBomb(
        Character character,
        float miningModifier = DefaultMiningModifier,
        float timingModifier = DefaultMiningTimingModifier)
    {
        yield return new WaitForSeconds(3f);

        foreach (KeyValuePair<Cell.Direction, Cell> item in cell.NeighborCells)
        {
            item.Value.Dead();
        }
    }
    
    void EnableCrossBomb(
        Character character, 
        float miningModifier = DefaultMiningModifier, 
        float timingModifier = DefaultMiningTimingModifier)
    {
        StartCoroutine(DisableSplashBomb(character, miningModifier, 3f));
    }
    
    IEnumerator DisableCrossBomb(
        Character character,
        float miningModifier = DefaultMiningModifier,
        float timingModifier = DefaultMiningTimingModifier)
    {
        yield return new WaitForSeconds(3f);

        //if(cell.IsExist())
    }
    
    
    void Swap()
    {
        globalController.SwapCharacters();
        // TODO: Write controlling class
        // Which will perform these changes.
    }
    
}
