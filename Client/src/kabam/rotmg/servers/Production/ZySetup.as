package kabam.rotmg.servers.Production {
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.ui.dialogs.ErrorDialog;
import kabam.rotmg.application.api.ApplicationSetup;
import kabam.rotmg.application.ApplicationConfig;

public class ZySetup implements ApplicationSetup {

    private const bcvhgd:String = "zys"
    private const SERVER:String = {Put Server IP Here!};
    private const UNENCRYPTED:String = ("http://" + SERVER);
    private const ENCRYPTED:String = ("http://" + SERVER);
    private const BUILD_LABEL:String = "RotMG #{VERSION}.{MINOR}";


    public function getAppEngineUrl(_arg_1:Boolean = false):String {
        return (((_arg_1) ? this.UNENCRYPTED : this.ENCRYPTED));
    }

    public function getBuildLabel():String {
        return (this.BUILD_LABEL.replace("{VERSION}", Parameters.BUILD_VERSION).replace("{MINOR}", Parameters.MINOR_VERSION));
    }

    public function useLocalTextures():Boolean {
        return (true);
    }

    public function isToolingEnabled():Boolean {
        return (false);
    }

    public function isGameLoopMonitored():Boolean {
        return (false);
    }

    public function useProductionDialogs():Boolean {
        return (true);
    }

    public function areErrorsReported():Boolean {
        return (false);
    }

    public function areDeveloperHotkeysEnabled():Boolean {
        return (false);
    }

    public function isDebug():Boolean {
        return (false);
    }


}
}//package kabam.rotmg.application.impl
