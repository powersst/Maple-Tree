using CefSharp;
using Cemu_UI;

namespace MaryJane
{
    public class UpdaterPlugin : Plugins.IPlugin
    {
        public string Author => @"github.com/Tsume";

        public string Description => "Updates Wii U titles to the latest version.";

        public string Name => "Title Updater";

        public void Initialize()
        {
            Database.Initialize();

            Cemu_UI.Program.main.browser.RegisterJsObject("Updater", Toolbelt.Database);
            Cemu_UI.Program.main.browser.EvaluateScriptAsync(@"$('#game-info').append('<a id=""update"" onclick=""Updater.updateGame(cemu.selected.titleId, cemu.selected.game);"" style=""font - size: 18px; display: block; width: 200px; margin: 15px 0 0; padding: 5px 20px; cursor: pointer; transition: 1s; text - align: center; text - decoration: none; color: #fff; border-radius: 8px; background: #00acd2;"">Update Game</a>');");

            Cemu_UI.Program.events.GameSelected += Events_GameSelected;

            Logger.log($@"Initializing Plugin: {Name}");
        }

        private void Events_GameSelected(object sender, Events.GameSelectedArgs e)
        {
            Logger.log($"Selected {e.titleId}");
            //Toolbelt.Database.UpdateGame(e.titleId, e.file);
        }
    }
}