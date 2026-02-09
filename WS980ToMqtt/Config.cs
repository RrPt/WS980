using RpToolsNs;
using System;
using System.Collections.Generic;

public class Config : ConfigBase
{
    // als Abkürzung kann folgende definition verwendet werden 
    public static Config Instance
    {
        get => GenericConfig<Config>.Instance;
    }   // nur benötigt damit die Aufrufe wie bisher  Config.Instance. statt GenericConfig<Config>.Instance.
        //... alle Parameter als Properies, z.B. auch Listen von anderen Klassen u.s.w.

    private string _mqttBroker = "192.168.22.101";
    public string Mqtt_Broker
    {
        get => RpTools.FixLocalBrokerIp(_mqttBroker);
        set => _mqttBroker = value;
    } 

    public string Mqtt_User
    {
        get;
        set;
    } = "hv";

    private CryptedString mqtt_password;
    public CryptedString Mqtt_password
    {
        get => mqtt_password; 
        set => mqtt_password = GenericConfig<Config>.Instance.EncryptIfNessesary(value, null, CryptedStringEncryptionScopeType.LocalMachine);
    }

    /// <summary>
    /// Unter diesem Topic werden die Daten veröffentlicht
    /// </summary>
    public string Mqtt_TopicBase
    {
        get ;
        set ;
    } = "weatherstation/";

    /// <summary>
    /// Wie oft werden die Werte abgefragt und übertragen
    /// </summary>
    public int IntervalInSeconds
    {
        get;
        set;
    } = 15;
}