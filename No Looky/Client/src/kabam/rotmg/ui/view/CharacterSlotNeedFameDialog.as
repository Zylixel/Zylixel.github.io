package kabam.rotmg.ui.view {
import com.company.assembleegameclient.ui.dialogs.Dialog;

import flash.display.Sprite;
import flash.events.Event;

import kabam.rotmg.text.model.TextKey;

import org.osflash.signals.Signal;

public class CharacterSlotNeedFameDialog extends Sprite {

    public const cancel:Signal = new Signal();
    public const PlayGame:Signal = new Signal();

    private var dialog:Dialog;
    private var price:int;


    public function setPrice(_arg_1:int):void {
        this.price = _arg_1;
        ((((this.dialog) && (contains(this.dialog)))) && (removeChild(this.dialog)));
        this.makeDialog();
        this.dialog.addEventListener(Dialog.LEFT_BUTTON, this.onCancel);
        this.dialog.addEventListener(Dialog.RIGHT_BUTTON,this.onPlayGame);
    }

    private function makeDialog():void {
        this.dialog = new Dialog("Not Enough Fame", "", TextKey.FRAME_CANCEL, "Go Farm!");
        this.dialog.setTextParams(TextKey.CHARACTERSLOTNEEDFAMEDIALOG_PRICE, {"price": this.price});
        addChild(this.dialog);
    }

    public function onCancel(_arg_1:Event):void {
        this.cancel.dispatch();
    }

    public function onPlayGame(_arg_1:Event):void {
        this.PlayGame.dispatch();
    }


}
}//package kabam.rotmg.ui.view
