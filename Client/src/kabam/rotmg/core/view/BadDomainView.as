package kabam.rotmg.core.view {
import flash.display.Sprite;
import flash.text.TextField;
import flash.text.TextFormat;

public class BadDomainView extends Sprite {

    private static const BAD_DOMAIN_TEXT:String = ((('<p align="center"><font color="#FFFFFF">Loading Game')));

    public function BadDomainView() {
        var _local_1:TextField = new TextField();
        _local_1.selectable = false;
        var _local_2:TextFormat = new TextFormat();
        _local_2.size = 20;
        _local_1.defaultTextFormat = _local_2;
        _local_1.htmlText = BAD_DOMAIN_TEXT;
        _local_1.width = 800;
        _local_1.y = ((600 / 2) - (_local_1.height / 2));
        addChild(_local_1);
    }

}
}//package kabam.rotmg.core.view
