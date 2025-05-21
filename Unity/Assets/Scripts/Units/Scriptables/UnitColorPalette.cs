using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/UnitColorPalette", fileName = "Unit Color Palette")]
public class UnitColorPalette : ScriptableObject
{
    public Material face;
    public Material belt;
    public Material suit;
    public Material hand;

    public Material[] GetMaterial(UnitTypes unitType)
    {
        switch (unitType)
        {
            case UnitTypes.Tanker:
                //Face-Belt-Suit-Hand
                Material[] result = new Material[4];
                result[0] = face;
                result[1] = belt;
                result[2] = suit;
                result[3] = hand;
                return result;
            case UnitTypes.Dealer:
                //Face-Belt-Suit-Hand-Hand
                Material[] dealerResult = new Material[5];
                dealerResult[0] = face;
                dealerResult[1] = belt;
                dealerResult[2] = suit;
                dealerResult[3] = hand;
                dealerResult[4] = hand;
                return dealerResult;
            case UnitTypes.Healer:
                //Belt-Suit-Hand-Face
                Material[] healerResult = new Material[4];
                healerResult[0] = belt;
                healerResult[1] = suit;
                healerResult[2] = hand;
                healerResult[3] = face;
                return healerResult;
        }
        return null;
    }
}
