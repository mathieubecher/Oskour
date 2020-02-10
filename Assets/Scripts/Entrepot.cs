using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrepot : BuildController
{
    public float stockFood;
    public float stockCoffee;

    protected override void ActiveComportement()
    {
        int i = 0;
        while (i < characters.Count)
        {
            CharacterController character = characters[i];
            stockFood -= bonusTime / ((float)maxCharacter * manager.timeScale) * Time.deltaTime;
            stockCoffee -= bonusTime / ((float)maxCharacter * manager.timeScale) * Time.deltaTime;
            bool deletecharacter = true;

            if (stockFood > 0)
            {
                deletecharacter &= AddResourcesCharacter(character, character.food, out character.food, bonusTime / ((float)maxCharacter * manager.timeScale) * Time.deltaTime);
            }

            if (stockCoffee > 0)
            {
                deletecharacter &= AddResourcesCharacter(character, character.energy, out character.energy, bonusTime / ((float)maxCharacter * manager.timeScale) * Time.deltaTime);
            }
            if (!deletecharacter) ++i;
        }
    }
}
