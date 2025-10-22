## Coordinate System

In the Coordinate System section, an Object Pool was implemented.
Within a certain distance, when the user performs rock, scissors, or paper gestures, a red box, orange box, or light blue box is generated respectively.
A total of three boxes are created for each color. When a box falls or is placed in another coordinate, it becomes deactivated and recycled through the Object Pool.
The number of boxes does not exceed the amount specified in the initPoolSize variable of the Object Pool.
If all pooled boxes are already active, no new boxes are generated.

ObjectPool.cs
```csharp
// creates the pool (invoke when the lag is not noticeable)
    private void SetupPool()
    {
        // missing objectToPool Prefab field
        if (objectToPool == null)
        {
            return;
        }

        stack = new Stack<PooledObject>();

        // populate the pool
        PooledObject instance = null;

        for (int i = 0; i < initPoolSize; i++)
        {
            instance = Instantiate(objectToPool);
            instance.Pool = this;
            instance.gameObject.SetActive(false);
            stack.Push(instance);
        }
    }
```
GestureDectectorObjectPool.cs
```csharp
// Switch based on gesture type to spawn the corresponding object
            switch (gestureType)
            {
                case GestureType.Rock:
                    // Get an orange object from the pool
                    PooledObject orangePooledObject = orangeObjectPool.GetPooledObject();
                    if (orangePooledObject != null)
                    {
                        GameObject orangeObj = orangePooledObject.gameObject;
                        orangeObj.SetActive(true); // Activate the object
                        orangeObj.transform.SetPositionAndRotation(spawnPos, spawnRot); // Set position and rotation
                    }
                    break;
                case GestureType.Paper:
                    // Get a red object from the pool
                    PooledObject redPooledObject = redObjectPool.GetPooledObject();
                    if(redPooledObject != null)
                    {
                        GameObject redObj = redPooledObject.gameObject;
                        redObj.SetActive(true); // Activate the object
                        redObj.transform.SetPositionAndRotation(spawnPos, spawnRot); // Set position and rotation
                    }
                    break;
                case GestureType.Scissors:
                    // Get a light blue object from the pool
                    PooledObject lightBluePooledObject = lightBlueObjectPool.GetPooledObject();        
                    if(lightBluePooledObject != null)
                    {
                        GameObject lightBlueObj = lightBluePooledObject.gameObject;
                        lightBlueObj.SetActive(true); // Activate the object
                        lightBlueObj.transform.SetPositionAndRotation(spawnPos, spawnRot); // Set position and rotation
                    }
                    break;
            }
```

## License 
- All **source code** in this repository is licensed under the [MIT License](./LICENSE).
- Some code and assets are adapted from **Game Development II course materials**.
  Details are listed below.
- Third-party **assets** (models, textures, sounds, fonts, etc.) remain under their original licenses.
  They may **not be licensed for redistribution or in-game use** in this repository.  
  Please check the original source pages for specific license terms.

### Assets References
> Note: Third-party assets remain under their original licenses.
> They may **not be licensed for redistribution or in-game use** in this repository.  
> Please review each source page for license terms before redistribution or in-game use.


- Meta All-in-One SDK </br>
 Source: https://assetstore.unity.com/packages/tools/integration/meta-xr-all-in-one-sdk-269657
- VINTAGE LIVING ROOM 3D GAME PACK </br>
 Source: https://assetstore.unity.com/packages/3d/environments/vintage-living-room-3d-game-pack-314464
- Destructible Wooden Table </br>
 Source: https://assetstore.unity.com/packages/3d/props/furniture/destructible-wooden-table-260169
- Gold Coins </br>
 Source: https://assetstore.unity.com/packages/3d/props/gold-coins-1810
- Free Treasure Chest </br>
 Source: https://assetstore.unity.com/packages/3d/props/free-treasure-chest-313268
- Magic VFX-Ice (FREE) </br>
 Source: https://assetstore.unity.com/packages/vfx/particles/spells/magic-vfx-ice-free-170242

### Reference link I used for 3D modeling
- Puzzle: Jigsaw Puzzle Speed Modeling In Blender </br>
 Source: https://www.youtube.com/watch?v=fcuHhP9ck64
- Chair: [Blender Basic Lecture] Modeling 07 : Mirror </br>
 Source: https://www.youtube.com/watch?v=E1KgDQtkmc4&t=165s
- Table: Modeling Table Under 2Min || Blender Beginners Tutorial </br>
 Source: https://www.youtube.com/watch?v=8b7G0AWcV1k
- Abonden house: How to make an abandoned house in Blender - Tutorial </br>
 Source: https://www.youtube.com/watch?v=1aNnERnHRZg
- How to setup VR for META QUEST in Unity </br>
 Source: https://www.youtube.com/watch?v=NV9WzAfRFz4

### Here’s a useful reference video on how to set up hand tracking in Unity: </br>
 Source: https://www.youtube.com/watch?v=NV9WzAfRFz4
 
