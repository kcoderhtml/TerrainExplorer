using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class manual
{
    public List<KeyCode> gearKeys = new List<KeyCode>();
}

public class VehicleFree : MonoBehaviour {

    public enum ControlType
    {
        automatic,
        semi_automatic,
        manual,
        simple
    };

    public enum TractionType
    {
        front_wheel_drive,
        rear_wheel_drive,
        all_whell_drive,
        custom_whell_drive
    };

    public enum BreakType
    {
        front_wheel_drive,
        rear_wheel_drive,
        all_whell_drive,
        custom_whell_drive
    };

    [System.Serializable]
    public enum SpeedType
    {
        kmh,
        mph
    };

    // Pedals
    [Header ("Pedals and variable")]
    [Range (0,6500)]public float gasPedal;

    // Types
    [Header("Vehicle Settings")]
    [SerializeField] public ControlType m_controlType;
    [SerializeField] public TractionType m_tractionType;
    [SerializeField] public SpeedType m_speedType;
    public int[] customWheelAxle;

    // Torq Variable
    [Header("Torq variable")]
    public float HP;
    [Tooltip("Rpm / Torq")]public AnimationCurve motorTorqGraphic;

    // Break Variable
    [Header("Break Settings")]
    [SerializeField] public BreakType m_breakType;
    public float breakPower;
    public int[] customWheelAxleBreak;

    // Gear variable
    [Header ("Gear Settings")]
    public int gear;
    public float[] gearRatios;
    public int reversingGear;

    // Rpm variable
    [Header("Rpm Variable")]
    public float maxRpm;
    public float currentRpm;
    public float fixedRpm;
    public float startEngineRpm;

    // Values to be assigned
    [Header("Assigned")]
    public WheelCollider[] whellCols;
    public Transform[] whellMeshs;

    // Control type
    [Header("Controller")]
    public int nextGearRpm;
    public int previousGearRpm;
    [SerializeField] public manual m_manual;

    // Private
    WheelHit whellHit;
    Rigidbody rb;

    private void Start()
    {
        whellHit = new WheelHit(); 

        rb = GetComponent<Rigidbody>();
        selectWheelHit();
    }

    void FixedUpdate()
    {
        // Automatic or manual gear control
        GearControl();

        // Rpm calculation
        calculateRpm(startEngineRpm);

        // the process of wheel rotation     
        whellSteer(30f * Input.GetAxis("Horizontal"));
        //Mesh tracking
        meshMove();

        // Depending on the speed and calculate wheel rpm
        fixedRpm = ((currentSpeed() * 518) / 60) * (gearRatios[0] * gearRatios[gear]);
    }

    // Automatic or manual gear control
    void GearControl ()
    {
        switch (m_controlType)
        {
            case ControlType.simple:               
                simpleControl();
                break;
            case ControlType.automatic:
                automaticControl();
                break;
            case ControlType.semi_automatic:
                semiAutomaticControl();
                break;
            case ControlType.manual:
                manualControl();
                break;
        }
    }

