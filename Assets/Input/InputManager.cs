using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

[DefaultExecutionOrder(-100)]
public class InputManager : MonoBehaviour
{
    //no longer gloabal singleton
    //every time CreateActionForDevice is called, a new instance of applications for each controls. 
    public static ApplicationActions CreateActionForDevice (string controlSchemeName)
    {
        ApplicationActions Actions = new ApplicationActions();

        //find the scheme from its name in input actions
        //firstordefualt returns the default struct value if no match

        var scheme = Actions.controlSchemes.FirstOrDefault(s => s.name == controlSchemeName);

        if(!string.IsNullOrEmpty(scheme.name))
        {
           var matchedDevice = new List<InputDevice>();

            foreach( var requirement in scheme.deviceRequirements)
            {
                //requirement.controlpath is the string name of the control scheme
                //Inputsystem.FindControl resolves this against cnnected devices

                var device = InputSystem.devices.FirstOrDefault(d =>
                    InputControlPath.Matches(requirement.controlPath, d));

                if (device != null && !matchedDevice.Contains(device))
                {
                    matchedDevice.Add(device);
                }
            }

            Actions.devices = new ReadOnlyArray<InputDevice>(matchedDevice.ToArray());

            Actions.bindingMask = InputBinding.MaskByGroup(controlSchemeName);

            Actions.Enable();
            return Actions;
        }
        return null;
    }


   
}
