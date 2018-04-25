using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InterpolatedCamera
{
    public enum CreatableWorldObjectTypes
    {
        Sphere,
        Cube
    }

    public enum AnimationTypes
    {
        None,
        Ball
    }

    public class WorldObjectCreator : MonoBehaviour
    {
        public GameObject WorldObjectParent;
        public CreatableWorldObjectTypes objectType;
        public Color color = Color.white;
        public AnimationTypes Behavior;
        public Vector3 StartPosition = new Vector3(0, 0, 2.5f);
        public Vector3 Size = new Vector3(0.1f, 0.1f, 0.1f);
        
        public GameObject CreateWorldObject()
        {
            GameObject worldObj = new GameObject();

            switch (objectType)
            {
                case CreatableWorldObjectTypes.Sphere:
                    GameObject.DestroyImmediate(worldObj);
                    worldObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    break;
                case CreatableWorldObjectTypes.Cube:
                    GameObject.DestroyImmediate(worldObj);
                    worldObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    break;
                default:
                    GameObject.DestroyImmediate(worldObj);
                    return null;
            }

            worldObj.SetActive(false);
            worldObj.name = objectType.ToString();

            worldObj.transform.parent = WorldObjectParent.transform;

            var mr = worldObj.GetComponent<MeshRenderer>();
            mr.material.SetColor("_Color", color);
            worldObj.transform.localScale = Size;
            worldObj.transform.localPosition = StartPosition;

            AssignBehavior(worldObj, Behavior);

            worldObj.SetActive(true);
            return worldObj;
        }

        public string GetAnimationResourceLoadString(AnimationTypes behaviorType)
        {
            string str = behaviorType.ToString();
            return "Animations/" + str;
        }

        public void AssignBehavior(GameObject obj, AnimationTypes behavior)
        {
            if (behavior == AnimationTypes.None)
            {
                var rb = obj.AddComponent<Rigidbody>();
                rb.useGravity = true;
            }
            else
            {
                var animator = obj.AddComponent<Animator>();
                animator.runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load(GetAnimationResourceLoadString(behavior), typeof(RuntimeAnimatorController)));
            }
        }
    }
}