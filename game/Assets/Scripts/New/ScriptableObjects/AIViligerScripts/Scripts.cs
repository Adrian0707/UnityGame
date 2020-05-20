using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class Scripts : ScriptableObject
{
    [TextArea(15, 20)]
    public string viligerInteraction;
    public bool viligerInteractionNormalMode;

    [TextArea(15, 20)]
    public string viligerLookingForJobWithTool;
    public bool viligerLookingForJobWithToolNormalMode;

}