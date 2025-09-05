using S7.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviarDatos : MonoBehaviour
{
    // Configuración del PLC
    public string plcIP = "127.0.0.1"; // Cambia a la IP de tu PLC
    public CpuType cpuType = CpuType.S71200;
    public short rack = 0;
    public short slot = 1;

    public string memoryAddress = "DB1.DBX0.0"; // Dirección de memoria de prueba
    private Plc plc;

    // Intervalo de lectura/escritura
    public float pollInterval = 0.5f;

    void Start()
    {
        // Crear instancia del PLC
        plc = new Plc(cpuType, plcIP, rack, slot);

        try
        {
            plc.Open();
            if (plc.IsConnected)
            {
                Debug.Log("✅ Conexión con PLC establecida");
                StartCoroutine(ReadWriteLoop());
            }
            else
            {
                Debug.LogError("❌ No se pudo conectar al PLC");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al conectar al PLC: " + e.Message);
        }
    }

    IEnumerator ReadWriteLoop()
    {
        bool testValue = false;

        while (true)
        {
            if (plc != null && plc.IsConnected)
            {
                // --- Enviar valor (toggle) ---
                plc.Write(memoryAddress, testValue);
                Debug.Log($"Se escribió {testValue} en {memoryAddress}");

                // --- Leer valor ---
                object readValue = plc.Read(memoryAddress);
                if (readValue is bool b)
                {
                    Debug.Log($"Se leyó {b} desde {memoryAddress}");
                }
                else
                {
                    Debug.LogWarning($"Valor leído no es booleano: {readValue}");
                }

                // Alternar valor para la próxima escritura
                testValue = !testValue;
            }

            yield return new WaitForSeconds(pollInterval);
        }
    }

    void OnApplicationQuit()
    {
        if (plc != null && plc.IsConnected)
        {
            plc.Close();
            Debug.Log("🔒 Conexión con PLC cerrada");
        }
    }
}
