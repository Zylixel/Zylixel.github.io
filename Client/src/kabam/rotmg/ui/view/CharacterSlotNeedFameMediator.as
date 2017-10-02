package kabam.rotmg.ui.view {
import kabam.rotmg.account.core.signals.OpenMoneyWindowSignal;
import kabam.rotmg.dialogs.control.CloseDialogsSignal;
import kabam.rotmg.core.model.PlayerModel;
import com.company.assembleegameclient.appengine.SavedCharacter;
import kabam.rotmg.classes.model.CharacterClass;
import kabam.rotmg.classes.model.ClassesModel;
import kabam.rotmg.game.model.GameInitData;
import kabam.rotmg.game.signals.PlayGameSignal;
import robotlegs.bender.bundles.mvcs.Mediator;

public class CharacterSlotNeedFameMediator extends Mediator {

    [Inject]
    public var view:CharacterSlotNeedFameDialog;
    [Inject]
    public var closeDialog:CloseDialogsSignal;
    [Inject]
    public var openMoneyWindow:OpenMoneyWindowSignal;
    [Inject]
    public var model:PlayerModel;
    [Inject]
    public var playGame:PlayGameSignal;
    [Inject]
    public var playerModel:PlayerModel;
    [Inject]
    public var classesModel:ClassesModel;

    private function onPlayGame():void {
        this.closeDialog.dispatch();
        var _local_1:SavedCharacter = this.playerModel.getCharacterByIndex(0);
        this.playerModel.currentCharId = _local_1.charId();
        var _local_2:CharacterClass = this.classesModel.getCharacterClass(_local_1.objectType());
        _local_2.setIsSelected(true);
        _local_2.skins.getSkin(_local_1.skinType()).setIsSelected(true);
        var _local_4:GameInitData = new GameInitData();
        _local_4.createCharacter = false;
        _local_4.charId = _local_1.charId();
        _local_4.isNewGame = true;
        this.playGame.dispatch(_local_4);
    }

    override public function initialize():void {
        this.view.PlayGame.add(this.onPlayGame);
        this.view.cancel.add(this.onCancel);
        this.view.setPrice(this.model.getNextCharSlotPrice());
    }

    override public function destroy():void {
        this.view.PlayGame.remove(this.onPlayGame);
        this.view.cancel.remove(this.onCancel);
    }

    public function onCancel():void {
        this.closeDialog.dispatch();
    }



}
}//package kabam.rotmg.ui.view
