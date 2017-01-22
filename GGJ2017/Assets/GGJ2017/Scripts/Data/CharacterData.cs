using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Custom/CharacterData")]
public class CharacterData : ScriptableObject {
    public string m_displayName;
    public Color m_color;

    public int bodySegments = 5;
    public int armSegments = 5;

    public AnimationCurve m_bodyCurve;
    public AnimationCurve m_armCurve;

    public Texture m_faceDefault;
    public Texture m_faceAngry;
    public Texture m_faceExcited;
    public Texture m_faceSad;
}
