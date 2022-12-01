using System.Collections.Generic;

namespace Minecraft_News
{
    public class Servers
    {
        public static List<Server> AllServers = new List<Server>();

        public static Server MineplexNews = new Server
        {
            ID = "MineplexNews",
            Name = "Mineplex (News)",
            HomePage = "https://www.mineplex.com/",
            NewsPage = "https://www.mineplex.com/forums/news/",
            NewsLinkNode = "//a[@class='PreviewTooltip']",
            NewsTimeNode = "//a[@class='faint']",
            TitleNode = "//*[@id='content']/div/div[2]/div/div/div[2]/h1",
            DateNode1 = "//span[@class='DateTime']",
            DateNode2 = "//abbr[@class='DateTime']",
            AuthorNode = "//a[@class='username']",
            HTMLNode = "//div[@class='messageContent']",
        };
        public static Server MineplexChangelogs = new Server
        {
            ID = "MineplexChangelogs",
            Name = "Mineplex (Changelogs)",
            HomePage = "https://www.mineplex.com/",
            NewsPage = "https://www.mineplex.com/forums/changelogs/",
            NewsLinkNode = "//a[@class='PreviewTooltip']",
            NewsTimeNode = "//a[@class='faint']",
            TitleNode = "//*[@id='content']/div/div[2]/div/div/div[2]/h1",
            DateNode1 = "//span[@class='DateTime']",
            DateNode2 = "//abbr[@class='DateTime']",
            AuthorNode = "//a[@class='username']",
            HTMLNode = "//div[@class='messageContent']",
        };
        public static Server CosmicSky = new Server
        {
            ID = "CosmicSky",
            Name = "Cosmic Sky",
            HomePage = "https://forum.cosmicsky.com",
            NewsPage = "https://forum.cosmicsky.com/",
            NewsLinkNode = "//a[@class='u-concealed']",
            NewsTimeNode = "//time[@class='u-dt']",
            NewsTimeAttribute = "data-date-string",
            TitleNode = "//h1[@class='p-title-value']",
            DateNode1 = "//time[@class='u-dt']",
            DateAttribute = "data-date-string",
            AuthorNode = "//a[@class='u-concealed']",
            HTMLNode = "//div[@class='bbWrapper']",
        };
        public static Server Cubecraft = new Server
        {
            ID = "Cubecraft",
            Name = "Cubecraft",
            HomePage = "https://www.cubecraft.net/",
            NewsPage = "https://www.cubecraft.net/forums/news/",
            NewsLinkNode = "//a[@class='PreviewTooltip']",
            NewsTimeNode = "//a[@class='faint']",
            TitleNode = "//*[@id='content']/div/div[2]/div[1]/h1",
            DateNode1 = "//span[@class='DateTime']",
            DateNode2 = "//abbr[@class='DateTime']",
            AuthorNode = "//a[@class='username']",
            HTMLNode = "//div[@class='messageContent']",
        };
        public static Server CosmicPvP = new Server
        {
            ID = "CosmicPvP",
            Name = "Cosmic PvP",
            HomePage = "https://forum.cosmicpvp.com/",
            NewsPage = "https://forum.cosmicpvp.com/forums/server-announcements.2/",
            NewsLinkNode = "//a[@class='PreviewTooltip']",
            NewsTimeNode = "//a[@class='faint']",
            TitleNode = "//*[@id='content']/div/div/div[2]/div[1]/h1",
            DateNode1 = "//span[@class='DateTime']",
            DateNode2 = "//abbr[@class='DateTime']",
            AuthorNode = "//a[@class='username']",
            HTMLNode = "//div[@class='messageContent']",
        };
        public static Server Snapcraft = new Server
        {
            ID = "Snapcraft",
            Name = "Snapcraft",
            HomePage = "https://snapcraft.net/",
            NewsPage = "https://snapcraft.net/news/",
            NewsLinkNode = "//a[@class='PreviewTooltip']",
            NewsTimeNode = "//a[@class='faint']",
            TitleNode = "//h1",
            DateNode1 = "//span[@class='DateTime']",
            DateNode2 = "//abbr[@class='DateTime']",
            AuthorNode = "//a[@class='username']",
            HTMLNode = "//div[@class='messageContent']",
        };
        public static Server Desteria = new Server
        {
            ID = "Desteria",
            Name = "Desteria",
            HomePage = "https://desteria.com",
            NewsPage = "https://desteria.com/community/forums/desteria-announcements.6/",
            NewsLinkNode = "//a[@class='']",
            NewsTimeNode = "//time[@class='u-dt']",
            NewsTimeAttribute = "data-date-string",
            TitleNode = "//h1[@class='p-title-value']",
            DateNode1 = "//time[@class='u-dt']",
            DateAttribute = "data-date-string",
            AuthorNode = "//a[@class='username  u-concealed']",
            HTMLNode = "//div[@class='message-content js-messageContent']",
        };
        public static Server VanityMC = new Server
        {
            ID = "VanityMC",
            Name = "VanityMC",
            HomePage = "https://www.vanitymc.co",
            NewsPage = "https://www.vanitymc.co/forums/announcements/",
            NewsLinkNode = "//a[@class='']",
            NewsTimeNode = "//time[@class='u-dt']",
            NewsTimeAttribute = "data-date-string",
            TitleNode = "//h1[@class='p-title-value']",
            DateNode1 = "//time[@class='u-dt']",
            DateAttribute = "data-date-string",
            AuthorNode = "//a[@class='username ']",
            HTMLNode = "//div[@class='message-content js-messageContent']",
        };
        public static Server MCGamer = new Server
        {
            ID = "MCGamer",
            Name = "MCGamer",
            HomePage = "http://mcgamer.net/",
            NewsPage = "http://mcgamer.net/forums/news/",
            NewsLinkNode = "//a[@class='PreviewTooltip']",
            NewsTimeNode = "//a[@class='faint']",
            TitleNode = "//*[@id='content']/div/div/div[1]/div/div/div[3]/h1",
            DateNode1 = "//abbr[@class='DateTime']",
            DateNode2 = "//span[@class='DateTime']",
            AuthorNode = "//a[@class='username']",
            HTMLNode = "//div[@class='messageContent']",
        };
        public static Server Backplay = new Server
        {
            ID = "Backplay",
            Name = "Backplay",
            HomePage = "https://backplay.net",
            NewsPage = "https://backplay.net/",
            NewsLinkNode = "//a[@class='button button--icon']",
            NewsTimeNode = "//time[@class='u-dt']",
            NewsTimeAttribute = "data-date-string",
            TitleNode = "//h1[@class='p-title-value']",
            DateNode1 = "//time[@class='u-dt']",
            DateAttribute = "data-date-string",
            AuthorNode = "//a[@class='username  u-concealed']",
            HTMLNode = "//div[@class='bbWrapper']",

        };
        /*
        public static Server Minecraft = new Server
        {
            Name = "Minecraft",
        };
        public static Server Lunar = new Server
        {
            Name = "Lunar",
        };
        */
    }
}
