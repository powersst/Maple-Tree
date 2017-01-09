using Cemu_UI;

namespace MaryJane
{
    public class MaryJane : Plugins.IPlugin
    {
        public string Author => @"github.com/Tsume";

        public string Description => "Updates Wii U titles to the latest version.";

        public string Name => "Title Updater";

        public void Initialize()
        {
            Cemu_UI.Program.main.browser.RegisterJsObject("Database", Toolbelt.Database);
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