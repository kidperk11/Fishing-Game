using UnityEditor;
using UnityEngine;

/**
 * Original Code from https://www.reddit.com/r/Unity2D/comments/libffv/ive_finally_done_it_a_custom_editor_based_on_enum/
 * 
 * Modified by Austin Garrison
 **/

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(BHTorpedoStats))]
public class ProjectileTypeInspector : PropertyDrawer
{
    //I use PropertyDrawer because Phase does not derived from MonoBehavior
    //I don'tuse monobehavior because I want Phase to be serializable


    public override void OnGUI(Rect position, SerializedProperty propertyInfo, GUIContent label)
    {
        //set height
        position.height = 20;//If you want line to be closer to each other in y direction, change here.

        //-----phaseName-----
        //var phaseNameProperty = propertyInfo.FindPropertyRelative("projectileName");
        //phaseNameProperty.stringValue = EditorGUI.TextField(position, "Projectile Name", phaseNameProperty.stringValue); 
        //position.y += position.height;//add height after phaseName

        //-----movementType-----
        //get movementType property
        var movementTypeProperty = propertyInfo.FindPropertyRelative("projectileEnumReference");

        //make a new movementType variable as a Phase.PhaseMovementType and set it equal to the property after convert
        BHTorpedoType movementType = (BHTorpedoType)movementTypeProperty.enumValueIndex;//get Index and type cast it back to enum

        //create popup to get the new value
        movementType = (BHTorpedoType)EditorGUI.EnumPopup(position, "", movementType);//""(label) is important. Else the title of the popup will appear

        //send back the new value
        movementTypeProperty.enumValueIndex = (int)movementType;

        //position.y += position.height;//add height after movementType field (not needed if the label is "")

        propertyInfo.isExpanded = EditorGUI.Foldout (position, propertyInfo.isExpanded, label);
        //using property.isExpanded is very convenient, but there's a catch. I can not have multiple foldout, because there's only 1 variable.
        //If I want multiple foldout, I may need a dictionary or something to store it instead.
        //However, since I only want 1 foldout (setting) that differ through each enum, it will works fine for now.

        //Depending on which enum value is selected on dropdown, inspector value changes. 
        if (propertyInfo.isExpanded)
        {
            position.y += position.height;
            if (movementType == BHTorpedoType.multiBullet)
            {
                //Properties for Multi-Shot Projectile
                //-----Multi-Bullet Amount-----
                var amountProperty = propertyInfo.FindPropertyRelative("multiBulletAmount");
                amountProperty.intValue = EditorGUI.IntSlider(position, "Amount", amountProperty.intValue, 1, 5);
                position.y += position.height;


                //-----Multi-Bullet Delay-----
                var delayProperty = propertyInfo.FindPropertyRelative("bulletFireDelay");
                delayProperty.floatValue = EditorGUI.FloatField(position, "Fire Delay", delayProperty.floatValue);
                position.y += position.height;

            }
            else if (movementType == BHTorpedoType.sprayBullet)
            {
                //Properties for Spray Projectile
                //-----Spray Bullet Fire Direction-----
                var directionProperty = propertyInfo.FindPropertyRelative("sprayBulletDirection");
                directionProperty.intValue = EditorGUI.IntField(position, "Fire Direction", directionProperty.intValue);
                position.y += position.height;


                //-----Spray Bullet Delay-----
                var delayProperty = propertyInfo.FindPropertyRelative("bulletFireDelay");
                delayProperty.floatValue = EditorGUI.FloatField(position, "Fire Delay", delayProperty.floatValue);
                position.y += position.height;

            }
            else if (movementType == BHTorpedoType.helixBullet)
            {
                //Properties for CrissCross Projectile

                //-----Helix Vertical Speed-----
                var verticalSpeedProperty = propertyInfo.FindPropertyRelative("verticalSpeed");
                verticalSpeedProperty.floatValue = EditorGUI.FloatField(position, "Vertical Speed", verticalSpeedProperty.floatValue);
                position.y += position.height;

                //-----Helix Range-----
                var verticalRangeProperty = propertyInfo.FindPropertyRelative("verticalRange");
                verticalRangeProperty.floatValue = EditorGUI.FloatField(position, "Vertical Range", verticalRangeProperty.floatValue);
                position.y += position.height;

                //-----Helix Delay-----
                var delayProperty = propertyInfo.FindPropertyRelative("bulletFireDelay");
                delayProperty.floatValue = EditorGUI.FloatField(position, "Fire Delay", delayProperty.floatValue);
                position.y += position.height;

            }
            else if (movementType == BHTorpedoType.remoteExplosive)
            {
                //Properties for CrissCross Projectile

                //-----Remote Explosive Delay-----
                var remoteExplodeTimerProperty = propertyInfo.FindPropertyRelative("autoDetonationTimer");
                remoteExplodeTimerProperty.floatValue = EditorGUI.FloatField(position, "Auto-Detonate Timer", remoteExplodeTimerProperty.floatValue);
                position.y += position.height;

                //-----Remote Explade Manual Delay-----
                //remoteExplodeFireDelay

                var manualExploderProperty = propertyInfo.FindPropertyRelative("remoteExplodeFireDelay");
                manualExploderProperty.floatValue = EditorGUI.FloatField(position, "Manual Explode Delay", manualExploderProperty.floatValue);
                position.y += position.height;
            }

            //-----Universal Lifetime-----
            var uniLifetimeProperty = propertyInfo.FindPropertyRelative("bulletLifetime");
            uniLifetimeProperty.floatValue = EditorGUI.FloatField(position, "Lifetime", uniLifetimeProperty.floatValue);
            position.y += position.height;

            //-----Universal  Damage-----
            var uniDamageProperty = propertyInfo.FindPropertyRelative("bulletDamage");
            uniDamageProperty.floatValue = EditorGUI.FloatField(position, "Damage", uniDamageProperty.floatValue);
            position.y += position.height;

            //-----Universal  Speed-----
            var uniSpeedProperty = propertyInfo.FindPropertyRelative("bulletSpeed");
            uniSpeedProperty.floatValue = EditorGUI.FloatField(position, "Speed", uniSpeedProperty.floatValue);
            position.y += position.height;
        }
    }

    //GetPropertyHeight is like getting the height of the whole box of script in the inspector before filling in
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return property.isExpanded ? 20*7 : 20*3;
        //kind of inconvenient but works for now. If I have more variable I have to adjust these number myself.
    }
}
#endif