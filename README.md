# Hololens Hue Lights
Control the Philips Hue Lights with the Hololens

# Get started

This version include a **Hue Bridge** and **Lamp** prefab.
With does 2 prefab you should be able to get going pretty fasts.

The project is set to already have all the elements from the Holotoolkit to interact with the lamp.

To get started click in your Hierachy on the **Hue Bridge** set the ip address the bridge and a Philips Hue developer username if you have one.

By default right now the lamp is the second lamp in order of the lamps you have. To change the lamp id (Device Path) open the script: `SetLightColorAction` and in the OnSelect() method:

```csharp
void OnSelect ()
   {
       HueLamp[] lamps = hueBridge.GetComponentsInChildren<HueLamp>();
       foreach (HueLamp lamp in lamps)
       {
           //Debug.Log("lamp before: " + lamp.devicePath + " : " + lamp.on);

           // Get the second lamp using device path.
           if (lamp.devicePath == "2")
           {
                   lamp.on = true;
                   lamp.color = color;
           }

           //Debug.Log("lamp after: " + lamp.devicePath + " : " + lamp.on);

       }

   }
```
change the `lamp.devicePath == "2"` to the lamp you are trying to control.

# roadmap

This is just a base to get started. We used it as a POC in demos.
Next we are working on a new User Interface to
1. Set the color and brightness of each lights.
2. General panel to set the color and brightness or themes on all the lights.
3. Setup mode to position the lamp in your room and save it.
