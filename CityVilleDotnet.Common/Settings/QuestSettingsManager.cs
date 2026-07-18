using Microsoft.Extensions.Logging;
using System.Xml.Serialization;
using CityVilleDotnet.Common.Settings.QuestSettings;

namespace CityVilleDotnet.Common.Settings;

public class QuestSettingsManager
{
    public static readonly HashSet<string> TaskActions = ["seenQuest"];
    public static readonly Dictionary<string, List<string>> QuestStartInventoryItem = new()
    {
        ["qm_storage_warehouse"] = ["warehouse"],
        ["qm_visitor_center"] = ["mun_visitorcenter"],
        ["qm_factory_1"] = ["factory_premiumgoods"],
        ["qm_clerk_office"] = ["mun_clerkoffice"],
        ["qm_rent_collector"] = ["mun_rentcollectordepot"],
        ["qm_hardware1"] = ["bus_hardwarestore"],
        ["qm_cruiseship_dock"] = ["dock_house"],
        ["qm_arctic_zoo"] = ["enclosure_arctic"],
        ["qm_zoo_1"] = ["enclosure_jungle"],
        ["qm_bridge_1"] = ["bridge_standard"],
        ["qm_promo_kungfupanda"] = ["bus_drivein"],
        ["qf_mall_construction"] = ["mall"],
        ["qf_ticket_booth"] = ["mun_carnivalticketbooth"],
        ["qf_carnival_quest"] = ["mun_streetcarnivalsmall"],
        ["qm_green1"] = ["deco_windfarm"],
        ["qf_tikisocial"] = ["tiki_social_business"],
        ["qf_casinosocial"] = ["casino_social_business"],
        ["qf_hotels"] = ["resort_hotel_low"],
        ["qf_sandcastle"] = ["mun_shellticketbooth"],
        ["qf_beach_carnival_quest"] = ["mun_shellcarnivalsmall"],
        ["qm_landmark_sailboat_hotel"] = ["hotel_sailboat_low"],
        ["qf_dam_1"] = ["mun_dam"],
        ["qm_clown"] = ["mun_circus_clowncollege"],
        ["qm_remodeling"] = ["mun_constructioncompany"],
        ["qm_promo_bbuy"] = ["bus_electronicsstore"],
        ["qm_promo_2bbuy"] = ["bus_electronicsstore", "deco_bestbuygift"],
        ["qf_gardens"] = ["garden_roses_4x4"],
        ["qf_cars"] = ["mun_cargarage"],
        ["qf_carcraft"] = ["mun_customcarshop"],
        ["q_governor_run_toy_1"] = ["mun_duckfactory"],
        ["qm_stadiums_baseball"] = ["mun_baseballstadium"],
        ["qf_enrique_1"] = ["atr_concert"],
        ["qf_mb_1"] = ["atr_mb_concert"],
        ["qm_stadiums_soccer"] = ["mun_soccerstadium"],
        ["qm_spyagency"] = ["mun_spy_building"],
        ["qm_citysymphony"] = ["mun_metro_symphonyhall"],
        ["qm_prodigystudios"] = ["mun_metro_recordingstudio"],
        ["qm_coliseum"] = ["mun_coliseum"],
        ["q_governor_run_act2_nature_1"] = ["mun_conservatory"],
        ["qf_animalrescue"] = ["mun_animalrescue"],
        ["qf_universities"] = ["univ_library"],
        ["qf_candybooth"] = ["mun_candyticketbooth"],
        ["qm_farmers"] = ["mun_insurancebuilding"],
        ["q_cokezero"] = ["bus_cocacolaplant"],
        ["qf_halloween_3"] = ["mun_trickortreathouse"],
        ["qf_halloween_2_2"] = ["tower_of_terror_regular"],
        ["qt_cidermill"] = ["mun_cidermill"],
        ["qm_applefarm"] = ["mun_appleorchard"],
        ["q_halloweenneighborhood"] = ["hood_halloween"],
        ["q_xmas_saga_act1_1"] = ["santas_workshop"],
        ["qm_swissmuseum"] = ["mun_swiss_museum"],
        ["q_elder3_cookie_baking_3"] = ["mun_cookingschool"],
        ["qf_minicoreloop_gov"] = ["res_govmansion"],
        ["qm_skyscrapers1"] = ["skyscraper_cetronas_center_1"],
        ["qm_skyscrapers2_1"] = ["skyscraper_cetronas_center_1"],
        ["qt_russianresidence"] = ["res_russian"],
        ["qt_venetianresidence"] = ["lm_venetianpalace"],
    };
    
    private static QuestSettingsManager? _instance;
    private static readonly object Lock = new();
    private readonly Dictionary<string, QuestItem> _items = new();
    private bool _isInitialized = false;

    public static QuestSettingsManager Instance
    {
        get
        {
            if (_instance is null)
            {
                lock (Lock)
                {
                    _instance ??= new QuestSettingsManager();
                }
            }

            return _instance;
        }
    }

    public void Initialize(ILogger<QuestSettingsManager> logger, string path = "wwwroot/questSettings.xml")
    {
        if (_isInitialized)
            return;

        if (!File.Exists(path))
        {
            logger.LogError("Missing file assets ({Path})", path);
            return;
        }

        var serializer = new XmlSerializer(typeof(GameQuests));

        using (var fileStream = new FileStream(path, FileMode.Open))
        {
            var content = serializer.Deserialize(fileStream);

            if (content is null)
            {
                logger.LogError("Can't deserialize questSettings.xml file");
                return;
            }

            var gameSettings = (GameQuests)content;

            foreach (var item in gameSettings.Quests)
            {
                _items[item.Name] = item;

                foreach (var task in item.Tasks.Tasks)
                {
                    TaskActions.Add(task.Action);
                }
            }
        }

        logger.LogInformation("Loaded questSettings.xml with {ItemsCount} items and {TaskActionsCount} task actions", _items.Count, TaskActions.Count);

        _isInitialized = true;
    }

    public QuestItem? GetItem(string itemName)
    {
        if (!_isInitialized)
            throw new InvalidOperationException("QuestSettingsManager not initialized");

        return _items.TryGetValue(itemName, out var item) ? item : null;
    }

    public IEnumerable<QuestItem> GetAllQuests()
    {
        if (!_isInitialized)
            throw new InvalidOperationException("QuestSettingsManager not initialized");

        return _items.Values;
    }
}