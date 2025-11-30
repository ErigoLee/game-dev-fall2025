## Note on Contributors
Because the Game Center uses a **shared desktop environment**, there were cases where Git was used while another account was still logged in.  
As a result, some commits were accidentally attributed to a different user (e.g., **nathanPayson**).  
Since removing that contributor would also delete important commits from the project’s history, the contributor entry has been intentionally **left as is**.


# Assignment 1 - Unity Review: Pacman
**Branch version:** `Assign1`</br>
[Play the Game](https://erigolee.github.io/game-dev-fall2025-Pacman/Pacman_Builds/) </br>
[Description](https://github.com/ErigoLee/game-dev-fall2025/blob/Assign2/README.md) </br>

# Assignment - 2 OOP Inventory
**Branch version:** `Assign2`</br>
[Play the Game](https://erigolee.github.io/game-dev-fall2025-Pacman/Pacman_version2_Builds/) </br>
[Description](https://github.com/ErigoLee/game-dev-fall2025/blob/Assign2/README.md) </br>

# Assignment - 3 Your Design Pattern
**Branch version:** `Assign3`</br>
**Branch version:** `Assign3-1`</br>
**Branch version:** `Assign3-2`</br>


## Coordinate System
**Branch version:** `Assign3` series </br>
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

### Coordinate System – Factory Pattern (Before Using Object Pool)
**Branch version:** `Assign3`series </br>

Before implementing the **Object Pool**, the **Factory Pattern** was used in the *Coordinate System* to handle object creation.  
The mechanism worked in a similar way: when a specific gesture was recognized, an object was generated through the **Factory structure**.

The following code snippet from `GestureObjectFactory.cs` demonstrates how this mechanism worked:

```csharp
public void GestureRecognized(GestureType gestureType)
{
    Vector3 playerPos = player.transform.position;
    playerPos.y = 0;
    Vector3 spawnPos = spawnPoint;
    spawnPos.y = 0;
    float distance = Vector3.Distance(playerPos, spawnPos);

    if (GestureType.Scissors == gestureType && distance <= m_DistanceLimit)
    {
        Factory selectedFactory = m_factories[0];
        IProduct product = selectedFactory.GetProduct(spawnPoint);

        // Add the GameObject of the created product to the list
        if (product is Component component)
        {
            m_CreatedProducts.Add(component.gameObject);
        }
    }
}
```
**Branch version:** `Assign4` </br>
# Assignment - 4 Shader Graphs
**Branch version:** `Assign4`</br>

<img width="1636" height="954" alt="image" src="https://github.com/user-attachments/assets/3e7e4cbe-4524-4f3a-9101-d6d8fff9c8d5" /> </br>
<img width="1920" height="1003" alt="image" src="https://github.com/user-attachments/assets/de06efe3-cc69-4fd2-b4d4-a008407e7db9" /> </br>
**I created a water material using Shader Graph based on a video tutorial**

<img width="1144" height="823" alt="image" src="https://github.com/user-attachments/assets/b5e852a6-334b-4c24-a9ca-2efabdd166c4" /> </br>



**I recently added shader codes** </br>
I am creating a variety of shaders as part of my practice.
Using wave-based code, I made a water shader, and I included my code below.
I referenced the code from the tutorial video at the link below:
https://www.youtube.com/watch?v=kfM-yu0iQBk&t=13795s
```
Shader "Unlit/Wave3"
{
    Properties // input data
    {
        _ColorA ("ColorA", Color) = (1,1,1,1)
        _ColorB ("ColorB", Color) = (1,1,1,1)
        _ColorStart ("Color Start", Range(0,1)) = 1
        _ColorEnd ("Color End", Range(0,1)) = 1
        _WaveAmp ("Wave Amplitude", Range(0,0.4)) = 0.1
    }
    SubShader
    {
        // subshader tags
        Tags {
            "RenderType"="Opaque" //tag to inform the render pipeline of what type this is
           
        } 
        LOD 100

        Pass
        {
            //pass tags

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #define TAU 6.28318530718

            float4 _ColorA;
            float4 _ColorB;
            float _ColorStart;
            float _ColorEnd;
            float _WaveAmp; 

            //automatically filled out by Unity
            struct appdata // appdata = MeshData //per-vertext mesh data
            {
                float4 vertex : POSITION; // vertex position
                float3 normals : NORMAL; // local space normal direction
                //float4 tangent : TANGENT; // tangent direction (xyz) tangent sign (w) surface orientation information
                //float4 color : COLOR; // vertex colors
                float2 uv0 : TEXCOORD0; // uv0 diffuse/normal map textures
                //float4 uv1 : TEXCOORD1; // uv1 coordinates lightmap coordinates
                //float4 uv2 : TEXCODRD2; // uv2 coordinates lightmap coordinates
                //float4 uv3 : TEXCODRD3; // uv3 coordinates lightmap coordinates 
            };

            //data passed from the vertex shader to the fragment shader
            // this will interpolate/blend across the triangle!
            struct v2f //v2f = Interpolators
            {
                float4 vertex : SV_POSITION; // clip space position
                float3 normal: TEXCOORD0;
                float2 uv : TEXCOORD1;        
                
            };

            float GetWave(float2 uv){
                //float2 uvsCentered = uv*2-1;
                //float radialDistance = length(uvsCentered);
                float wave = cos( (uv*2-1- _Time.y *0.1) *TAU *5);
                return wave;
            }

            v2f vert (appdata v) //appdata = meshData
            {
                v2f o;

                //float wave = cos( (v.uv0.y - _Time.y *0.1) *TAU *5);
                //float wave2 = cos( (v.uv0.x - _Time.y *0.1) *TAU *5);
                //v.vertex.y = wave * wave2 *  _WaveAmp;
                v.vertex.y = GetWave(v.uv0) * _WaveAmp;
                o.vertex = UnityObjectToClipPos(v.vertex); //local space to clip space
                o.normal = UnityObjectToWorldNormal(v.normals); // just pass through
                o.uv = v.uv0; //(v.uv0 + _Offset )* _Scale; //passthrough
                return o;
            }

            float InverseLerp(float a, float b, float v){
                return (v-a)/(b-a);
            }

            float4 frag (v2f i) : SV_Target // color calculation per pixel
            {
                
                return lerp(_ColorA, _ColorB, GetWave(i.uv));
            }
            ENDCG
        }
    }
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
- Stylized Environnement - Free Pack </br>
 Source: https://assetstore.unity.com/packages/3d/environments/fantasy/stylized-environnement-free-pack-178090

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
- 3D Stylized Water with Refraction and Foam Shader Graph - Unity Tutorial
 Source: https://www.youtube.com/watch?v=MHdDUqJHJxM
### Here’s a useful reference video on how to set up hand tracking in Unity: </br>
 Source: https://www.youtube.com/watch?v=NV9WzAfRFz4
 
### Reference Shader Code - `shader` & `main` branch
- Shader Basics, Blending & Textures • Shaders for Game Devs [Part 1] & [Part 2]
 Source: https://www.youtube.com/@acegikmo
- Fire Shader
 Source: https://blog.febucci.com/2019/05/fire-shader/