    void simpleControl()
    {

        if (fixedRpm <= startEngineRpm)
        {
            gear = 0;
            if (Input.GetAxisRaw("BreakPedal") == 1)
            {
                gear = gearRatios.Length - 1;
                reversingGear = -1;
            }
        }

        if (gear > 0)
        {
            if (fixedRpm > nextGearRpm)
                if (gear < gearRatios.Length - 2)
                {
                    gear++;
                    reversingGear = 1;
                }
            if (fixedRpm < previousGearRpm)
                if (gear > 1 && gear != gearRatios.Length - 1)
                {
                    gear--;
                    reversingGear = 1;
                }
        }
        else
        {
            if (currentRpm > startEngineRpm + 100)
                gear = 1;
            reversingGear = 1;
        }

        if (fixedRpm < maxRpm - 500)
        {
            if (gear == gearRatios.Length - 1)
            {
                gasPedal = Input.GetAxis("BreakPedal") * maxRpm;
                addBreakTorq(Input.GetAxisRaw("GasPedal") * breakPower);
                addTorq(calculateCurrentTorq(gear, gasPedal) * reversingGear);
            }
            else
            {
                gasPedal = Input.GetAxis("GasPedal") * maxRpm;
                addBreakTorq(Input.GetAxisRaw("BreakPedal") * breakPower);
                addTorq(calculateCurrentTorq(gear, gasPedal) * reversingGear);
            }
        }
        else
        {
            addTorq(0);
            addBreakTorq(-1);
        }
    }
    void automaticControl ()
    {
        gasPedal = Input.GetAxis("GasPedal") * maxRpm;
        addBreakTorq(Input.GetAxisRaw("BreakPedal") * breakPower);

        if (fixedRpm < startEngineRpm - 100 && gear != gearRatios.Length - 1)
            gear = 0;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            gear = 0;
            reversingGear = 1;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) && gear < 2)
        {
            gear = gearRatios.Length - 1;
            reversingGear = -1;
        }

        if (gear > 0)
        {
            if (fixedRpm > nextGearRpm)
                if (gear < gearRatios.Length - 2)
                {
                    gear++;
                    reversingGear = 1;
                }
            if (fixedRpm < previousGearRpm)
                if (gear > 1 && gear != gearRatios.Length - 1)
                {
                    gear--;
                    reversingGear = 1;
                }
        }
        else
        {
            if (currentRpm > startEngineRpm + 100)
                gear = 1;
            reversingGear = 1;
        }

        
        if (fixedRpm < maxRpm - 500) // -500 olma sebebi kesiciye girmesi
            addTorq(calculateCurrentTorq(gear, gasPedal) * reversingGear);
        else
            addTorq(-calculateCurrentTorq(gear, fixedRpm) * reversingGear);
    }
    void semiAutomaticControl ()
    {
        gasPedal = Input.GetAxis("GasPedal") * maxRpm;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (gear < gearRatios.Length - 2)
            {
                gear++;
                reversingGear = 1;
            }

            if (gear == gearRatios.Length - 1)
            {
                gear = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (gear != gearRatios.Length - 1)
            {
                gear--;
            }

            if (gear <= 0)
            {
                gear = gearRatios.Length - 1;
                reversingGear = -1;
            }
        }

        if (fixedRpm < maxRpm - 500)
        {
            addTorq(calculateCurrentTorq(gear, gasPedal) * reversingGear);
            addBreakTorq(Input.GetAxis("BreakPedal") * breakPower);
        }
        else
        {
            addTorq(0);
            addBreakTorq(-1);
        }
    }
    void manualControl ()
    {       
        gasPedal = Input.GetAxis("GasPedal") * maxRpm;

        for (int i = 0; i < m_manual.gearKeys.Capacity; i++)
            if (Input.GetKeyDown(m_manual.gearKeys[i]))
            {
                gear = i + 1;
                if (gear == gearRatios.Length - 1)
                    reversingGear = -1;
                else
                    reversingGear = 1;
            }

        if (fixedRpm < maxRpm - 500)
        {
            addTorq(calculateCurrentTorq(gear, gasPedal) * reversingGear);
            addBreakTorq(Input.GetAxisRaw("BreakPedal") * breakPower);
        }
        else
        {
            addTorq(0);
            addBreakTorq(-1);
        }
    }

    // speed calculation 
    public float currentSpeed ()
    {
        float speed = rb.velocity.magnitude;
        if (m_speedType == SpeedType.kmh)
            speed = rb.velocity.magnitude * 3.6f; //is multiplied by a 3.6 kmh
        if (m_speedType == SpeedType.mph)
            speed = rb.velocity.magnitude * 2.2f; //is multiplied by a 3.6 mph

        return speed;
    }

    // UI operations
    private float referance;
    public float rpmUI ()
    {
        float rpmUI = 1;
        selectWheelHit();

        if (gear != 0)
        {
            if (whellHit.forwardSlip <= 0.8f) // if the wheel is not slipping
            {
                Mathf.SmoothDamp(rpmUI, fixedRpm, ref referance, 1f);
                referance++;
                rpmUI = referance;
            }
            else // if the wheel is slipping
            {
                Mathf.SmoothDamp(rpmUI, currentRpm, ref referance, 1f);
                referance++;
                rpmUI = referance;
            }
        }
        else
        {
            rpmUI = currentRpm;
        }

        return rpmUI/maxRpm;
    }

    // Given the power to the wheels, selects one of 
    void selectWheelHit()
    {
        switch (m_tractionType)
        {
            case TractionType.all_whell_drive:
                whellCols[0].GetGroundHit(out whellHit);
                break;
            case TractionType.front_wheel_drive:
                whellCols[0].GetGroundHit(out whellHit);
                break;
            case TractionType.rear_wheel_drive:
                whellCols[whellCols.Length - 1].GetGroundHit(out whellHit);
                break;
            case TractionType.custom_whell_drive:
                whellCols[(customWheelAxle[0] * 2) - 1].GetGroundHit(out whellHit);
                break;
        }
    }

    // Torque calculation
    float calculateCurrentTorq(int gear, float rpm)
    {
        float currentTorqVoid = 0;

        if (gear != 0)
            currentTorqVoid = (gearRatios[0] * gearRatios[gear]) * motorTorqGraphic.Evaluate(rpm);
        else
            currentTorqVoid = 0;

        return currentTorqVoid;
    }

    //  rpm calculation
    void calculateRpm(float startEngineRpm)
    {
        float oran = HP / maxRpm;
        if (Input.GetAxisRaw("GasPedal") > 0)
        {
            if (currentRpm < maxRpm - 500)
                currentRpm += gasPedal * oran;
            if (currentRpm > maxRpm - 500)
                if (currentRpm != maxRpm - 500)
                    currentRpm -= 50f;
        }
        else
        {
            if (currentRpm >= startEngineRpm)
                currentRpm -= maxRpm * oran;
            if (currentRpm < startEngineRpm)
                currentRpm = startEngineRpm;
        }

    }

    // the process of wheel rotation
    void whellSteer (float angel)
    {
        whellCols[0].steerAngle = angel;
        whellCols[1].steerAngle = angel;
    }

    // Break
    public void addBreakTorq (float torq)
    {        
        if (torq == -1)
        {
            for (int i = 0; i < whellCols.Length; i++)
                whellCols[i].brakeTorque = fixedRpm;
        }
        else
        {
            for (int i = 0; i < whellCols.Length; i++)
                whellCols[i].brakeTorque = 0;

            switch (m_breakType)
            {
                case BreakType.all_whell_drive:
                    for (int i = 0; i < whellCols.Length; i++)
                        whellCols[i].brakeTorque = torq;
                    break;
                case BreakType.front_wheel_drive:
                    for (int i = 0; i < 2; i++)
                        whellCols[i].brakeTorque = torq;
                    break;
                case BreakType.rear_wheel_drive:
                    whellCols[whellCols.Length - 1].brakeTorque = torq;
                    whellCols[whellCols.Length - 2].brakeTorque = torq;
                    break;
                case BreakType.custom_whell_drive:
                    for (int i = 0; i < customWheelAxleBreak.Length; i++)
                    {
                        whellCols[(customWheelAxleBreak[i] * 2) - 1].brakeTorque = torq;
                        whellCols[(customWheelAxleBreak[i] * 2) - 2].brakeTorque = torq;
                    }
                    break;
            }
        }
    }

    // Add torq
    void addTorq (float torq)
    {       
        switch (m_tractionType)
        {
            case TractionType.all_whell_drive:
                for (int i = 0; i < whellCols.Length; i++)
                    whellCols[i].motorTorque = torq;
                break;
            case TractionType.front_wheel_drive:
                for (int i = 0; i < 2; i++)
                    whellCols[i].motorTorque = torq;
                break;
            case TractionType.rear_wheel_drive:
                whellCols[whellCols.Length - 1].motorTorque = torq;
                whellCols[whellCols.Length - 2].motorTorque = torq;
                break;
            case TractionType.custom_whell_drive:
                for (int i = 0; i < customWheelAxle.Length; i++)
                {
                    whellCols[(customWheelAxle[i] * 2) - 1].motorTorque = torq;
                    whellCols[(customWheelAxle[i] * 2) - 2].motorTorque = torq;
                }
                break;
        }
    }

    void meshMove ()
    {
        Vector3 pos;
        Quaternion quat;

        for(int i = 0; i < whellCols.Length; i++)
        {
            whellCols[i].GetWorldPose(out pos, out quat);
            whellMeshs[i].position = pos;
            whellMeshs[i].rotation = quat;
        }
    }

}
