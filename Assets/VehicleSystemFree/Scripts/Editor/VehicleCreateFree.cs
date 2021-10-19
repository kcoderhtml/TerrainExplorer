using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class VehicleCreateFree : EditorWindow {

    public enum TractionType
    {
        front_wheel_drive,
        rear_wheel_drive,
        all_whell_drive,
        custom_whell_drive,
        none
    };

    public enum SpeedType
    {
        kmh,
        mph,
        none
    };

    public enum BreakType
    {
        front_wheel_drive,
        rear_wheel_drive,
        all_whell_drive,
        custom_whell_drive,
        none
    };

    [MenuItem("Window/Vehicle System Free/Vehicle Create")]
    public static void ShowWindow()
    {
        GetWindow<VehicleCreateFree>("Vehicle Create Free");
    }

    // The variables of the selection panel
    private bool vehicleSettingsPanel;
    private string labelString;
    private TractionType tractionType;
    private bool customTractionType;
    private int customTractionTypeAxleSize;
    private int[] customTractionTypeAxle = new int[0];
    private SpeedType speedType;
    private BreakType breakType;
    private bool customBreakType;
    private int customBreakTypeAxleSize;
    private int[] customBreakTypeAxle = new int[0];

    // To open and close the windows
    private bool createVehiclePanel;
    private bool vehicleEnginePanel;
    private bool vehicleBodyPanel;

    // Engine Variables
    private bool engineSettingsComplate;
    private string vehicleName;
    private int HP;
    private int numberOfWheel;
    AnimationCurve value = new AnimationCurve();
    private int breakPower;
    private int maxRpm;
    private int startEngineRpm;
    private bool curvePanel = false;
    private bool saveCurve = false;
    public bool editCurveButton = false;
    private int maxGear;
    private float[] gearRatios;
    private bool gearPanel;
    private bool gearSave;

    // Editor
    Texture m_createVehicleIcon;
    Texture m_engineSettingsIcon;
    Texture m_bodySettingsIcon;
    Texture m_sedanTemplateIcon;
    Texture m_JipTemplateIcon;
    Texture m_TruckTemplateIcon;
    Texture m_BusTemplateIcon;
    Texture m_CustomEngineSettingsIcon;

    private void OnEnable()
    {
        // Define when the editor opens
        tractionType = TractionType.none;
        speedType = SpeedType.none;
        breakType = BreakType.none;

        m_createVehicleIcon = (Texture)AssetDatabase.LoadMainAssetAtPath("Assets/VehicleSystemFree/Icon/CreateVehicleIcon.png");
        m_engineSettingsIcon = (Texture)AssetDatabase.LoadMainAssetAtPath("Assets/VehicleSystemFree/Icon/EngineSettingsIcon.png");
        m_bodySettingsIcon = (Texture)AssetDatabase.LoadMainAssetAtPath("Assets/VehicleSystemFree/Icon/BodySettingsIcon.png");
        m_sedanTemplateIcon = (Texture)AssetDatabase.LoadMainAssetAtPath("Assets/VehicleSystemFree/Icon/SedanTemplateIcon.png");
        m_JipTemplateIcon = (Texture)AssetDatabase.LoadMainAssetAtPath("Assets/VehicleSystemFree/Icon/JipTemplateIcon.png");
        m_TruckTemplateIcon = (Texture)AssetDatabase.LoadMainAssetAtPath("Assets/VehicleSystemFree/Icon/TruckTemplateIcon.png");
        m_BusTemplateIcon = (Texture)AssetDatabase.LoadMainAssetAtPath("Assets/VehicleSystemFree/Icon/BusTemplateIcon.png");
        m_CustomEngineSettingsIcon = (Texture)AssetDatabase.LoadMainAssetAtPath("Assets/VehicleSystemFree/Icon/CustomVehicleIcon.png");
    }

    private void OnGUI()
    {
        // Open the vehicle creation panel
        if (GUILayout.Button(new GUIContent(m_createVehicleIcon), GUILayout.MaxHeight(50f), GUILayout.ExpandWidth(true)))
        {
            createVehiclePanel = !createVehiclePanel;
        }

        if (createVehiclePanel)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("|\n|");
            GUILayout.FlexibleSpace();
            GUILayout.Label("|\n|");
            GUILayout.FlexibleSpace();
            GUILayout.Label("|\n|");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            createPanel();
        }
    }

    void createPanel ()
    {
        // Create vehicle
        if (engineSettingsComplate == false)
        {
            GUILayout.Button(new GUIContent(m_engineSettingsIcon), GUILayout.MaxHeight(50f), GUILayout.ExpandWidth(true));
            vehicleEnginePanel = true;
        }

        if (engineSettingsComplate)
        {
            GUILayout.Button(new GUIContent(m_engineSettingsIcon), GUILayout.MaxHeight(50f), GUILayout.ExpandWidth(true));

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("|\n|");
            GUILayout.FlexibleSpace();
            GUILayout.Label("|\n|");
            GUILayout.FlexibleSpace();
            GUILayout.Label("|\n|");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Button(new GUIContent(m_bodySettingsIcon), GUILayout.MaxHeight(50f), GUILayout.ExpandWidth(true));
            vehicleBodyPanel = true;
            vehicleEnginePanel = false;
        }

        if (vehicleEnginePanel)
        {
            showEngineSettings();
        }

        if (vehicleBodyPanel)
        {           
            if (engineSettingsComplate)
            {
                showBodySettings();
            }
        }
    }


    bool addVehicleLight = false;
    bool addVehicleUI = false;
    bool addVehicleCamera = false;
    bool addVehicleSound = false;
    bool addWheelEfect = false;
    bool addReadyParticle = false;
    bool addReadyWheelTrail = false;

    void showEngineSettings()
    {
        // Template selection
        GUILayout.Label("Select Vehicle Template", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();

        // Create custom vehicle settings
        if (GUILayout.Button(new GUIContent("Custom", m_CustomEngineSettingsIcon), GUILayout.MaxHeight(40f), GUILayout.ExpandWidth(true)))
        {
            vehicleSettingsPanel = true;
            labelString = "Custom ";
            vehicleName = "";
            numberOfWheel = 0;
            tractionType = TractionType.none;
            customTractionType = false;
            customTractionTypeAxleSize = 0;
            customTractionTypeAxle = new int[0];
            speedType = SpeedType.none;
            HP = 0;
            startEngineRpm = 0;
            maxRpm = 0;
            saveCurve = false;
            breakType = BreakType.none;
            breakPower = 0;
            maxGear = 0;
            gearRatios = new float[0];
            gearPanel = false;
        }
        // Create sedan vehicle settings
        if (GUILayout.Button(new GUIContent("Sedan",m_sedanTemplateIcon), GUILayout.MaxHeight(40f), GUILayout.ExpandWidth(true)))
        {
            vehicleSettingsPanel = true;
            labelString = "You can use ready templates only in the paid version. !!!";
        }

        // Create Truck settings
        if (GUILayout.Button(new GUIContent("Truck",m_TruckTemplateIcon), GUILayout.MaxHeight(40f), GUILayout.ExpandWidth(true)))
        {
            vehicleSettingsPanel = true;
            labelString = "You can use ready templates only in the paid version. !!!";
        }
      
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        // Create bus vehicle settings
        if (GUILayout.Button(new GUIContent("Bus",m_BusTemplateIcon), GUILayout.MaxHeight(40f), GUILayout.ExpandWidth(true)))
        {
            vehicleSettingsPanel = true;
            labelString = "You can use ready templates only in the paid version. !!!";
        }

        // Create Jip vehicle settings
        if (GUILayout.Button(new GUIContent("Jip", m_JipTemplateIcon), GUILayout.MaxHeight(40f), GUILayout.ExpandWidth(true)))
        {
            vehicleSettingsPanel = true;
            labelString = "You can use ready templates only in the paid version. !!!";
        }
        GUILayout.EndHorizontal();

        if (vehicleSettingsPanel)
        {
            GUILayout.Label(labelString + "Vehicle Settings", EditorStyles.boldLabel);


            // get a name entry
            GUILayout.BeginHorizontal();
            GUILayout.Label("Name :");
            vehicleName = EditorGUILayout.TextField(vehicleName);
            if (vehicleName == "" || vehicleName == null)
                EditorGUILayout.HelpBox("Please enter your name the vehicle", MessageType.Info);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            if (vehicleName == "" || vehicleName == null)
                return;

            // get a wheel entry 
            GUILayout.BeginHorizontal();
            GUILayout.Label("Number Of Wheel :");
            numberOfWheel = EditorGUILayout.IntField(numberOfWheel);
            if (numberOfWheel % 2 == 1 || numberOfWheel <= 2)
                EditorGUILayout.HelpBox("Please,enter a double number.Minimum 4", MessageType.Info);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            if (numberOfWheel % 2 == 1 || numberOfWheel <= 2)
                return;

            // get a Traction Type entry          
            if (tractionType == TractionType.none)
            {
                GUILayout.Label("Select Wheel Drive :");

                if (GUILayout.Button("Front Wheel Drive"))
                    tractionType = TractionType.front_wheel_drive; customTractionType = true;
                if (GUILayout.Button("Rear Wheel Drive"))
                    tractionType = TractionType.rear_wheel_drive; customTractionType = true;
                if (GUILayout.Button("All Wheel Drive"))
                    tractionType = TractionType.all_whell_drive; customTractionType = true;
                if (GUILayout.Button("Custom Wheel Drive"))
                    tractionType = TractionType.custom_whell_drive; customTractionType = false;

                EditorGUILayout.HelpBox("Please select a value", MessageType.Info);
                return;
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Select Wheel Drive :");
                switch (tractionType)
                {
                    case TractionType.front_wheel_drive:
                        GUILayout.Label("Selected value => Front wheel drive");
                        customTractionType = true;
                        break;
                    case TractionType.rear_wheel_drive:
                        GUILayout.Label("Selected value => Rear wheel drive");
                        customTractionType = true;
                        break;
                    case TractionType.all_whell_drive:
                        GUILayout.Label("Selected value => All wheel drive");
                        customTractionType = true;
                        break;
                    case TractionType.custom_whell_drive:
                        GUILayout.Label("Selected value => Custom wheel drive");                       
                        break;
                }

                if (GUILayout.Button("Edit"))
                    tractionType = TractionType.none;
                GUILayout.EndHorizontal();
            }

            // Selected Custom Wheel
            if(tractionType == TractionType.custom_whell_drive && customTractionType == false)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Select Axle :");
                customTractionTypeAxleSize = EditorGUILayout.IntField(customTractionTypeAxleSize);
                GUILayout.Label("How many axles should be active ?");
                if (GUILayout.Button("Apply"))
                    customTractionTypeAxle = new int[customTractionTypeAxleSize];
                GUILayout.EndHorizontal();

                if (customTractionTypeAxle.Length != 0)
                    if (customTractionTypeAxle.Length == customTractionTypeAxleSize)
                    {
                        GUILayout.BeginHorizontal();
                        for (int i = 0; i < customTractionTypeAxleSize; i++)
                            customTractionTypeAxle[i] = EditorGUILayout.IntField(customTractionTypeAxle[i]);
                        GUILayout.EndHorizontal();
                        GUILayout.Label("Select active axle(Examle: 3 => Left Wheel 2 and Right Wheel 2)");

                        if (GUILayout.Button("Apply"))
                            customTractionType = true;
                    }
               
            }

            if (!customTractionType)
                return;

            // Speed Type
            if (speedType == SpeedType.none)
            {
                GUILayout.Label("Select Speed Type :");

                if (GUILayout.Button("MPH"))
                    speedType = SpeedType.mph;
                if (GUILayout.Button("KMH"))
                    speedType = SpeedType.kmh;

                EditorGUILayout.HelpBox("Please select a value", MessageType.Info);
                return;
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Select Speed Type :");
                switch (speedType)
                {
                    case SpeedType.mph:
                        GUILayout.Label("Selected value => MPH");
                        break;
                    case SpeedType.kmh:
                        GUILayout.Label("Selected value => KMH");
                        break;
                }

                if (GUILayout.Button("Edit"))
                    speedType = SpeedType.none;
                GUILayout.EndHorizontal();
            }

            // HP
            GUILayout.BeginHorizontal();
            GUILayout.Label("Vehicle HP :");
            HP = EditorGUILayout.IntField(HP);
            GUILayout.Label("Standard value = 175");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            if (HP <= 0)
                return;



            // Start Engine Rpm
            GUILayout.BeginHorizontal();
            GUILayout.Label("Start Engine Rpm :");
            startEngineRpm = EditorGUILayout.IntField(startEngineRpm);
            GUILayout.EndHorizontal();

            if (startEngineRpm < 100 || startEngineRpm > 900)
            {
                EditorGUILayout.HelpBox("Idling speeds of the vehicle (Between 100 and 900)", MessageType.Info);
                return;
            }

            // Max Rpm
            GUILayout.BeginHorizontal();
            GUILayout.Label("Vehicle Max Rpm :");
            maxRpm = EditorGUILayout.IntField(maxRpm);
            if (maxRpm >= 2500)
            {
                if (GUILayout.Button("Okey"))
                {
                    editCurveButton = true;
                    int rate = 0;
                    rate = maxRpm / 1000;
                    int mod = 0;
                    mod = maxRpm % 1000;


                    for (int i = 0; i < value.keys.Length; i++)
                        value.RemoveKey(i);

                    if (mod == 500)
                    {
                        for (int i = 0; i < rate + 2; i++)
                            value.AddKey(i * 1000, 150);
                    }

                    if (mod == 0)
                    {
                        for (int i = 0; i < rate + 1; i++)
                            value.AddKey(i * 1000, 150);
                    }
                    curvePanel = false;
                }
            }
            GUILayout.EndHorizontal();
            if (maxRpm % 500 != 0 || maxRpm < 2500)
            {
                EditorGUILayout.HelpBox("in multiples of 500 (min 2500)", MessageType.Warning);
                return;
            }


            // Motor torq graphic
            if (editCurveButton)
                if (GUILayout.Button("Edit Curve"))
                    curvePanel = !curvePanel;

            if (curvePanel)
            {
                saveCurve = false;
                GUILayout.Label("Enter the torque produced at every 1000 rpm (Start Engine Rpm / ... / Max Rpm)");
                int mod = maxRpm % 1000;

                value.MoveKey(0, new Keyframe(startEngineRpm, 0));
                if (mod == 500)
                {
                    for (int i = 1; i < value.keys.Length - 1; i++)
                        value.MoveKey(i, new Keyframe(i * 1000, value.keys[i].value));
                    value.MoveKey(value.keys.Length - 1, new Keyframe(maxRpm, value.keys[value.keys.Length - 1].value));
                }

                if (mod == 0)
                {
                    for (int i = 1; i < value.keys.Length; i++)
                        value.MoveKey(i, new Keyframe(i * 1000, value.keys[i].value));
                }

                EditorGUILayout.CurveField(value);

                if (GUILayout.Button("Save Curve"))
                {
                    editCurveButton = false;
                    curvePanel = false;
                    saveCurve = true;
                }
            }

            if (!saveCurve || editCurveButton)
                return;

            // Break Type          
            if (breakType == BreakType.none)
            {
                GUILayout.Label("Select Break Type :");

                if (GUILayout.Button("Front"))
                    breakType = BreakType.front_wheel_drive; customBreakType = true;
                if (GUILayout.Button("Rear"))
                    breakType = BreakType.rear_wheel_drive; customBreakType = true;
                if (GUILayout.Button("All"))
                    breakType = BreakType.all_whell_drive; customBreakType = true;
                if (GUILayout.Button("Custom"))
                    breakType = BreakType.custom_whell_drive; customBreakType = false;

                EditorGUILayout.HelpBox("Please select a value", MessageType.Info);
                return;
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Select Break Type :");
                switch (breakType)
                {
                    case BreakType.front_wheel_drive:
                        GUILayout.Label("Selected value => Front");
                        customBreakType = true;
                        break;
                    case BreakType.rear_wheel_drive:
                        GUILayout.Label("Selected value => Rear");
                        customBreakType = true;
                        break;
                    case BreakType.all_whell_drive:
                        GUILayout.Label("Selected value => All");
                        customBreakType = true;
                        break;
                    case BreakType.custom_whell_drive:
                        GUILayout.Label("Selected value => Custom");
                        break;
                }

                if (GUILayout.Button("Edit"))
                    breakType = BreakType.none;

                GUILayout.Label("Break Power");
                breakPower = EditorGUILayout.IntField(breakPower);
                GUILayout.EndHorizontal();
            }
            if (breakPower <= 0)
            {
                EditorGUILayout.HelpBox("Please enter a positive number of brake power.Eg:500", MessageType.Info);
                return;
            }

            if (breakType == BreakType.custom_whell_drive && customBreakType == false)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Select Axle :");
                customBreakTypeAxleSize = EditorGUILayout.IntField(customBreakTypeAxleSize);
                GUILayout.Label("How many axles should be active ?");
                if (GUILayout.Button("Apply"))
                    customBreakTypeAxle = new int[customBreakTypeAxleSize];
                GUILayout.EndHorizontal();

                if (customBreakTypeAxle.Length != 0)
                    if (customBreakTypeAxle.Length == customBreakTypeAxleSize)
                    {
                        GUILayout.BeginHorizontal();
                        for (int i = 0; i < customBreakTypeAxleSize; i++)
                            customBreakTypeAxle[i] = EditorGUILayout.IntField(customBreakTypeAxle[i]);
                        GUILayout.EndHorizontal();
                        GUILayout.Label("Select active axle(Examle: 3 => Left Wheel 2 and Right Wheel 2)");

                        if (GUILayout.Button("Apply"))
                            customBreakType = true;
                    }

            }

            if (!customBreakType)
                return;

            // Gear
            GUILayout.BeginHorizontal();
            GUILayout.Label("Total number of gear :");
            maxGear = EditorGUILayout.IntField(maxGear);
            if (maxGear <= 0)
                EditorGUILayout.HelpBox("Please enter a positive number", MessageType.Info);
            else if (GUILayout.Button("Edit"))
            {
                gearRatios = new float[maxGear+1];
                gearPanel = true;
            }
            GUILayout.EndHorizontal();

            if (gearPanel)
            {
                GUILayout.Label("gear 0: Differential gear ratio / gear 1...: Gear ratios");
                GUILayout.BeginHorizontal();
                string labelStr = "";
                for (int i = 0; i < gearRatios.Length; i++)
                {
                    labelStr = i + " Gear:";
                    if (i == 0)
                        labelStr ="Differential gear";
                    if (i == gearRatios.Length - 1)
                    {
                        labelStr = "Reverse gear";
                        gearRatios[i] = gearRatios[1];
                    }
                    GUILayout.Label(labelStr);
                    gearRatios[i] = EditorGUILayout.FloatField(gearRatios[i]);
                }
                GUILayout.EndHorizontal();

                if (GUILayout.Button("Save"))
                {
                    gearSave = true;
                    gearPanel = false;
                }
            }

            if (!gearSave)
                return;

            // Features to add
            GUILayout.Label("Add property",EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();          
            GUILayout.Label("Do you want to add vehicle LIGHTS ?");
            addVehicleLight =  EditorGUILayout.Toggle(addVehicleLight);
            if (addVehicleLight)
                GUILayout.Label(" <<< You can use this feature only in the paid version",EditorStyles.boldLabel);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Do you want to add a UI in the vehicle ?");
            addVehicleUI =     EditorGUILayout.Toggle(addVehicleUI);
            if (addVehicleUI)
                GUILayout.Label(" <<< You can use this feature only in the paid version", EditorStyles.boldLabel);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Do you want to add a CAMERA in the vehicle ?");
            addVehicleCamera = EditorGUILayout.Toggle(addVehicleCamera);
            if (addVehicleCamera)
                GUILayout.Label(" <<< You can use this feature only in the paid version", EditorStyles.boldLabel);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Do you want to add a WHEEL SKID SOUND in the vehicle ?");
            addWheelEfect = EditorGUILayout.Toggle(addWheelEfect);
            if (addWheelEfect)
                GUILayout.Label(" <<< You can use this feature only in the paid version", EditorStyles.boldLabel);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Do you want to add AUDIO to your vehicle ?");
            addVehicleSound =  EditorGUILayout.Toggle(addVehicleSound);
            if (addVehicleSound)
                GUILayout.Label(" <<< You can use this feature only in the paid version", EditorStyles.boldLabel);
            GUILayout.EndHorizontal();

            if (addVehicleSound)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Do you want to add your vehicle PARTICLE ?");
                addReadyParticle = EditorGUILayout.Toggle(addReadyParticle);
                if (addReadyParticle)
                    GUILayout.Label(" <<< You can use this feature only in the paid version", EditorStyles.boldLabel);
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label("Do you want to add a WHEEL TRAIL in the vehicle ?");
            addReadyWheelTrail = EditorGUILayout.Toggle(addReadyWheelTrail);
            if (addReadyWheelTrail)
                GUILayout.Label(" <<< You can use this feature only in the paid version", EditorStyles.boldLabel);
            GUILayout.EndHorizontal();

            if (engineSettingsComplate)
            {
                vehicleEnginePanel = false;
                GUILayout.Label("Lütfen body ", EditorStyles.boldLabel);
                return;
            }
            else
            {
                if (GUILayout.Button("NEXT STEP => Body"))
                {
                    // Equalization of variables
                    equalize();
                    vehicleBodyPanel = true;
                    vehicleEnginePanel = false;
                    engineSettingsComplate = true;
                }
            }
        }
    }

    GameObject gm;
    VehicleFree vehicle;
    void equalize ()
    {
        //  This section installs the features specified in your vehicle

        gm = new GameObject();
        gm.AddComponent<Rigidbody>();
        gm.AddComponent<VehicleFree>();

        gm.GetComponent<Rigidbody>().mass = 1000;
        gm.GetComponent<Rigidbody>().drag = 0.015f;        
       
        vehicle = gm.GetComponent<VehicleFree>();

        gm.name = vehicleName;

        vehicle.m_controlType = VehicleFree.ControlType.semi_automatic;

        switch (tractionType)
        {
            case TractionType.all_whell_drive:
                vehicle.m_tractionType = VehicleFree.TractionType.all_whell_drive;
                break;
            case TractionType.front_wheel_drive:
                vehicle.m_tractionType = VehicleFree.TractionType.front_wheel_drive;
                break;
            case TractionType.rear_wheel_drive:
                vehicle.m_tractionType = VehicleFree.TractionType.rear_wheel_drive;
                break;
            case TractionType.custom_whell_drive:
                vehicle.m_tractionType = VehicleFree.TractionType.custom_whell_drive;
                vehicle.customWheelAxle = customTractionTypeAxle;
                break;
        }

        switch (speedType)
        {
            case SpeedType.kmh:
                vehicle.m_speedType = VehicleFree.SpeedType.kmh;
                break;
            case SpeedType.mph:
                vehicle.m_speedType = VehicleFree.SpeedType.mph;
                break;
        }

        vehicle.HP = HP;

        vehicle.motorTorqGraphic = value;

        switch (breakType)
        {
            case BreakType.all_whell_drive:
                vehicle.m_breakType = VehicleFree.BreakType.all_whell_drive;
                break;
            case BreakType.front_wheel_drive:
                vehicle.m_breakType = VehicleFree.BreakType.front_wheel_drive;
                break;
            case BreakType.rear_wheel_drive:
                vehicle.m_breakType = VehicleFree.BreakType.rear_wheel_drive;
                break;
            case BreakType.custom_whell_drive:
                vehicle.m_breakType = VehicleFree.BreakType.custom_whell_drive;
                vehicle.customWheelAxleBreak = customBreakTypeAxle;
                break;
        }

        vehicle.breakPower = breakPower;

        vehicle.gearRatios = gearRatios;

        vehicle.maxRpm = maxRpm;
        vehicle.startEngineRpm = startEngineRpm;

        //Center of gravty object
        GameObject centerOfGravityObj = new GameObject();
        centerOfGravityObj.name = "CenterOfGravity";
        centerOfGravityObj.transform.parent = gm.transform;

        GameObject centerOfGravityObj_Front = new GameObject();
        centerOfGravityObj_Front.name = "Front";
        centerOfGravityObj_Front.transform.parent = centerOfGravityObj.transform;
        centerOfGravityObj_Front.transform.localPosition = new Vector3(0, -0.2f, 1);
        centerOfGravityObj_Front.AddComponent<Rigidbody>().mass = 285;
        FixedJoint cogFFJ = centerOfGravityObj_Front.AddComponent<FixedJoint>();
        cogFFJ.connectedBody = gm.GetComponent<Rigidbody>();

        GameObject centerOfGravityObj_Back = new GameObject();
        centerOfGravityObj_Back.name = "Back";
        centerOfGravityObj_Back.transform.parent = centerOfGravityObj.transform;
        centerOfGravityObj_Back.transform.localPosition = new Vector3(0, -0.2f, -1);
        centerOfGravityObj_Back.AddComponent<Rigidbody>().mass = 185;
        FixedJoint cogBFJ = centerOfGravityObj_Back.AddComponent<FixedJoint>();
        cogBFJ.connectedBody = gm.GetComponent<Rigidbody>();

        //Body object
        GameObject body = new GameObject();
        body.name = "Body";
        body.transform.parent = gm.transform;

        //Collider object
        GameObject Collider = new GameObject();
        Collider.name = "Collider";
        Collider.transform.parent = gm.transform;
        Collider.AddComponent<BoxCollider>();      

        //Wheel collider object
        GameObject wheelCol = new GameObject();
        wheelCol.name = "WheelCollider";
        wheelCol.transform.parent = gm.transform;

        WheelFrictionCurve wfcForward = new WheelFrictionCurve();
        wfcForward.extremumSlip = 1f;
        wfcForward.extremumValue = 2f;
        wfcForward.asymptoteSlip = 2f;
        wfcForward.asymptoteValue = 1f;
        wfcForward.stiffness = 1f;
        WheelFrictionCurve wfcSideway = new WheelFrictionCurve();
        wfcSideway.extremumSlip = 0.5f;
        wfcSideway.extremumValue = 1f;
        wfcSideway.asymptoteSlip = 1f;
        wfcSideway.asymptoteValue = 0.5f;
        wfcSideway.stiffness = 1f;

        for (int i = 0; i < numberOfWheel; i++)
        {
            GameObject wheelC = new GameObject();
            if (i >= numberOfWheel / 2)
                wheelC.name = "WheelColliderR" + (i - numberOfWheel/2);
            else
                wheelC.name = "WheelColliderL" + i;
            wheelC.transform.parent = wheelCol.transform;
            WheelCollider wcol = wheelC.AddComponent<WheelCollider>();           
            wcol.mass = 20;
            wcol.forwardFriction = wfcForward;
            wcol.sidewaysFriction = wfcSideway;
        }

        //Wheel mesh object
        GameObject wheelMesh = new GameObject();
        wheelMesh.name = "WheelMesh";
        wheelMesh.transform.parent = gm.transform;
    }

    GameObject bodyPrefab;
    GameObject rightWheelPrefab;
    GameObject[] rightWheelObj;
    GameObject leftWheelPrefab;
    GameObject[] leftWheelObj;

    bool selectedBodyPrefab;
    bool selectedRightWheelPrefab;
    bool selectedLeftWheelPrefab;

    bool wheelTransformPanel;
    GameObject[] selectedWheelObj;
    Vector3 plusWheelPos;

    void showBodySettings ()
    {      
        if (wheelTransformPanel == false)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Select Body Prefab       ", EditorStyles.boldLabel);
            GUILayout.Label("Select Left Wheel Prefab ", EditorStyles.boldLabel);
            GUILayout.Label("Select Right Wheel Prefab", EditorStyles.boldLabel);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            // Body prefab   
            bodyPrefab = EditorGUILayout.ObjectField(bodyPrefab, typeof(GameObject), true) as GameObject;
            if (bodyPrefab != null)
            {
                if (selectedBodyPrefab == false)
                    if (GUILayout.Button("Okey"))
                    {
                        bodyPrefab = Instantiate(bodyPrefab, gm.transform.Find("Body").transform) as GameObject;
                        bodyPrefab.transform.localPosition = Vector3.zero;
                        selectedBodyPrefab = true;
                    }

                if (GUILayout.Button("Edit"))
                {
                    if (selectedBodyPrefab && bodyPrefab != null)
                        DestroyImmediate(bodyPrefab);
                    selectedBodyPrefab = false;
                    bodyPrefab = null;
                }
            }

            // Left wheel
            leftWheelPrefab = EditorGUILayout.ObjectField(leftWheelPrefab, typeof(GameObject), true) as GameObject;
            if (leftWheelPrefab != null)
            {
                if (selectedLeftWheelPrefab == false)
                    if (GUILayout.Button("Okey"))
                    {
                        leftWheelObj = new GameObject[numberOfWheel / 2];
                        for (int i = 0; i < numberOfWheel / 2; i++)
                        {
                            leftWheelPrefab = Instantiate(leftWheelPrefab, gm.transform.Find("WheelMesh").transform) as GameObject;
                            leftWheelObj[i] = leftWheelPrefab;
                            leftWheelObj[i].name = "WheelMeshL" + i;
                        }
                        selectedLeftWheelPrefab = true;
                    }

                if (GUILayout.Button("Edit"))
                {
                    if (selectedLeftWheelPrefab && leftWheelPrefab != null)
                    {
                        DestroyImmediate(leftWheelPrefab);
                        for (int i = 0; i < leftWheelObj.Length; i++)
                            DestroyImmediate(leftWheelObj[i]);
                    }
                    selectedLeftWheelPrefab = false;
                    leftWheelPrefab = null;
                }
            }

            // Right wheel
            rightWheelPrefab = EditorGUILayout.ObjectField(rightWheelPrefab, typeof(GameObject), true) as GameObject;
            if (rightWheelPrefab != null)
            {
                if (selectedRightWheelPrefab == false)
                    if (GUILayout.Button("Okey"))
                    {
                        rightWheelObj = new GameObject[numberOfWheel / 2];
                        for (int i = 0; i < numberOfWheel / 2; i++)
                        {
                            rightWheelPrefab = Instantiate(rightWheelPrefab, gm.transform.Find("WheelMesh").transform) as GameObject;
                            rightWheelObj[i] = rightWheelPrefab;
                            rightWheelObj[i].name = "WheelMeshR" + i;
                        }
                        selectedRightWheelPrefab = true;
                    }

                if (GUILayout.Button("Edit"))
                {
                    if (selectedRightWheelPrefab && rightWheelPrefab != null)
                    {
                        DestroyImmediate(rightWheelPrefab);
                        for (int i = 0; i < rightWheelObj.Length; i++)
                            DestroyImmediate(rightWheelObj[i]);
                    }
                    selectedRightWheelPrefab = false;
                    rightWheelPrefab = null;
                }
            }
            GUILayout.EndHorizontal();

            if (bodyPrefab != null && selectedBodyPrefab && rightWheelPrefab != null && selectedRightWheelPrefab && leftWheelPrefab != null && selectedLeftWheelPrefab)
            {
                if (GUILayout.Button("Select And Calculate Transform"))
                {
                    wheelTransformPanel = true;
                    // Estimated wheel position calculation
                    for (int i = 0; i < leftWheelObj.Length; i++)
                        leftWheelObj[leftWheelObj.Length - i -1].transform.localPosition = new Vector3(-1, -1, i);
                    for (int i = 0; i < rightWheelObj.Length; i++)
                        rightWheelObj[rightWheelObj.Length - i -1].transform.localPosition = new Vector3(1, -1, i);
                }
            }
        }

        if (bodyPrefab == null || rightWheelPrefab == null || leftWheelPrefab == null)
        {
            EditorGUILayout.HelpBox("Please select the objects => body,right wheel,left wheel", MessageType.Info);
            EditorGUILayout.HelpBox("Your choices should be in the form of prefab", MessageType.Warning);
            // Wheel transform kodu içerisinden çkeilde
            selectedWheelObj = new GameObject[numberOfWheel];
        }

        

        if (wheelTransformPanel == false)
            return;

        if (wheelTransformPanel)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Select Wheel :", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            for (int i = 0; i < leftWheelObj.Length; i++)
            {
                if (GUILayout.Button(leftWheelObj[i].name))
                {
                    if (selectedWheelObj[i] != leftWheelObj[i])
                        selectedWheelObj[i] = leftWheelObj[i];
                    else
                        selectedWheelObj[i] = null;
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            for (int i = 0; i < rightWheelObj.Length; i++)
            {
                if (GUILayout.Button(rightWheelObj[i].name))
                {
                    if (selectedWheelObj[numberOfWheel/2 + i] != rightWheelObj[i])
                        selectedWheelObj[numberOfWheel/2 + i] = rightWheelObj[i];
                    else
                        selectedWheelObj[numberOfWheel/2 + i] = null;
                }
            }
            GUILayout.EndHorizontal();

            Tools.current = Tool.Move;

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Tools", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Remove all"))
            {
                selectedWheelObj = new GameObject[numberOfWheel];
                Selection.activeGameObject = null;
            }
            if (GUILayout.Button("Select all"))
            {
                selectedWheelObj = new GameObject[numberOfWheel];
                for (int i = 0; i < leftWheelObj.Length; i++)
                    selectedWheelObj[i] = leftWheelObj[i];
                for (int i = 0; i < rightWheelObj.Length; i++)
                    selectedWheelObj[numberOfWheel/2 + i] = rightWheelObj[i];
            }
            GUILayout.EndHorizontal();
            Selection.objects = selectedWheelObj;
        }


        if (GUILayout.Button("Apply"))
        {
            VehicleFree gmVehicle = gm.GetComponent<VehicleFree>();
            gmVehicle.whellCols = new WheelCollider[numberOfWheel];
            gmVehicle.whellMeshs = new Transform[numberOfWheel];

            for (int i = 0; i < numberOfWheel; i++)
            {
                gm.transform.Find("WheelCollider").GetChild(i).localPosition = gm.transform.Find("WheelMesh").GetChild(i).localPosition + new Vector3(0, 0.15f, 0);
            }

            // Adjust the left wheel
            for (int i = 0; i < numberOfWheel / 2; i++)
            {
                gmVehicle.whellCols[i * 2] = gm.transform.Find("WheelCollider").GetChild(i).GetComponent<WheelCollider>();

                gmVehicle.whellMeshs[i * 2] = gm.transform.Find("WheelMesh").GetChild(i).GetComponent<Transform>();
            }

            // Adjust the right wheel
            for (int i = numberOfWheel / 2; i < numberOfWheel; i++)
            {
                gmVehicle.whellCols[(i - numberOfWheel / 2) * 2 + 1] = gm.transform.Find("WheelCollider").GetChild(i).GetComponent<WheelCollider>();

                gmVehicle.whellMeshs[(i - numberOfWheel / 2) * 2 + 1] = gm.transform.Find("WheelMesh").GetChild(i).GetComponent<Transform>();
            }

            this.Close();
        }
        
    }
}
