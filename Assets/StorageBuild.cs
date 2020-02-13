using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageBuild : InteractiveBuild
{
    public float storeFood;
    public float maxFood = 8;
    public float storeCoffee;
    public float maxCoffee = 8;
    protected override bool CharacterAction(CharacterController character)
    {
        bool end = true;

        foreach (InteractiveController.BonusResources resource in ((InteractiveController)build).resources)
        {
            float incrValue;
            switch (resource.resource)
            {
                case Resources.Energy:
                    incrValue = Time.deltaTime * resource.value * (((InteractiveController) build).maxCharacter-characters.Count)/(build._manager.timeScale * ((InteractiveController) build).maxCharacter);
                    if (storeCoffee >= incrValue && character.energy < 1)
                    {
                        storeCoffee -= incrValue;
                        character.energy = Mathf.Min(1,character.energy + incrValue);
                    }
                    
                    end &= character.energy >= 1 || storeCoffee <= 0;
                    break;
                
                case Resources.Food :
                    incrValue = Time.deltaTime * resource.value * (((InteractiveController) build).maxCharacter-characters.Count)/(build._manager.timeScale * ((InteractiveController) build).maxCharacter);
                    if (storeFood >= incrValue && character.food < 1)
                    {
                        storeFood -= incrValue;
                        character.food = Mathf.Min(1,character.food + incrValue);
                    }
                    
                    end &= character.food >= 1|| storeFood <= 0;;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return end;
    }
    
}
