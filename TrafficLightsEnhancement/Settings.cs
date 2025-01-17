using System.Reflection;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.SceneFlow;
using Game.Settings;
using Game.UI.Widgets;
using Unity.Entities;

namespace C2VM.TrafficLightsEnhancement;

[FileLocation("C2VM-TrafficLightsEnhancement")]
[SettingsUIGroupOrder(["General", "Default", "Version"])]
[SettingsUIShowGroupName]
public class Settings : ModSetting
{
    [SettingsUISection("General")]
    [SettingsUIDropdown(typeof(Settings), "GetLanguageValues")]
    public string m_LocaleOption
    {
        get
        {
            return m_Locale;
        }
        set
        {
            m_Locale = value;
            Colossal.Localization.LocalizationManager localizationManager = Game.SceneFlow.GameManager.instance.localizationManager;
            localizationManager.GetType().GetTypeInfo().GetDeclaredMethod("NotifyActiveDictionaryChanged").Invoke(localizationManager, null);
        }
    }

    public string m_Locale;

    [SettingsUISection("Version")]
    public string m_ReleaseChannel
    {
        get
        {
            #if SHOW_CANARY_BUILD_WARNING
            return "Canary";
            #else
            return "Alpha";
            #endif
        }
    }

    [SettingsUISection("Version")]
    public string m_TleVersion => Mod.m_InformationalVersion.Substring(0, 20);

    [SettingsUISection("Version")]
    public string m_LaneSystemVersion => C2VM.CommonLibraries.LaneSystem.Mod.m_InformationalVersion.Substring(0, 20);

    [SettingsUISection("Default")]
    public bool m_DefaultSplitPhasing { get; set; }

    [SettingsUISection("Default")]
    public bool m_DefaultAlwaysGreenKerbsideTurn { get; set; }

    [SettingsUISection("Default")]
    public bool m_DefaultExclusivePedestrian { get; set; }

    [SettingsUISection("Default")]
    [SettingsUIButton]
    [SettingsUIConfirmation(null, null)]
    [SettingsUIDisableByCondition(typeof(Settings), "IsNotInGame")]
    public bool m_ForceNodeUpdate
    {
        get
        {
            return false;
        }
        set
        {
            EntityQuery entityQuery = Mod.m_World.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<Game.Net.TrafficLights>());
            Mod.m_World.EntityManager.AddComponent<Game.Common.Updated>(entityQuery);
        }
    }

    [SettingsUIHidden]
    public bool m_HasReadLdtRetirementNotice { get; set; }

    public Settings(IMod mod) : base(mod)
    {
        SetDefaults();
    }

    public override void SetDefaults()
    {
        m_LocaleOption = "auto";
        m_DefaultSplitPhasing = false;
        m_DefaultAlwaysGreenKerbsideTurn = false;
        m_DefaultExclusivePedestrian = false;
        m_HasReadLdtRetirementNotice = false;
    }

    public override void Apply()
    {
        base.Apply();
    }

    public static DropdownItem<string>[] GetLanguageValues()
    {
        DropdownItem<string>[] list = [
            new DropdownItem<string>
            {
                value = "auto",
                displayName = "Auto"
            },
            new DropdownItem<string>
            {
                value = "de-DE",
                displayName = "German"
            },
            new DropdownItem<string>
            {
                value = "en-US",
                displayName = "English"
            },
            new DropdownItem<string>
            {
                value = "es-ES",
                displayName = "Spanish"
            },
            new DropdownItem<string>
            {
                value = "fr-FR",
                displayName = "French"
            },
            // new DropdownItem<string>
            // {
            //     value = "it-IT",
            //     displayName = "Italian"
            // },
            new DropdownItem<string>
            {
                value = "ja-JP",
                displayName = "Japanese"
            },
            new DropdownItem<string>
            {
                value = "ko-KR",
                displayName = "Korean"
            },
            new DropdownItem<string>
            {
                value = "nl-NL",
                displayName = "Dutch"
            },
            new DropdownItem<string>
            {
                value = "pl-PL",
                displayName = "Polish"
            },
            new DropdownItem<string>
            {
                value = "pt-BR",
                displayName = "Portuguese (Brazil)"
            },
            new DropdownItem<string>
            {
                value = "ru-RU",
                displayName = "Russian"
            },
            new DropdownItem<string>
            {
                value = "zh-HANS",
                displayName = "Chinese (Simplified)"
            },
            new DropdownItem<string>
            {
                value = "zh-HANT",
                displayName = "Chinese (Traditional)"
            },
            new DropdownItem<string>
            {
                value = "zh-HK",
                displayName = "Chinese (Hong Kong)"
            },
            new DropdownItem<string>
            {
                value = "zh-TW",
                displayName = "Chinese (Taiwan)"
            }
        ];
        return list;
    }

    public bool IsNotInGame()
    {
        return GameManager.instance.gameMode != Game.GameMode.Game;
    }
}