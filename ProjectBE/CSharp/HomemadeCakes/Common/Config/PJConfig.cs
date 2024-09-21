using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace HomemadeCakes.Common.Config
{
    public static class PJConfig
    {
        private static IConfigurationBuilder _builder;

        private static IConfigurationRoot _appConfig;

        private static IConfigurationSection _appSettings;

        private static IConfigurationSection _connections;

        private static bool? _useAuth;

        private static bool? _useRedis;

        public static IConfigurationBuilder Builder
        {
            get
            {
                if (_builder == null)
                {
                    _builder = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory);
                }

                if (File.Exists(Path.Combine(AppContext.BaseDirectory, "appsettings.json")))
                {
                    _builder = _builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
                }

                return _builder;
            }
            set
            {
                _builder = value;
            }
        }

        public static IConfigurationRoot AppConfig
        {
            get
            {
                if (_appConfig == null)
                {
                    _appConfig = Builder.Build();
                }

                return _appConfig;
            }
        }

        public static IConfigurationSection AppSettings
        {
            get
            {
                string text = "AppSettings";
                if (_appSettings == null && IsExist(text))
                {
                    _appSettings = AppConfig.GetSection(text);
                }

                return _appSettings;
            }
        }

        public static IConfigurationSection Connections
        {
            get
            {
                string text = "ConnectionStrings";
                if (_connections == null && IsExist(text))
                {
                    _connections = AppConfig.GetSection(text);
                }

                return _connections;
            }
        }

        public static ClientSetting ClientSetting
        {
            get
            {
                //string text = "LVClientSetting";
                //ClientSetting clientSetting = CacheManager.InProcess.Get<ClientSetting>(text)?.Value;
                //if (clientSetting == null || !clientSetting.Loaded)
                //{
                //    clientSetting = AppSettings.Get<ClientSetting>();
                //    if (clientSetting == null)
                //    {
                //        clientSetting = new ClientSetting();
                //    }

                //    clientSetting.Loaded = true;
                //    CacheManager.InProcess.Add(text, clientSetting);
                //}

                return clientSetting;
            }
        }

        public static Settings Settings
        {
            get
            {
                string text = "LVSettings";
                Settings settings = CacheManager.InProcess.Get<Settings>(text)?.Value;
                if (settings == null || !settings.Loaded)
                {
                    if (AppSettings == null)
                    {
                        settings = new Settings
                        {
                            Loaded = true
                        };
                    }
                    else
                    {
                        settings = AppSettings.Get<Settings>();
                        if (settings == null)
                        {
                            settings = new Settings();
                        }

                        if (settings.Mail == null)
                        {
                            settings.Mail = new Mail();
                        }

                        if (settings.AzureServiceBus == null)
                        {
                            settings.AzureServiceBus = new AzureServiceBus();
                        }

                        if (settings.RabbitMQ == null)
                        {
                            settings.RabbitMQ = new RabbitMQ();
                        }

                        if (settings.Elastic == null)
                        {
                            settings.Elastic = new Elastic();
                        }

                        settings.Loaded = true;
                       // CacheManager.InProcess.Add(text, settings);
                    }
                }

                return settings;
            }
        }

        public static bool UseAuth
        {
            get
            {
                if (!_useAuth.HasValue)
                {
                    _useAuth = Settings.AuthService && !string.IsNullOrEmpty(Settings.Secret);
                }

                return _useAuth.Value;
            }
        }

        public static bool UseRedis
        {
            get
            {
                if (!_useRedis.HasValue)
                {
                    string value = Connections["Redis"];
                    _useRedis = !string.IsNullOrEmpty(value);
                }

                return _useRedis.Value;
            }
        }

        private static bool IsExist(string sectionName)
        {
            return AppConfig.GetChildren().Any((IConfigurationSection item) => item.Key == sectionName);
        }
    }
}
