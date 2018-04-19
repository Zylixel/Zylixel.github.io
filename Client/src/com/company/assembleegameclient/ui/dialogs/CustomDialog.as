package com.company.assembleegameclient.ui.dialogs {

import kabam.rotmg.messaging.impl.incoming.DialogResult;

import org.osflash.signals.Signal;
import org.osflash.signals.natives.NativeMappedSignal;

public class CustomDialog extends Dialog {

    public var cancel:Signal;

    public function CustomDialog(arg1:DialogResult) {
        super(arg1.title_, arg1.desc_, "Close", null);
        this.cancel = new NativeMappedSignal(this, LEFT_BUTTON);
    }

}
}//package com.company.assembleegameclient.ui.dialogs