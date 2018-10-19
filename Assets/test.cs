using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MQTTnet;
using MQTTnet.Server;
using System;
using MQTTnet.Diagnostics;

public class test : MonoBehaviour {

    private IMqttServer mqttServer;

	// Use this for initialization
	async void Start () {
        // Start a MQTT server.
        mqttServer = new MqttFactory().CreateMqttServer();
        var ipAddress = new byte[] { 0, 0, 0, 0 };
        var ipObject = new System.Net.IPAddress(ipAddress);
        var options = new MqttServerOptionsBuilder()
            .WithDefaultEndpointBoundIPAddress(ipObject)
            .WithDefaultEndpointPort(1883);

        // Write all trace messages to the console window.
        MqttNetGlobalLogger.LogMessagePublished += (s, e) =>
        {
            var trace = $">> [{e.TraceMessage.Timestamp:O}] [{e.TraceMessage.ThreadId}] [{e.TraceMessage.Source}] [{e.TraceMessage.Level}]: {e.TraceMessage.Message}";
            if (e.TraceMessage.Exception != null)
            {
                trace += Environment.NewLine + e.TraceMessage.Exception.ToString();
            }

            Debug.Log(trace);
        };

        try
        {
            await mqttServer.StartAsync(options.Build());
        } catch (Exception e)
        {
            // testing...
            Debug.Log("exception!");
            Debug.Log(e.Message);
        }
        

        mqttServer.ClientConnected += (s, e) =>
        {
            Debug.Log("Client connected " + e.ClientId);
        };
        mqttServer.Started += (s, e) =>
        {
            Debug.Log("Server was started");
        };
        mqttServer.Stopped += (s, e) =>
        {
            Debug.Log("Server was stopped");
        };

        mqttServer.ApplicationMessageReceived += (s, e) =>
        {
            // TODO split values up! & find out what is what.
            // -> parse values like in the code from Ricky.
            Debug.Log("application message received");
            var payload = string.Concat(e.ApplicationMessage.Payload);
            Debug.Log(payload);
        };

        Debug.Log(mqttServer.Options.DefaultEndpointOptions.Port);
        Debug.Log(mqttServer.Options.DefaultEndpointOptions.BoundInterNetworkAddress);
    }
	
	// Update is called once per frame
	void Update () {
        /*Debug.Log(mqttServer.Options.DefaultEndpointOptions.Port);
        Debug.Log(mqttServer.Options.DefaultEndpointOptions.BoundInterNetworkAddress);
        */
    }
}
