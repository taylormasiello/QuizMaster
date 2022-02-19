using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    int correctAnswers = 0;
    int questionsSeen = 0;

    public int GetCorrectAnswers() // getter 
    {
        return correctAnswers;
    }

    public void IncrementCorrectAnswers() // setter 
    {
        correctAnswers++;
    }

    public int GetQuestionsSeen()
    {
        return questionsSeen;
    }

    public void IncrementQuestionsSeen()
    {
        questionsSeen++;
    }

    public int CalculateScore()
    {
        return Mathf.RoundToInt((correctAnswers / (float)questionsSeen) * 100); 

        // Mathf class of namespace System, has many mathmatical operations available; THIS Mathf is a UNITY struct, containing .RoundToInt() https://docs.unity3d.com/540/Documentation/ScriptReference/Mathf.html
        // ".RoundToInt" returns f rounded to nearest int; if f ends in .5 thus halfway between 2 int's, one of which is even and the other is odd, will return the even number https://docs.unity3d.com/540/Documentation/ScriptReference/Mathf.RoundToInt.html
    }
}
