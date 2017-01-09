using Cemu_UI;

namespace MaryJane
{
    public class CUIPlugin : Plugins.IPlugin
    {
        public string Author
        {
            get
            {
                return @"github.com/Tsume";
            }
        }

        public string Description
        {
            get
            {
                return "Updates Wii U titles to the latest version.";
            }
        }

        public string Name
        {
            get
            {
                return "Title Updater";
            }
        }

        public void Initialize()
        {
            Cemu_UI.Program.events.GameSelected += Events_GameSelected;
            Logger.log($@"Initializing Plugin: {Name}");
        }

        private void Events_GameSelected(object sender, Events.GameSelectedArgs e)
        {
            Logger.log($"Selected {e.titleId}");
            Toolbelt.Database.UpdateGame(e.titleId, e.file);
        }
    }
}