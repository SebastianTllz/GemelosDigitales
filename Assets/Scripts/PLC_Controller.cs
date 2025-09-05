using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using S7.Net;

public class PLC_Controller : MonoBehaviour
{
    public RotateSubmarineBlades motor;      // Referencia al script que rota las hélices
    public string memoryAddress = "DB2.DBX0.0"; // Dirección en el PLC para encender/apagar motor

    private Plc plc;

    [Header("PLC Settings")]
    public string ip = "127.0.0.1";   // IP de tu PLC
    public CpuType cpuType = CpuType.S71200;
    public short rack = 0;
    public short slot = 1;

    private void Start()
    {
        plc = new Plc(cpuType, ip, rack, slot);
        try
        {
            plc.Open();
            Debug.Log("Conexión con PLC establecida");
        }
        catch
        {
            Debug.LogError("No se pudo conectar al PLC");
        }

        StartCoroutine(ReadMotorState());
    }

    IEnumerator ReadMotorState()
    {
        while (true)
        {
            if (plc != null && plc.IsConnected)
            {
                // Leer el estado del bit del PLC
                object value = plc.Read(memoryAddress);

                if (value is bool motorState)
                {
                    motor.RotateObject(motorState); // Actualiza la rotación
                }
                else
                {
                    Debug.LogWarning("El valor leído no es booleano");
                }
            }
            yield return new WaitForSeconds(0.2f); // Leer cada 200ms
        }
    }

    private void OnApplicationQuit()
    {
        if (plc != null && plc.IsConnected)
        {
            plc.Close();
        }
    }
}
