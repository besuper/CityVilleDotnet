using System.Xml.Serialization;

namespace CityVilleDotnet.Api.Services.Settings
{
    [Serializable]
    [XmlRoot("settings")]
    public class GameSettings
    {
        [XmlElement("items")]
        public ItemsContainer Items { get; set; }
    }

    [Serializable]
    public class ItemsContainer
    {
        [XmlElement("item")]
        public List<GameItem> Items { get; set; }
    }

    [Serializable]
    public class GameItem
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("requiredLevel")]
        public int? RequiredLevel { get; set; }

        [XmlElement("requiredPopulation")]
        public int? RequiredPopulation { get; set; }

        [XmlElement("cost")]
        public int? Cost { get; set; }

        [XmlElement("construction")]
        public string Construction { get; set; }

        public GameItem() { }
    }

    public class GameSettingsManager
    {
        private static GameSettingsManager _instance;
        private static readonly object _lock = new object();
        private Dictionary<string, GameItem> _items;
        private bool _isInitialized;

        private GameSettingsManager()
        {
            _items = new Dictionary<string, GameItem>();
            _isInitialized = false;
        }

        public static GameSettingsManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new GameSettingsManager();
                        }
                    }
                }
                return _instance;
            }
        }

        public void Initialize(string filePath = "wwwroot/gameSettings.xml")
        {
            if (_isInitialized)
                return;

            XmlSerializer serializer = new XmlSerializer(typeof(GameSettings));

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                var gameSettings = (GameSettings)serializer.Deserialize(fileStream);

                if (gameSettings?.Items?.Items != null)
                {
                    foreach (var item in gameSettings.Items.Items)
                    {
                        if (item.Name != null)
                        {
                            _items[item.Name] = item;
                        }
                    }
                }
            }

            _isInitialized = true;
        }

        public GameItem GetItem(string itemName)
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException("GameSettingsManager not initialized");
            }

            if (_items.TryGetValue(itemName, out GameItem item))
            {
                return item;
            }

            return null;
        }
    }
}