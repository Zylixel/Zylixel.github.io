package kabam.rotmg.arena.view {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.ui.DeprecatedTextButton;
import com.company.assembleegameclient.ui.panels.Panel;

import flash.display.Bitmap;

import kabam.rotmg.arena.util.ArenaViewAssetFactory;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;
import kabam.rotmg.ui.view.SignalWaiter;

public class InformerPanel2 extends Panel {

    private const titleText:TextFieldDisplayConcrete = ArenaViewAssetFactory.returnTextfield(0xFFFFFF, 16, true);

    private var icon:Bitmap;
    var infoButton:DeprecatedTextButton;
    var enterButton:DeprecatedTextButton;
    private var title:String = "Gotcha!";
    private var infoButtonString:String = "Pets.caretakerPanelButtonInfo";
    private var upgradeYardButtonString:String = "ArenaQueryPanel.leaderboard";
    private var waiter:SignalWaiter;
    var type:uint;

    public function InformerPanel2(_arg_1:GameSprite, _arg_2:uint) {
        this.waiter = new SignalWaiter();
        this.type = _arg_2;
        super(_arg_1);
        this.waiter.complete.addOnce(this.alignButton);
        this.handleIcon();
        this.handleIcon2();
        this.handleTitleText();
        this.handleInfoButton();
    }

    private function handleInfoButton():void {
        this.infoButton = new DeprecatedTextButton(32, this.infoButtonString);
        this.waiter.push(this.infoButton.textChanged);
        addChild(this.infoButton);
    }

    private function handleTitleText():void {
        this.titleText.setStringBuilder(new LineBuilder().setParams(this.title));
        this.titleText.x = 65;
        this.titleText.y = 50;
        addChild(this.titleText);
    }


    private function handleIcon():void {
        this.icon = ArenaViewAssetFactory.returnHostBitmap(this.type);
        addChild(this.icon);
        this.icon.x = 5;
        this.icon.y = 15;
    }

    private function handleIcon2():void {
        this.icon = ArenaViewAssetFactory.returnHostBitmap(this.type);
        addChild(this.icon);
        this.icon.x = 130;
        this.icon.y = 15;
    }

    private function alignButton():void {
        this.infoButton.x = (HEIGHT);
        this.infoButton.y = (WIDTH);
    }


}
}