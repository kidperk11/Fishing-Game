using UnityEditor;
using UnityEngine;

/**Original Code from https://www.reddit.com/r/Unity2D/comments/libffv/ive_finally_done_it_a_custom_editor_based_on_enum/
 * 
 * Modified by Austin Garrison
 **/

[CustomPropertyDrawer(typeof(ProjectileType))]
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
        ProjectileType.ProjectilesEnum movementType = (ProjectileType.ProjectilesEnum)movementTypeProperty.enumValueIndex;//get Index and type cast it back to enum

        //create popup to get the new value
        movementType = (ProjectileType.ProjectilesEnum)EditorGUI.EnumPopup(position, "", movementType);//""(label) is important. Else the title of the popup will appear

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
            if (movementType == ProjectileType.ProjectilesEnum.multiBullet)
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

                //-----Multi-Bullet Lifetime-----
                var lifetimeProperty = propertyInfo.FindPropertyRelative("bulletLifetime");
                lifetimeProperty.floatValue = EditorGUI.FloatField(position, "Lifetime", lifetimeProperty.floatValue);
                position.y += position.height;
                
                //-----Multi-Bullet Damage-----
                var damageProperty = propertyInfo.FindPropertyRelative("bulletDamage");
                damageProperty.floatValue = EditorGUI.FloatField(position, "Damage", damageProperty.floatValue);
                position.y += position.height; 
                
                //-----Multi-Bullet Speed-----
                var speedProperty = propertyInfo.FindPropertyRelative("bulletSpeed");
                speedProperty.floatValue = EditorGUI.FloatField(position, "Speed", speedProperty.floatValue);
                position.y += position.height;
            }
            else if (movementType == ProjectileType.ProjectilesEnum.sprayBullet)
            {
                //Properties for Spray Projectile
                //-----Spray Bullet Fire Direction-----
                var directionProperty = propertyInfo.FindPropertyRelative("sprayBulletDirection");
                directionProperty.intValue = EditorGUI.IntField(position, "Fire Direction", directionProperty.intValue);
                position.y += position.height;

                //-----Spray Delay-----
                var delayProperty = propertyInfo.FindPropertyRelative("bulletFireDelay");
                delayProperty.floatValue = EditorGUI.FloatField(position, "Fire Delay", delayProperty.floatValue);
                position.y += position.height;

                //-----Spray Lifetime-----
                var lifetimeProperty = propertyInfo.FindPropertyRelative("bulletLifetime");
                lifetimeProperty.floatValue = EditorGUI.FloatField(position, "Lifetime", lifetimeProperty.floatValue);
                position.y += position.height;

                //-----Spray Damage-----
                var damageProperty = propertyInfo.FindPropertyRelative("bulletDamage");
                damageProperty.floatValue = EditorGUI.FloatField(position, "Damage", damageProperty.floatValue);
                position.y += position.height;

                //-----Spray Speed-----
                var speedProperty = propertyInfo.FindPropertyRelative("bulletSpeed");
                speedProperty.floatValue = EditorGUI.FloatField(position, "Speed", speedProperty.floatValue);
                position.y += position.height;
            }
            else if (movementType == ProjectileType.ProjectilesEnum.crissCrossBullet)
            {
                //Properties for CrissCross Projectile

                //-----CrissCross Delay-----
                var delayProperty = propertyInfo.FindPropertyRelative("bulletFireDelay");
                delayProperty.floatValue = EditorGUI.FloatField(position, "Fire Delay", delayProperty.floatValue);
                position.y += position.height;

                //-----CrissCross Lifetime-----
                var lifetimeProperty = propertyInfo.FindPropertyRelative("bulletLifetime");
                lifetimeProperty.floatValue = EditorGUI.FloatField(position, "Lifetime", lifetimeProperty.floatValue);
                position.y += position.height;

                //-----CrissCross Damage-----
                var damageProperty = propertyInfo.FindPropertyRelative("bulletDamage");
                damageProperty.floatValue = EditorGUI.FloatField(position, "Damage", damageProperty.floatValue);
                position.y += position.height;

                //-----CrissCross Speed-----
                var speedProperty = propertyInfo.FindPropertyRelative("bulletSpeed");
                speedProperty.floatValue = EditorGUI.FloatField(position, "Speed", speedProperty.floatValue);
                position.y += position.height;
            }
        }
    }

    //GetPropertyHeight is like getting the height of the whole box of script in the inspector before filling in
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return property.isExpanded ? 20*6 : 20*3;
        //kind of inconvenient but works for now. If I have more variable I have to adjust these number myself.
    }
}
