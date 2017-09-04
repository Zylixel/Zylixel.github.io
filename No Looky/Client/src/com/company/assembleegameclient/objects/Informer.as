package com.company.assembleegameclient.objects {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.ui.panels.Panel;

import kabam.rotmg.arena.view.InformerPanel2;

public class Informer extends GameObject implements IInteractiveObject {

    public function Informer(_arg_1:XML) {
        super(_arg_1);
        isInteractive_ = true;
    }

    public function getPanel(_arg_1:GameSprite):Panel {
        return (new InformerPanel2(_arg_1, objectType_));
    }


}
}//package com.company.assembleegameclient.objects